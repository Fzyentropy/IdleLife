using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Inventory_Item_UI : MonoBehaviour
{
    [Header("Item Scriptable")]
    public Item item_instance;

    [Header("UI")] 
    // public TMP_Text item_label;     // Item 名称，暂时不显示
    public Image item_image;
    public TMP_Text item_amount;


    
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
    
    
    
    
    ////// 点击、悬停 时触发的操作
    
    
    public void OnClick()
    {
        Debug.Log($"你点击了 {item_instance.Item_Id}");
    }

    public void OnHover()
    {
        
    }
    
    
    //------------------------------------------------------------------------------------------------------------------
    
    
}
