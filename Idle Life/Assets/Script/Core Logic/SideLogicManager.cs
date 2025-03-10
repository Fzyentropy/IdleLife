using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SideLogicManager : MonoBehaviour
{
    
    // 体力值相关
    public float Player_Stamina_Restore_Rate => Calculate_Player_Stamina_Restore_Rate();


    private void Start()
    {
        StartCoroutine(Auto_Restore_Stamina());               // 回复玩家体力
        StartCoroutine(Auto_Reduce_Satiety());              // 减少玩家饱腹值
    }




    private float Calculate_Player_Stamina_Restore_Rate()     // 计算玩家体力值回复速率，一直启用
    {

        return
            GameManager.GM.Player_Stamina_Base_Restore_Rate     // 基础回复速率
            * GameManager.GM.Player_Satiety  // 饱腹值影响：Player_Satiety 饱腹值的平方
            ;
    }

    private IEnumerator Auto_Restore_Stamina()          // 回复玩家体力
    {
        float _elapsed = 0.01f;

        while (true)
        {
            yield return new WaitUntil(() => GameManager.GM.can_restore_stamina);
            yield return new WaitForSeconds(_elapsed);
            
            GameManager.GM.Change_Player_Stamina(Player_Stamina_Restore_Rate * _elapsed);   // 回复玩家体力，*_elapsed 为系统层offset，保证每秒钟内的累计 = Restore Rate /s
        }
    }

    private IEnumerator Auto_Reduce_Satiety()
    {
        float _elapsed = 0.01f;

        while (true)
        {
            yield return new WaitForSeconds(_elapsed);
            
            GameManager.GM.Change_Player_Satiety(-GameManager.GM.Player_Satiety_Reduce_Rate * _elapsed);   // 减少玩家饱腹值，*_elapsed 为系统层offset，保证每秒钟内的累计 = Restore Rate /s
        }
    }
    
    
    
    
    
    
    
    
}
