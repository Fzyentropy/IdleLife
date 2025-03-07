using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;

public class Inventory_Item_UI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //------------------------------------------------------------------------------------------------------------------
    
    [Header("Item Scriptable")]
    public Item item_instance;

    [Header("UI")] 
    // public TMP_Text item_label;     // Item 名称，暂时不显示
    public Image item_image;            // icon
    public TMP_Text item_amount;
    public Color cursor_hover_color;     // 鼠标悬停时 icon 颜色


    //------------------------------------------------------------------------------------------------------------------
    
    private void Start()
    {
        Check_Item_Instance();
        Item_Setup();
    }

    private void Check_Item_Instance()
    {
        if (item_instance == null)
            Debug.LogError("Inventory_Item_UI(prefab): Item 实例未设置");
    }
    
    
    //------------------------------------------------------------------------------------------------------------------
    
    
    private void Item_Setup()       // 设置 Item 的外观，等不变的因素
    {
        // item_label = item_instance.Item_Label;              // 设置 Item名称
        item_image.sprite = item_instance.Item_Image;       // 设置 Item图片
    }
    

    public void Update_Item(Item item, int amount)      // 设置 Item实例 和 拥有的Item数量
    {
        item_instance = item;
        // item_label = item_instance.Item_Label;              // 设置 Item名称
        item_image.sprite = item_instance.Item_Image;       // 设置 Item图片
        item_amount.text = amount.ToString();
    }
    
    
    //------------------------------------------------------------------------------------------------------------------
    // 点击、悬停 时触发的操作
    
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        Hover_Item();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Exit_Item();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Click_Item();
    }
    
    
    public void Click_Item()
    {
        // Debug.Log(Check_Item_Info());
        
        if (PanelManager.PM.Current_Open_Item != null && PanelManager.PM.Current_Open_Item == item_instance)
            PanelManager.PM.Close_Item_Info_Panel();
        else
            PanelManager.PM.Open_Item_Info_Panel(item_instance);
    }

    public void Hover_Item()
    {
        item_image.color = cursor_hover_color;
    }
    
    public void Exit_Item()
    {
        item_image.color = Color.white;
    }
    
    
    //------------------------------------------------------------------------------------------------------------------


    // 检测Item的Scriptable Object 实例是否能顺利储存inspector中设置的数据（能否正确序列化）
    public string Check_Item_Info()         
    {
        string log = new string(
            $"你点击了 {item_instance.Item_Label} " +
            $"\n 该物品的类别是：{item_instance.Item_Type}");

        if (item_instance.Item_Modules.Count > 0)
        {
            foreach (var module in item_instance.Item_Modules)
            {
                if (module is ItemModule_Upgrade upgrade)
                {
                    log += $"\n 该物品具备“升级”模块，升级所需经验值：{upgrade.expRequired}，升级成为物品：{upgrade.Upgrade_To}";
                }
                else if (module is ItemModule_ShopItem shopItem)
                {
                    log += $"\n 该物品具备“商店售卖”模块，价格：{shopItem.Item_Price}，商店中总商品数量：{shopItem.Item_Total_Amount}";
                }
                else if (module is ItemModule_Equipment equipment)
                {
                    log += $"\n 该物品具备“装备”模块，装备类型：{equipment.Equipment_Type}";
                }
                else if (module is ItemModule_Use use)
                {
                    string log_use = new string($"\n 该物品具备“使用”模块，使用功能有：");

                    foreach (var func in use.use_funcs)
                    {
                        if (func is ItemModule_UseFunc_AddSatiety func_add_satiety)
                        {
                            log_use += $"\n --- 增加饱腹值：{func_add_satiety.Add_Satiety_Amount} ";
                        }
                    }
                    
                    log += log_use;
                }
            
            }
        }
        else
        {
            log += "\n 该物品没有模块组件";
        }
        

        return log;
    }
    
    
    
    
    
}
