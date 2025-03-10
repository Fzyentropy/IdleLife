using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //------------------------------------------------------------------------------------------------------------------
    // 玩家各项参数状态 Player Stats - 保存主体 Save Section
    [Space(5)][Header("玩家状态 Player Stats")]
    
    //// 金钱
    [Sirenix.OdinInspector.ReadOnly] public float Player_Money;      // 玩家拥有的金钱数 

    //// 体力值
    [Sirenix.OdinInspector.ReadOnly] public float Player_Stamina { get; private set; }           // 玩家体力值
    [Sirenix.OdinInspector.ReadOnly] public float Player_Stamina_Max { get; private set; }       // 玩家体力值上限
    [Sirenix.OdinInspector.ReadOnly] public float Player_Stamina_Base_Restore_Rate = .5f;         // 体力恢复速率 /每秒 (基础值)
    [Sirenix.OdinInspector.ReadOnly] public bool can_restore_stamina = true;                     // 是否可以回复体力值 (体力回复锁)
    
    //// 饥饿值 (饱腹值) (-1 - 2)
    [Sirenix.OdinInspector.ReadOnly] public float Player_Satiety;         // 玩家饱腹值
    [Sirenix.OdinInspector.ReadOnly] public float Player_Satiety_Min = -1f;     // 玩家饱腹值下限
    [Sirenix.OdinInspector.ReadOnly] public float Player_Satiety_Max = 2f;     // 玩家饱腹值上限
    [Sirenix.OdinInspector.ReadOnly] public float Player_Satiety_Reduce_Rate = .005f;         // 玩家饱腹值减少速率
    
    // 玩家的各能力等级 (含当前等级和经验值)
    public Dictionary<string, Ability> Player_Ability;    
    
    
    //------------------------------------------------------------------------------------------------------------------
    // 玩家状态 Player Stats 临时调试参数
    [Space(5)][Header("临时设置 - 玩家开局状态")]

    public float temp_player_money;         // 临时设置，玩家开局金钱
    public float temp_player_stamina;       // 临时设置，玩家开局体力值
    public float temp_player_stamina_max;   // 临时设置，玩家开局体力上限
    public float temp_player_satiety;       // 临时设置，玩家开局饱腹值
    
    
    //------------------------------------------------------------------------------------------------------------------
    // 游戏各项执行系统，交互逻辑  Game Interaction and Implementation system

    public static GameManager GM;       // 唯一管理员



    ////////////////////////     所有配置、路径参数

    public const string PATH_SCRIPTABLE_OBJECTS_ABILITY = "Scriptable_Objects/AbilityInstance";
    
    
    
    //------------------------------------------------------------------------------------------------------------------
    // Awake, Start, Update
    
    private void Awake()
    {
        GM = this;
        LoadAllAbilities();
    }

    
    private void Start()
    {
        Set_Player_Stats_Temp();    // 临时，调试用，设置玩家开局属性 - 实际调用
    }

    private void Set_Player_Stats_Temp()    // 临时，调试用，设置玩家开局属性 - 具体方法
    {
        Change_Player_Money(temp_player_money);              // 临时设置金钱
        Change_Player_Stamina_Max(temp_player_stamina_max);  // 临时设置体力上限
        Change_Player_Stamina(temp_player_stamina);          // 临时设置体力值
        Change_Player_Satiety(temp_player_satiety);          // 临时设置饱腹值
    }



    //------------------------------------------------------------------------------------------------------------------
    // 玩家状态参数变更方法
    
    
    public void Change_Player_Stamina(float stamina_change_amount)      // 变更玩家体力值（可输入负值）
    {
        Player_Stamina += stamina_change_amount;
        
        //若此时玩家体力值大于上限，则将体力值设置为上限值
        if (Player_Stamina > Player_Stamina_Max) 
            Player_Stamina = Player_Stamina_Max;

        if (Player_Stamina < 0)
            Player_Stamina = 0;
    }
    
    public void Change_Player_Stamina_Max(float stamina_max_change_amount)      // 变更玩家体力上限
    {
        Player_Stamina_Max += stamina_max_change_amount;
        
        //若此时玩家体力值大于上限，则将体力值设置为上限值
        if (Player_Stamina > Player_Stamina_Max) 
            Player_Stamina = Player_Stamina_Max;
    }

    public void Change_Player_Money(float money_change_amount)          // 变更玩家金钱（可输入负值）
    {
        Player_Money += money_change_amount;
    }

    public void Change_Player_Satiety(float satiety_change_amount)      // 变更玩家饱腹值（可输入负值）
    {
        Player_Satiety += satiety_change_amount;

        if (Player_Satiety > Player_Satiety_Max)
            Player_Satiety = Player_Satiety_Max;

        if (Player_Satiety < Player_Satiety_Min)
            Player_Satiety = Player_Satiety_Min;
    }
    
    
    //------------------------------------------------------------------------------------------------------------------
    // 游戏数据初始化 - 加载 Scriptable Object 和 JSON
    
    
    /// TODO 保存和加载功能
    

    private void LoadAllAbilities()     // 加载所有的 Ability Scriptable Objects
    {
        Player_Ability = new Dictionary<string, Ability>();
        
        // 加载所有Ability配置
        var abilityDataArray = Resources.LoadAll<Ability_Scriptable>(PATH_SCRIPTABLE_OBJECTS_ABILITY);
        
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

    
    //------------------------------------------------------------------------------------------------------------------
    
    
    
    

    
    
    
    
    
}
