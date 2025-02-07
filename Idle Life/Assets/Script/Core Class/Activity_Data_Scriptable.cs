using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// 训练数据存储 (ScriptableObject)
[CreateAssetMenu(fileName = "New_Activity", menuName = "Idle Life/Create New Activity")]
public class Activity_Data_Scriptable : ScriptableObject
{
    public string Activity_Id;    // 名称
    public string Activity_Label;     // 显示名称
    public string Activity_Type;    // 所属活动类型
    public float Activity_Duration; // 单次训练时间
    public float Required_Stamina;    // 所需体力值
    
    public List<Ability_Level> Unlock_Ability_Requirement;  // 解锁训练所需的能力值等级
    public List<Item_Amount> Item_Requirements;   // 训练所需原料物品
    
    public List<Ability_Exp> Activity_Outcome_Exp;    // 训练产出的能力经验值
    public List<Item_Amount> Activity_Outcome_Item;    // 训练产出的物品
}
