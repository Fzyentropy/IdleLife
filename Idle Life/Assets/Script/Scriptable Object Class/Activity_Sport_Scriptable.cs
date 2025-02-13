using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// 训练数据存储 (ScriptableObject)
[CreateAssetMenu(fileName = "Sport_", menuName = "Idle Life/New Activity - Sport")]
public class Activity_Sport_Scriptable : Activity_Scriptable
{
    [Header("Sport 专属数据")]
    public float Add_Stamina_Max_Amount;     // 产出金钱
}
