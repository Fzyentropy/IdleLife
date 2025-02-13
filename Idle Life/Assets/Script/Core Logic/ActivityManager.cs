using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;



public class ActivityManager : MonoBehaviour
{
    
    //////  Admin 数据总管

    public static ActivityManager AM;
    
    public List<Activity> All_Activities;   // 全部的 Activities, 在游戏启动时全部加载于此

    [CanBeNull] private Activity Current_Activity;  // 当前 Activity 容器

    [CanBeNull] public Activity current_activity    // 当前活动属性 连接和更改容器存储的 Activity
    {
        get => Current_Activity;
        private set 
        {
            if (Current_Activity != null) { StopCurrentActivity(); }   // 判定并替换旧活动
            Current_Activity = value;
        }
    }
    
    
    //////  活动进度相关参数

    public float tick_interval;     // 当前活动单次 Tick 总时长
    public float current_interval;  // 当次 Tick 所经过的时长
    public float tick_speed = 1f;   // Tick的速度
    
    ////// 所有配置、路径参数
    
    public const string PATH_SCRIPTABLE_OBJECTS_ACTIVITY = "Scriptable_Objects/ActivityInstance";
    
    
    //TODO 判定活动是否解锁并排列在界面
    
    //TODO 判定按钮是否可以按下（若Item不够则无法按下并弹出警告）
    
    
    
    public void _Start_Activity (Activity new_activity)  // 按下开始按钮时调用，开始活动方法
    {
        current_activity = null;
        current_activity = new_activity;    // 赋值时会调用 上方 private set，判定并替换当前 Activity
        StartCoroutine(ActivityLoopCoroutine());
    }

    
    // 专用协程处理活动循环
    private IEnumerator ActivityLoopCoroutine()
    {
        
        var act = current_activity;     // 赋值当前 Activity
        tick_interval = act.Activity_Duration;   // 设置此协程的单次 Tick 时长
        current_interval = 0;     // 累计当前进度
        float elapsed_time = 0.01f;     // 单次计算单位，也表示精度
        

        while (act == current_activity && act.Can_Start_Activity())   // Tick 循环 Loop
        {
            if (current_interval < tick_interval)
            {
                yield return new WaitForSeconds(elapsed_time);
                current_interval += elapsed_time * tick_speed;                   ///////////////  此处可进行时间控制
            }
            else
            {
                current_interval = 0;
                
                // act.OnTick?.Invoke();
                act.Activity_Outcome_Tick();    // 一次 Tick 执行， TODO 替换成 事件响应
            }
            
            // 这里有可能会因为运行时间差而产生bug（资源通过Outcome_Tick更新前就进入了下一个循环），先记下
        }
        
        current_interval = 0;
        yield break;
    }

    // 停止活动方法
    public void StopCurrentActivity()
    {
        current_interval = 0;
        Current_Activity = null;
    }

    

    private void Awake()
    {
        AM = this;
        LoadAllActivities();
    }
    

    ////// 从文件夹中加载 Scriptable Object 的具体方法

    private void LoadAllActivities()
    {
        All_Activities = new List<Activity>();
        
        // 加载所有Activity配置
        var activityDataArray = Resources.LoadAll<Activity_Scriptable>(PATH_SCRIPTABLE_OBJECTS_ACTIVITY);
        
        foreach (var activity_instance in activityDataArray)
        {
            var activity = Create_Activity_From_Scriptable(activity_instance);
            if (activity != null)
            {
                All_Activities.Add(activity);
            }
        }
        
        Debug.Log($"已加载{All_Activities.Count}项活动");
    }

    
    private Activity Create_Activity_From_Scriptable(Activity_Scriptable activity_instance)
    {
        if (activity_instance is Activity_Study_Scriptable activity_study_instance)    // 若是 Study 
        {
            try
            {
                var activity = new Activity_Study()
                {
                    Activity_Id = activity_study_instance.Activity_Id,
                    Activity_Label = activity_study_instance.Activity_Label,
                    Activity_Type = activity_study_instance.Activity_Type,
                    Activity_Duration = activity_study_instance.Activity_Duration,
                    Required_Stamina = activity_study_instance.Required_Stamina,

                    Unlock_Ability_Requirement =
                        ConvertToAbilityRequirementDictionary(activity_study_instance.Unlock_Ability_Requirement),
                    Activity_Item_Requirements = ConvertToItemDictionary(activity_study_instance.Activity_Item_Requirements),
                    Activity_Outcome_Exp = ConvertToExpDictionary(activity_study_instance.Activity_Outcome_Exp),
                    Activity_Outcome_Item = ConvertToItemDictionary(activity_study_instance.Activity_Outcome_Item)
                };
                return activity;
            }
            catch (Exception e)
            {
                Debug.LogError($"加载活动{activity_instance.Activity_Id}失败: {e.Message}");
                return null;
            }
        }
        
        if (activity_instance is Activity_Job_Scriptable activity_job_instance)    // 若是 Study 
        {
            try
            {
                var activity = new Activity_Job()
                {
                    Activity_Id = activity_job_instance.Activity_Id,
                    Activity_Label = activity_job_instance.Activity_Label,
                    Activity_Type = activity_job_instance.Activity_Type,
                    Activity_Duration = activity_job_instance.Activity_Duration,
                    Required_Stamina = activity_job_instance.Required_Stamina,
                    Job_Salary = activity_job_instance.Outcome_Money,

                    Unlock_Ability_Requirement =
                        ConvertToAbilityRequirementDictionary(activity_job_instance.Unlock_Ability_Requirement),
                    Activity_Item_Requirements = ConvertToItemDictionary(activity_job_instance.Activity_Item_Requirements),
                    Activity_Outcome_Exp = ConvertToExpDictionary(activity_job_instance.Activity_Outcome_Exp),
                    Activity_Outcome_Item = ConvertToItemDictionary(activity_job_instance.Activity_Outcome_Item)
                };
                return activity;
            }
            catch (Exception e)
            {
                Debug.LogError($"加载活动{activity_instance.Activity_Id}失败: {e.Message}");
                return null;
            }
        }
        
        if (activity_instance is Activity_Sport_Scriptable activity_sport_instance)    // 若是 Sport 
        {
            try
            {
                var activity = new Activity_Sport()
                {
                    Activity_Id = activity_sport_instance.Activity_Id,
                    Activity_Label = activity_sport_instance.Activity_Label,
                    Activity_Type = activity_sport_instance.Activity_Type,
                    Activity_Duration = activity_sport_instance.Activity_Duration,
                    Required_Stamina = activity_sport_instance.Required_Stamina,
                    Add_Stamina_Max_Amount = activity_sport_instance.Add_Stamina_Max_Amount,

                    Unlock_Ability_Requirement =
                        ConvertToAbilityRequirementDictionary(activity_sport_instance.Unlock_Ability_Requirement),
                    Activity_Item_Requirements = ConvertToItemDictionary(activity_sport_instance.Activity_Item_Requirements),
                    Activity_Outcome_Exp = ConvertToExpDictionary(activity_sport_instance.Activity_Outcome_Exp),
                    Activity_Outcome_Item = ConvertToItemDictionary(activity_sport_instance.Activity_Outcome_Item)
                };
                return activity;
            }
            catch (Exception e)
            {
                Debug.LogError($"加载活动{activity_instance.Activity_Id}失败: {e.Message}");
                return null;
            }
        }
        
        // 其他类型的活动加载
        // 在此处扩展
        /*if (activityInstance.Activity_Type == "其他类型")
        {
            try
            {
                var activity = new Activity_Study()
                {
                    Activity_Id = activityInstance.Activity_Id,
                    Activity_Label = activityInstance.Activity_Label,
                    Activity_Type = activityInstance.Activity_Type,
                    Activity_Duration = activityInstance.Activity_Duration,
                    Required_Stamina = activityInstance.Required_Stamina,

                    Unlock_Ability_Requirement =
                        ConvertToAbilityRequirementDictionary(activityInstance.Unlock_Ability_Requirement),
                    Activity_Requirements = ConvertToItemDictionary(activityInstance.Item_Requirements),
                    Activity_Outcome_Exp = ConvertToExpDictionary(activityInstance.Activity_Outcome_Exp),
                    Activity_Outcome_Item = ConvertToItemDictionary(activityInstance.Activity_Outcome_Item)
                };
                return activity;
            }
            catch (Exception e)
            {
                Debug.LogError($"加载活动{activityInstance.Activity_Id}失败: {e.Message}");
                return null;
            }
        }*/

        return null;
    }

    // 辅助转换方法
    private Dictionary<string, int> ConvertToItemDictionary(List<Item_Amount> list)
    {
        return list.ToDictionary(item => item.ItemId, item => item.ItemAmount);
    }
    
    private Dictionary<string, float> ConvertToExpDictionary(List<Ability_Exp> list)
    {
        return list.ToDictionary(exp => exp.AbilityId, exp => exp.Exp);
    }

    private Dictionary<string, int> ConvertToAbilityRequirementDictionary(List<Ability_Level> list)
    {
        return list.ToDictionary(ability => ability.AbilityId, ability => ability.LevelRequirement);
    }
    
    
    
    
    
    
    
    
    
}


