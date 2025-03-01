using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing_Script_Global : MonoBehaviour
{
    
    private void Start()
    {
        Put_Item_To_Inventory_At_Start();
    }

    void Update()
    {
        SpeedUpActivity();
    }


    public void SpeedUpActivity()
    {
        if (Input.GetKey(KeyCode.F))
        {
            ActivityManager.AM.tick_speed = 10f;
            GameManager.GM.Player_Stamina_Restore_Rate = 15f;
        }
        else
        {
            ActivityManager.AM.tick_speed = 1f;
            GameManager.GM.Player_Stamina_Restore_Rate = .5f;
        }
    }


    public void Put_Item_To_Inventory_At_Start()
    {
        Inventory.IVT.Player_Items.Add("Item_Townhouse", 2);
        Inventory.IVT.Player_Items.Add("Item_Food", 3);
        Debug.Log("Item 已添加");
    }
    
}
