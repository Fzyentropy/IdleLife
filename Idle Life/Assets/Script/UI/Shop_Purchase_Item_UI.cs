using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

// 用于挂载在一个商店中可出售物品的 prefab 上，载有购买逻辑
public class Shop_Purchase_Item_UI : MonoBehaviour
{
    [Header("Item Scriptable Object")]
    public Item_Scriptable_ShopItem Item_Instance;

    [Header("UI")]
    public TMP_Text itemName;
    public Image itemImage;
    public Button itemPurchaseButton;


    private void Start()
    {
        Initialize_Item();
        Set_Purchase_Button();
    }


    private void Initialize_Item()
    {
        if (Item_Instance != null)
        {
            itemName.text = Item_Instance.Item_Label;       // 加载 Item 名称 Label
            // itemImage = Item_Instance.image            // 加载 Item 图片，待补充
        }
        else
        {
            Debug.LogError("Item 实例不存在！");
        }
        
    }


    private void Set_Purchase_Button()
    {
        //// 按钮状态绑定
        Observable.CombineLatest(
                GameManager.GM.ObserveEveryValueChanged(gm => gm.Player_Money),
                Item_Instance.ObserveEveryValueChanged(item => item.Item_Purchase_Price),
                (player_money, item_price) => player_money >= item_price
            ).Subscribe(canPurchase => itemPurchaseButton.interactable = canPurchase)
            .AddTo(this);
        
        //// 点击事件
        itemPurchaseButton.OnClickAsObservable()
            .Subscribe(_ =>
                {
                    GameManager.GM.Change_Player_Money(-Item_Instance.Item_Purchase_Price);
                    Inventory.IVT.Add_Item_To_Inventory(Item_Instance.Item_Id, 1);
                }
            ).AddTo(this);
    }
    
    
    
    
    
}
