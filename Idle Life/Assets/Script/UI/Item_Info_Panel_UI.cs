using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Item_Info_Panel_UI : MonoBehaviour
{
    
    // [Header("当前物品")] [ReadOnly] public Item Current_Item;   // 当前 Item
    
    [Space(10)] [Header("Panel 模块")] 
    
    //------------------------------------------------------------------------------------------------------------------
    
    [Space(5)] [Header("物品 Info")] 
    public GameObject Panel_Item_Info;
    [Space(5)]
    public Image Item_Info_Image;
    public TMP_Text Item_Info_Label;
    public TMP_Text Item_Info_Description;
    
    //------------------------------------------------------------------------------------------------------------------
    
    [Space(5)] [Header("Use 使用模块")]
    public GameObject Panel_Item_Use;
    [Space(3)]
    public Slider Use_Amount_Slider;        // 数量条
    public TMP_Text maxQuantityText;        // 最大数量 Text
    public TMP_Text selectedQuantityText;    // 当前选择数量 Text
    public Button Use_Button;           // 使用按钮

    private ItemModule_Use item_module_use;   // 当前 Item 的使用模块
    private int _currentItemCount;      // 当前拥有的该Item数量（选择条最大值）
    [Sirenix.OdinInspector.ReadOnly] public int _selectedQuantity;      // 选择的数量（选择条当前值）

    private int handlerPosition;   // 使用数量 Slider的 Handler位置 实际存储
    private int _handlerPosition    // 使用数量 Slider的 Handler位置 访问接口
    {
        get
        {
            if (handlerPosition < 1)
                return 1;
            if (handlerPosition > _currentItemCount)
                return _currentItemCount;
            
            return handlerPosition;
        }
        set 
        {
            handlerPosition = value;
        }
    }       
    
    //------------------------------------------------------------------------------------------------------------------
    
    [Space(5)] [Header("Equip 装备模块")]
    public GameObject Panel_Item_Equip;
    
    //------------------------------------------------------------------------------------------------------------------
    
    [Space(5)] [Header("Upgrade 升级模块")]
    public GameObject Panel_Item_Upgrade;
    
    //------------------------------------------------------------------------------------------------------------------
    
    [Space(5)] [Header("Sell 出售模块")]
    public GameObject Panel_Item_Sell;

    //------------------------------------------------------------------------------------------------------------------

    private void Start()
    {
        Check_GameObject_Setup();   // 检查 GameObject 拖拽
    }

    private void OnEnable()
    {
        if (PanelManager.PM.Current_Open_Item == null)
        {
            PanelManager.PM.Close_Item_Info_Panel();
        }
        
        // 设置模块
        Check_And_Set_Panel();
        
        _handlerPosition = 1;  // Use Panel的数量Slider 初始选择数量设置为 1
    }

    private void Check_GameObject_Setup()
    {
        if (
            Panel_Item_Info == null
            || Panel_Item_Use == null
            || Panel_Item_Equip == null
            || Panel_Item_Upgrade == null
            || Panel_Item_Sell == null
        ) { Debug.LogError("Panel Sections 未完全设置"); }
        
        if (
            Use_Amount_Slider == null
            || maxQuantityText == null
            || selectedQuantityText == null
            || Use_Button == null
        ) { Debug.LogError("Use Panel UI元素未完全设置"); }
    }
    
    
    //------------------------------------------------------------------------------------------------------------------
    // 设置 Panel

    public void Check_And_Set_Panel()     // 根据当前 Item 设置显示模块（更新）
    {
        
        // 获取当前 Item 拥有数量
        _currentItemCount = Inventory.IVT.Player_Items[PanelManager.PM.Current_Open_Item.Item_Id];
        
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
                        item_module_use = use;  // 获取到当前（拥有使用模块的）Item 的使用模块，以便调用
                        Update_Use_Panel_Settings();     // 更新 Use面板
                        Panel_Item_Use.SetActive(true);
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
        
        Debug.Log("Item Info Panel 已刷新");
    }
    
    
    //------------------------------------------------------------------------------------------------------------------
    // Use 使用模块相关逻辑
    
    public void Update_Use_Panel_Settings()                 // 更新 Use面板
    {
        // 更新 Slider
        Use_Amount_Slider.minValue = 1;
        // minQuantityText.text = "1";  // 手动定死，无需写出
        Use_Amount_Slider.maxValue = _currentItemCount;
        maxQuantityText.text = _currentItemCount.ToString();
        Update_Selected_Amount(_handlerPosition);     // 打开时设置选择数量为 记录的选择数量（初始为 1）
        Use_Amount_Slider.wholeNumbers = true; // 启用整数模式
        
        // 设置 Slider 和 Button 监听事件
        Use_Amount_Slider.onValueChanged.AddListener(Update_Selected_Amount);
        Use_Button.onClick.RemoveAllListeners();
        Use_Button.onClick.AddListener(On_Use_Button_Click);
        
    }


    void Update_Selected_Amount(float value)      // Slider 拖拽时调用
    {
        _selectedQuantity = Mathf.RoundToInt(value);
        UpdateCurrentQuantityDisplay();
    }
    
    // TMP Input Field              // 数量输入框逻辑


    private void UpdateCurrentQuantityDisplay()         // 更新当前选择数量显示
    {
        selectedQuantityText.text = _selectedQuantity.ToString();   // Text 更新数量显示
        _handlerPosition = _selectedQuantity;       // handler 位置更新
    }
    
    
    // Update Effect Summary Text                // 更新使用总效果
    // foreach Use_Func in item_module_use
    
    


    private void On_Use_Button_Click()              // 点击使用按钮时 逻辑
    {
        int times = _selectedQuantity;     // 先获取次数
        
        Debug.Log("点击了，并使用了");
        // 
        for (int i = 0; i < times; i++)
        {
            Debug.Log($"正在使用第{i+1}次");
            item_module_use.UseItems(PanelManager.PM.Current_Open_Item);    // 使用 Item x 次数
        }
    }
    
    
    
    //------------------------------------------------------------------------------------------------------------------
    
    
    
    
    
    
    
    
    
}
