using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity_Sport : Activity
{
    [Header("Sport 专属数据")]
    public float Add_Stamina_Max_Amount;    // 体力上限增加值

    public override void Activity_Outcome_Tick()      // 运动会额外增加 体力上限
    {
        base.Activity_Outcome_Tick();
        
        GameManager.GM.Change_Player_Stamina_Max(Add_Stamina_Max_Amount);   // 利用 Game Manager 中的方法改变 体力上限
    }
}
