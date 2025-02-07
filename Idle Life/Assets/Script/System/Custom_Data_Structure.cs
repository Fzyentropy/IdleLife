using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Ability_Exp  ////// 对各种 Activity, 各Activity 所产出的能力经验值
{
    public string AbilityId;
    public float Exp;
}

[System.Serializable]
public class Ability_Level  /////// 对各种 Activity，各Activity 解锁所需的能力等级要求 
{
    public string AbilityId;
    public int LevelRequirement;
}

[System.Serializable]
public class Item_Amount  ////// 对各种 Activity 和 Ability，各项活动 所需/所产出 的Item和数量
{
    public string ItemId;
    public int ItemAmount;
}

[System.Serializable]
public class Ability_LevelUp_Exp_Requirement   /////// 对 Ability，Ability 升级所需经验值表
{
    public int Level;
    public float ExpRequirement;
}