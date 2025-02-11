using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// 训练数据存储 (ScriptableObject)
[CreateAssetMenu(fileName = "Job_", menuName = "Idle Life/New Activity-Job")]
public class Activity_Job_Scriptable : Activity_Scriptable
{
    [Header("Job 专属数据")]
    public float Outcome_Money;     // 产出金钱
}
