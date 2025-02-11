using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 抽象父类 Activity

public abstract class Activity
{
    [Header("基础信息")]
    public string Activity_Id;        // 活动名称 
    public string Activity_Label;
    public string Activity_Type;
    
    [Header("活动数据")]
    public float Activity_Duration;     // 单次活动持续时间 x秒
    public float Required_Stamina;      // 体力值消耗 /秒

    public Dictionary<string, int> Unlock_Ability_Requirement;  // 解锁活动所需的能力值
    public Dictionary<string, int> Activity_Requirements;   // 活动所需 Item

    public Dictionary<string, float> Activity_Outcome_Exp;   // 活动单位时间产出 - 经验值
    public Dictionary<string, int> Activity_Outcome_Item;    // 活动单位时间产出 - 物品

    
    public bool Meet_Unlock_Requirements()  // 解锁判定，每个活动不同
    {
        if (Unlock_Ability_Requirement.Count <= 0)
            return true;
        
        foreach (var ability in Unlock_Ability_Requirement)
        {
            if (GameManager.GM.Player_Ability[ability.Key].Ability_Level < ability.Value)  // 判定玩家是否满足所需的 每项能力值的等级
                return false;
        }
        
        return true;
    }
    

    public virtual bool Can_Start_Activity()  // 是否可以开始活动，即所需Item是否足够的判定，子类可加入自适应判定
    {

        if (GameManager.GM.Player_Stamina < Required_Stamina)
            return false;
        
        if (Activity_Requirements.Count == 0)
            return true;

        foreach (var item_requirement in Activity_Requirements)
        {
            if (GameManager.GM.Player_Inventory[item_requirement.Key] < item_requirement.Value)  // 判定玩家是否满足所需的 每项 Item 的数量
                return false;
        }

        return true;
    }

    
    public virtual void Activity_Outcome_Tick()   //// 一次活动产出结算
    {
        
        // TODO 加入判定：首先判断这次 Tick 是否成功，若仓库已满，或者其他原因，而不成功 ———— 则不会消耗资源也不会产出资源，且活动停止
        // 有可能另起一个方法判定

        
        GameManager.GM.Change_Player_Stamina(-Required_Stamina);        // 消耗体力值
        

        if (Activity_Requirements.Count > 0)    // 结算 Item 消耗
        {
            foreach (var item_requirement in Activity_Requirements)
            {
                if (GameManager.GM.Player_Inventory[item_requirement.Key] == item_requirement.Value)    // 若正好拥有需要消耗的数量，则直接从仓库中移除该资源
                    GameManager.GM.Player_Inventory.Remove(item_requirement.Key);
                else
                    GameManager.GM.Player_Inventory[item_requirement.Key] -= item_requirement.Value;    // 若拥有的数量超过需要消耗的，则从数量中减去

            }
        }
        
        if (Activity_Outcome_Exp.Count > 0)     // 结算 能力经验值
        {
            foreach (var ability_exp in Activity_Outcome_Exp)
            {
                GameManager.GM.Player_Ability[ability_exp.Key].AddExp(ability_exp.Value);
            }
        }

        if (Activity_Outcome_Item.Count > 0)      // 结算 Item 产出
        {
            foreach (var item in Activity_Outcome_Item)
            {
                if (GameManager.GM.Player_Inventory.ContainsKey(item.Key)) // 若已经拥有该物品，则增加数量
                    GameManager.GM.Player_Inventory[item.Key] ++;
                else
                    GameManager.GM.Player_Inventory.Add(item.Key,item.Value);  // 若没有该物品，则增加数量
            }
        }
        
    }
    
    
}


/*public class Activity_Study : Activity
{
    
}*/
