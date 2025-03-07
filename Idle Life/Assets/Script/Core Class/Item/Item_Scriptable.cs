using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Idle Life/Item/New Item")]
public class Item_Scriptable : ScriptableObject
{
    public string Item_Id;
    public string Item_Label;
    public Sprite Item_Sprite;
    public string Item_Description;
    public string Item_Type;      // 此 Item Type 为主观、从游戏设计视角的软分类，与下方 ItemType 枚举不同

    [Space(5)]
    [Header("物品模块")]
    [SerializeReference]
    public List<ItemModule> item_modules = new List<ItemModule>();    // Item 组件集合，用于定义一个 Item 拥有的功能模块
}

