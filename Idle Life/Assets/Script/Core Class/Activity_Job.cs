using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity_Job : Activity
{
    public float Job_Salary;

    public override void Activity_Outcome_Tick()
    {
        base.Activity_Outcome_Tick();
        
        GameManager.GM.Change_Player_Money(Job_Salary);
    }
}
