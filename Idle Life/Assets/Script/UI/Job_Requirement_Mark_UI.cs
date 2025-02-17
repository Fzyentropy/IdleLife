using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UniRx;

public class Job_Requirement_Mark_UI : MonoBehaviour
{
    public Activity_Job_Scriptable job_scriptable;
    private Activity job;
    private int ability_level;
    public Ability_Types ability_type;
    public TMP_Text requirement_text;


    private void Start()
    {
        Initialize_Job();
        Start_Monitor_Ability_Requirement();
    }

    private void Update()
    {
        Update_Ability_Level();
    }


    void Initialize_Job()
    {
        job = ActivityManager.AM.All_Activities
            .FirstOrDefault(a => a.Activity_Id == job_scriptable.Activity_Id);
    }

    void Update_Ability_Level()     // 实时更新当前绑定的能力等级
    {
        ability_level = GameManager.GM.Player_Ability["Ability_" + ability_type].Ability_Level;
    }

    void Start_Monitor_Ability_Requirement()
    {
        this.ObserveEveryValueChanged(mark => mark.ability_level)
            .Subscribe(abilityLevel =>
            {
                if (abilityLevel >= job.Unlock_Ability_Requirement["Ability_" + ability_type])
                {
                    requirement_text.color = Color.green;
                }
                else
                {
                    requirement_text.color = Color.red;
                }
            }).AddTo(this);
    }
    
    
}

