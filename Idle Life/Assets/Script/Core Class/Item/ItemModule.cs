using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ItemModule
{
    
}


[Serializable]
public class ItemModule_ShopItem : ItemModule       // 商店物品
{
    public int Item_Price;     // 购买价格
    public int Item_Total_Amount;
}

[Serializable]
public class ItemModule_Upgrade : ItemModule        // 升级模块
{
    public float expRequired;
    public string Upgrade_To;
}

[Serializable]
public class ItemModule_Equipment : ItemModule        // 升级模块
{
    public string Equipment_Type;
}

[Serializable]
public class ItemModule_Use : ItemModule        // 使用模块
{
    [SerializeField]
    public List<ItemModule_UseFunc> use_funcs;

    public void UseItems(Item item)
    {
        if (use_funcs == null) return;

        foreach (var use_func in use_funcs)
        {
            use_func.Use_Item(item);
        }
    }
}