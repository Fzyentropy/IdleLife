using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideLogicManager : MonoBehaviour
{
    
    // 体力值相关
    public bool can_restore_stamina = true;
    
    private void Start()
    {
        StartCoroutine(Auto_Restore_Stamina());
    }






    private IEnumerator Auto_Restore_Stamina()
    {
        float _elapsed = 0.01f;

        while (true)
        {
            yield return new WaitUntil(() => can_restore_stamina);
            yield return new WaitForSeconds(_elapsed);
            GameManager.GM.Change_Player_Stamina(GameManager.GM.Player_Stamina_Restore_Rate * _elapsed);
        }
    }
    
    
    
    
    
    
    
    
    
}
