using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Testing_Script_Panel : MonoBehaviour
{

    [Header("Inventory测试")] 
    public Button add_item_button;
    public Button remove_item_button;

    

    public void Add_Townhouse()
    {
        Inventory.IVT.Add_Item_To_Inventory("Item_Townhouse", 1);
        Debug.Log("TESTING: Townhouse Added");
    }

    public void Remove_Townhouse()
    {
        Inventory.IVT.Remove_Item_From_Inventory("Item_Townhouse",1);
        Debug.Log("TESTING: Townhouse Removed");
    }
    
    public void Add_Food()
    {
        Inventory.IVT.Add_Item_To_Inventory("Food", 1);
        Debug.Log("TESTING: Food Added");
    }
    
    public void Remove_Food()
    {
        Inventory.IVT.Remove_Item_From_Inventory("Food",1);
        Debug.Log("TESTING: Food Removed");
    }
    

}
