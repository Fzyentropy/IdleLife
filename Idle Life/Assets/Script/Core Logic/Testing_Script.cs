using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing_Script : MonoBehaviour
{

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
}
