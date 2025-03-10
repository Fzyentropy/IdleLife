using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_UI : MonoBehaviour
{
    [Header("Shop UI")]
    public GameObject shop_layout;      // 含 Grid Layout Group 的 Shop Item 们的父对象
    public GameObject shopitem_prefab;      // Shop Item 的 prefab
    
    private void Start()
    {
        Check_Setup();
    }

    private void OnEnable()     // 每当面板打开时
    {
        Refresh_Shop();    // 刷新 Shop 界面
    }
    
    
    private void Check_Setup()
    {
        if (shop_layout == null)
            Debug.LogError("未设置 Shop layout 父对象");
        
        if (shopitem_prefab == null)
            Debug.LogError("未设置 Shop Item Prefab");
    }


    private void Refresh_Shop()
    {
        
        // 清除旧 UI
        foreach (Transform child in shop_layout.transform)
        {
            Destroy(child.gameObject);
        }
        
        // 重新生成 UI
        // TODO 后续可能引入 Shop 分类、分页或者筛选
        foreach (var shopItem in Inventory.IVT.Shop_Items)
        {
            GameObject shop_item_instance = Instantiate(shopitem_prefab, shop_layout.transform);      // 生成 Item prefab
            shop_item_instance.name = $"ShopItem_{shopItem.Item_Id}";
            
            Shop_Item_UI shop_item_ui = shop_item_instance.GetComponent<Shop_Item_UI>();    // 获取到 Inventory_Item_UI 脚本实例
            shop_item_ui.Shop_Item_Instance = shopItem;     // 设置 Shop Item prefab 上 UI 脚本 的 Shop Item 实例
        }
        
        
    }
    
    
    
}
