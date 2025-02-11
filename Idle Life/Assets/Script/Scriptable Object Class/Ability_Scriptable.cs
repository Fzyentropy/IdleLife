using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Ability_", menuName = "Idle Life/New Ability")]
public class Ability_Scriptable : ScriptableObject
{
    [Header("基础信息")]
    public string Ability_Id; // 唯一标识
    public string Ability_Label; // 显示名称

    [Header("升级设置")] 
    public List<Ability_LevelUp_Exp_Requirement> LevelUp_Exp_Requirements;

    // [Header("图标与UI")]
    // public Sprite Icon; // 能力图标
    // public Color ThemeColor = Color.white; // 主题色
}