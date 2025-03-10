using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

// 用于挂载在一个商店中可出售物品的 prefab 上，载有购买逻辑
public class Shop_Item_UI : MonoBehaviour
{
    [Sirenix.OdinInspector.ReadOnly]
    public Item Shop_Item_Instance;
    private int item_price;     // 临时存储 物品价格

    [Header("UI")]
    public TMP_Text itemName;
    public Image itemImage;
    public TMP_Text itemDescription;
    public TMP_Text itemPrice;
    public Button itemPurchaseButton;


    private void Start()
    {
        Check_Setup();
        Initialize_Item();
        Set_Purchase_Button();
    }


    private void Check_Setup()
    {
        if (itemName == null
            || itemImage == null
            || itemPurchaseButton == null
            || itemDescription == null
            ) {Debug.LogError("Shop_Item_UI: UI组件未绑定完全");}
    }

    private void Initialize_Item()
    {
        if (Shop_Item_Instance != null)
        {
            itemName.text = Shop_Item_Instance.Item_Label;       // 加载 Item 名称 Label
            itemImage.sprite = Shop_Item_Instance.Item_Image;    // 加载 Item 图片
            itemDescription.text = Shop_Item_Instance.Item_Modules.OfType<ItemModule_ShopItem>().FirstOrDefault().Shop_Item_Description;
            
            item_price = Shop_Item_Instance.Item_Modules.OfType<ItemModule_ShopItem>().FirstOrDefault().Item_Price;     // 扒取价格
            itemPrice.text = $"${item_price}";
            
        }
        else
        {
            Debug.LogError("Shop_Item_UI: Item 实例不存在！");
        }
        
    }


    private void Set_Purchase_Button()
    {
        //// 按钮状态绑定
        
        /// TODO 加入其他判定能否购买的条件
        
        GameManager.GM.ObserveEveryValueChanged(gm => gm.Player_Money)
        .Subscribe(player_money =>
            {
                itemPurchaseButton.interactable = player_money >= item_price;
            }
            ).AddTo(this);
        
        //// 点击事件
        itemPurchaseButton.OnClickAsObservable()
            .Subscribe(_ =>
                {
                    GameManager.GM.Change_Player_Money(-item_price);
                    Inventory.IVT.Add_Item_To_Inventory(Shop_Item_Instance.Item_Id, 1);
                }
            ).AddTo(this);
    }
    
    
    
    
    
}
