using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Idle Life/New Item - Shop Purchase")]
public class Item_ShopPurchase_Scriptable : Item_Scriptable
{
    [Header("商店购买物品 特殊属性")]
    public float Item_Purchase_Price;     // 购买物品的价格
}
