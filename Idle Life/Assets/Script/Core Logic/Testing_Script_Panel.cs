using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Testing_Script_Panel : MonoBehaviour
{

    [Header("Inventory测试")] 
    public Button add_item_button;
    public Button remove_item_button;

    

    public void Add_Item()
    {
        Inventory.IVT.Add_Item_To_Inventory("Item_Townhouse", 1);
        Debug.Log("TESTING: Item Added");
    }

    public void Remove_Item()
    {
        Inventory.IVT.Remove_Item_From_Inventory("Item_Townhouse",1);
        Debug.Log("TESTING: Item Removed");
    }
    

}
