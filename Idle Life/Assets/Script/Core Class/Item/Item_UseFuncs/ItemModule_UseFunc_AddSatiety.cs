using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UseFunc_Add_Satiety", menuName = "Idle Life/Item UseFunc/UseFunc - Add Satiety")]      // Item使用功能：增加体力值
public class ItemModule_UseFunc_AddSatiety : ItemModule_UseFunc
{
    public float Add_Satiety_Amount;
    
    public override void Use_Item(Item item)
    {
        GameManager.GM.Player_Satiety += Add_Satiety_Amount;
        Debug.Log($"使用 {item.Item_Label} 恢复了 {Add_Satiety_Amount} 体力值");
    }
}
