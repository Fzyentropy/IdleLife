using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// using UnityEngine.Events;

// 核心数据容器
public class Ability
{
    public string Ability_Id { get; set; }
    
    public string Ability_Label { get; set; }
    public int Ability_Level { get; set; }
    public float Ability_Current_Exp { get;  set; }
    
    public Dictionary<int,float> ExpToNextLevel;
    
    public UnityEvent<Ability> OnLevelUp;
    
    public void AddExp(float amount)
    {
        Ability_Current_Exp += amount;
        while (Ability_Current_Exp >= ExpToNextLevel[Ability_Level + 1])
        {
            LevelUp();
        }
    }
    
    private void LevelUp()
    {
        Ability_Current_Exp -= ExpToNextLevel[Ability_Level + 1];
        Ability_Level++;
        OnLevelUp?.Invoke(this);
    }
}

