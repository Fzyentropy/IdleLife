using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI : MonoBehaviour
{

    public GameObject inventory_layout;     // 含 Grid Layout Group 的 Item 们的父对象
    public GameObject Item_Prefab;        // 在 Inventory Panel 中显示的一个 Item 的 prefab (含悬停和点击功能)


    
    //------------------------------------------------------------------------------------------------------------------
    
    
    private void Start()
    {
        Check_Setup();
    }

    private void OnEnable()     // 每当面板打开时
    {
        Refresh_Inventory();    // 刷新 Inventory
        Subscribe_To_Inventory_Update();    // 开始监听 Inventory Update 事件
    }

    private void OnDisable()
    {
        UnSubscribe_To_Inventory_Update();
    }
    
    
    //------------------------------------------------------------------------------------------------------------------

    private void Check_Setup()
    {
        if (inventory_layout == null)
            Debug.LogError("未设置 Item layout 父对象");
        
        if (Item_Prefab == null)
            Debug.LogError("未设置 Item Prefab");
    }
    
    
    //------------------------------------------------------------------------------------------------------------------
    // 订阅、取消订阅 Inventory更新事件
    
    private void Subscribe_To_Inventory_Update()
    {
        Inventory.IVT.On_Inventory_Update += Refresh_Inventory;
    }

    private void UnSubscribe_To_Inventory_Update()
    {
        Inventory.IVT.On_Inventory_Update -= Refresh_Inventory;
    }
    
    //------------------------------------------------------------------------------------------------------------------
    
    
    private void Refresh_Inventory()         // 更新 Item UI 显示
    {
        // 清除旧 UI
        foreach (Transform child in inventory_layout.transform)
        {
            Destroy(child.gameObject);
        }
        
        // 重新生成 UI
        // TODO 后续可能引入 Item 分类、分页或者筛选
        foreach (var item in Inventory.IVT.Player_Items)
        {
            GameObject item_instance = Instantiate(Item_Prefab, inventory_layout.transform);      // 生成 Item prefab
            item_instance.name = $"Inventory_{item.Key}";
            
            Inventory_Item_UI inventory_item_ui = item_instance.GetComponent<Inventory_Item_UI>();    // 获取到 Inventory_Item_UI 脚本实例
            inventory_item_ui.Update_Item(Inventory.IVT.Get_Item_By_ID_From_IVT(item.Key), item.Value);     // 设置 prefab 的 Item 实例和 Item 的数量
        }
        
    }
    

    //------------------------------------------------------------------------------------------------------------------
    
    
}
