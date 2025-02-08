using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using TMPro;
using UnityEngine.UI;

public class LearningPanelUI : MonoBehaviour
{
    [SerializeField] private Button _learnMathBtn;
    [SerializeField] private Slider Study_Math_Progress_Bar;
    
    [SerializeField] private Button _learnPhysicsBtn;
    

    private void Start()
    {
        SetupMathLearning();
        SetupPhysicsLearning();
    }

    private void Update()
    {
        Debug.Log(ActivityManager.AM.tick_interval);
    }

    private void SetupMathLearning()
    {
        var activity = Get_Activity_From_AM("Study_Math");

        //// 按钮状态绑定
        Observable.CombineLatest(
            GameManager.GM.ObserveEveryValueChanged(gm => gm.Player_Stamina),
            activity.ObserveEveryValueChanged(a => a.Meet_Unlock_Requirements()),
            (stamina, unlocked) => stamina >= activity.Required_Stamina && unlocked
        ).Subscribe(canInteract => _learnMathBtn.interactable = canInteract)
         .AddTo(this);
        
        
        //// 动态更新按钮状态
        ActivityManager.AM.ObserveEveryValueChanged(am => am.current_activity)
            .Subscribe(current => {
                bool isCurrent = current == activity;
            
                // 更新按钮文本
                _learnMathBtn.GetComponentInChildren<TMP_Text>().text = isCurrent ? "Stop" : "Start";
            
                // 更新按钮样式（示例：切换颜色）
                // _learnMathBtn.GetComponent<Image>().color = isCurrent ? Color.red : Color.white;
            
                // 如果需要切换Sprite：
                // _learnMathBtn.GetComponent<Image>().sprite = isCurrent ? stopSprite : startSprite;
            })
            .AddTo(this);

        
        //// 点击事件
        _learnMathBtn.OnClickAsObservable()
            .Subscribe(_ =>
                {
                    if (ActivityManager.AM.current_activity != null && activity.Activity_Id == ActivityManager.AM.current_activity.Activity_Id)
                        ActivityManager.AM.StopCurrentActivity();
                    else 
                        StartLearning(activity);
                }
                ).AddTo(this);
        
        
        ////// 活动进度条显示
        ActivityManager.AM.ObserveEveryValueChanged(a => a.current_interval)
            .CombineLatest(
                ActivityManager.AM.ObserveEveryValueChanged(a => a.tick_interval),
                (current, full) => new { current, full })
            .Subscribe(data =>
            {
                Study_Math_Progress_Bar.maxValue = data.full;
                Study_Math_Progress_Bar.value = data.current;
            })
            .AddTo(this);
    }

    private void SetupPhysicsLearning()
    {
        var activity = Get_Activity_From_AM("Study_Physics");

        //// 按钮状态绑定
        Observable.CombineLatest(
                GameManager.GM.ObserveEveryValueChanged(gm => gm.Player_Stamina),
                activity.ObserveEveryValueChanged(a => a.Meet_Unlock_Requirements()),
                (stamina, unlocked) => stamina >= activity.Required_Stamina && unlocked             // 若满足 "体力值足够" 以及 "满足解锁条件（能力值等级达到）" 即可点击按钮
            ).Subscribe(canInteract => _learnPhysicsBtn.interactable = canInteract)
            .AddTo(this);

        
        //// 动态更新按钮状态
        ActivityManager.AM.ObserveEveryValueChanged(am => am.current_activity)
            .Subscribe(current => {
                bool isCurrent = current == activity;
            
                // 更新按钮文本
                _learnPhysicsBtn.GetComponentInChildren<TMP_Text>().text = isCurrent ? "Stop" : "Start";
            
                // 更新按钮样式（示例：切换颜色）
                // _learnMathBtn.GetComponent<Image>().color = isCurrent ? Color.red : Color.white;
            
                // 如果需要切换Sprite：
                // _learnMathBtn.GetComponent<Image>().sprite = isCurrent ? stopSprite : startSprite;
            })
            .AddTo(this);

        
        //// 点击事件
        _learnPhysicsBtn.OnClickAsObservable()
            .Subscribe(_ =>
                {
                    if (ActivityManager.AM.current_activity != null && activity.Activity_Id == ActivityManager.AM.current_activity.Activity_Id)
                        ActivityManager.AM.StopCurrentActivity();
                    else 
                        StartLearning(activity);
                }
            ).AddTo(this);
    }

    private Activity Get_Activity_From_AM(string activityId)      // 从 AM 的 All_Activity List 中获取指定的活动
    {
        return ActivityManager.AM.All_Activities
            .FirstOrDefault(a => a.Activity_Id == activityId);
    }

    private void StartLearning(Activity activity)       // 点击按钮时调用的方法，开始学习 or 停止当前学习
    {
        
        if (activity.Can_Start_Activity())
        {
            ActivityManager.AM._Start_Activity(activity);
        }
    }
}
