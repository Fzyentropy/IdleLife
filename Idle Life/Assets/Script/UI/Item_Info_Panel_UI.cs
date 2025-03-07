using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class Item_Info_Panel_UI : MonoBehaviour
{
    [Header("当前物品")] 
    // [ReadOnly] public Item Current_Item;   // 当前 Item

    [Space(10)] [Header("Panel 模块")] 
    
    [Space(5)] [Header("物品 Info")] 
    public GameObject Panel_Item_Info;
    [Space(5)]
    public Image Item_Info_Image;
    public TMP_Text Item_Info_Label;
    public TMP_Text Item_Info_Description;
    
    [Space(5)] [Header("物品 Info")]
    public GameObject Panel_Item_Equip;
    public GameObject Panel_Item_Upgrade;
    public GameObject Panel_Item_Use;
    public GameObject Panel_Item_Sell;


    private void OnEnable()
    {
        // 设置模块
        Check_And_Set_Panel();
    }


    public void Check_And_Set_Panel()     // 根据当前 Item 设置显示模块（更新）
    {
        // 更新基础信息
        Item_Info_Image.sprite = PanelManager.PM.Current_Open_Item.Item_Image;
        Item_Info_Label.text = PanelManager.PM.Current_Open_Item.Item_Label;
        Item_Info_Description.text = PanelManager.PM.Current_Open_Item.Item_Description;
        
        Panel_Item_Use.SetActive(false);
        Panel_Item_Equip.SetActive(false);
        Panel_Item_Upgrade.SetActive(false);
        Panel_Item_Sell.SetActive(false);
        
        // 根据输入的 item 设置哪些模块出现
        if (PanelManager.PM.Current_Open_Item.Item_Modules.Count > 0)
        {
            // 动态显示模块面板
            foreach (var module in PanelManager.PM.Current_Open_Item.Item_Modules)
            {
                switch (module)
                {
                    case ItemModule_Equipment equipment:        // 装备模块
                    {
                        Panel_Item_Equip.SetActive(true);
                        // setup equipment panel TODO
                        break;
                    }
                    
                    case ItemModule_Upgrade upgrade:        // 升级模块
                    {
                        Panel_Item_Upgrade.SetActive(true);
                        // setup upgrade panel TODO
                        break;
                    }
                    
                    case ItemModule_Use use:            // 使用模块
                    {
                        Panel_Item_Use.SetActive(true);
                        // setup use panel TODO
                        break;
                    }
                    
                    case ItemModule_Sell sell:        // 升级模块
                    {
                        Panel_Item_Sell.SetActive(true);
                        // setup sell panel todo
                        break;
                    }
                    
                }
            }
        }
        
        
        
    }
}
