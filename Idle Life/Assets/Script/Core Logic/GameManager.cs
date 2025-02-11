using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    ///////////////////////    玩家各项参数状态 Player Stats - 保存主体 Save Section
    
    
    public float Player_Money;      // 玩家拥有的金钱数 

    public float Player_Stamina { get; private set; } // 玩家体力值
    public float Player_Stamina_Max { get; private set; } // 玩家体力值上限
    public float Player_Stamina_Restore_Rate = 1f;
    
    public Dictionary<string, Ability> Player_Ability;    // 玩家的能力值
    public Dictionary<string, int> Player_Inventory;     // 玩家拥有的 Item (Inventory)
    



    ////////////////////////     游戏各项执行系统，交互逻辑  Game Interaction and Implementation system

    public static GameManager GM;       // 唯一管理员
    
    public static ActivityManager Game_ActivityManager;        // 唯一活动管理器

    
    
    ////////////////////////     所有配置、路径参数

    public const string PATH_SCRIPTABLE_OBJECTS_ABILITY = "Scriptable_Objects/AbilityInstance";
    
    
    
    ////////////////////////     Awake, Start, Update
    
    private void Awake()
    {
        GM = this;
        LoadAllAbilities();
    }

    private void Start()
    {
        Change_Player_Stamina_Max(1000);
        Change_Player_Stamina(700);
    }

    
    
    
    ////////////////////////     玩家状态参数变更方法
    
    
    public void Change_Player_Stamina(float stamina_change_amount)
    {
        Player_Stamina += stamina_change_amount;
    }
    
    public void Change_Player_Stamina_Max(float stamina_max_change_amount)
    {
        Player_Stamina_Max += stamina_max_change_amount;

        if (Player_Stamina > Player_Stamina_Max)        // 若此时玩家体力值大于上限，则将体力值设置为上限值
            Player_Stamina = Player_Stamina_Max;
    }

    public void Change_Player_Money(float money_change_amount)
    {
        Player_Money += money_change_amount;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    

    ////////////////////////     游戏数据初始化 - 加载 Scriptable Object 和 JSON
    
    
    /// TODO 保存和加载功能
    

    private void LoadAllAbilities()     // 加载所有的 Ability Scriptable Objects
    {
        Player_Ability = new Dictionary<string, Ability>();
        
        // 加载所有Ability配置
        var abilityDataArray = Resources.LoadAll<Ability_Data_Scriptable>(PATH_SCRIPTABLE_OBJECTS_ABILITY);
        
        foreach (var ability_instance in abilityDataArray)
        {
            if (Player_Ability.ContainsKey(ability_instance.Ability_Id))
            {
                Debug.LogError($"重复的能力ID: {ability_instance.Ability_Id}");
                continue;
            }

            var ability = new Ability
            {
                Ability_Id = ability_instance.Ability_Id,
                Ability_Label = ability_instance.Ability_Label,
                Ability_Level = 0,
                Ability_Current_Exp = 0,
                ExpToNextLevel = new Dictionary<int, float>() 
                // 若有存档则从存档中加载数据
            };

            foreach (var levelUpExp in ability_instance.LevelUp_Exp_Requirements)
            {
                ability.ExpToNextLevel.Add(levelUpExp.Level, levelUpExp.ExpRequirement);
            }
            
            Player_Ability.Add(ability_instance.Ability_Id, ability);
        }
        
        Debug.Log($"已加载{Player_Ability.Count}项能力");
    }

    
    
    
    
    
    

    
    
    
    
    
}
