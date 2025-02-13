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
    public Item_Scriptable Inventory_Item_Scriptable;
    
    [Header("UI")]
    public Image item_image;
    public TMP_Text item_amount;


    private void Update()
    {
        Update_Item_Amount();
    }


    public void Update_Item_Amount()
    {
        if (Inventory.IVT.Player_Items.ContainsKey(Inventory_Item_Scriptable.Item_Id))
        {
            item_amount.text = Inventory.IVT.Player_Items[Inventory_Item_Scriptable.Item_Id].ToString();
            Debug.Log("Id: " + Inventory_Item_Scriptable.Item_Id + "Amount: "+ Inventory.IVT.Player_Items[Inventory_Item_Scriptable.Item_Id]);
        }
        else
        {
            item_amount.text = "0";
        }

    }
    
    
}
