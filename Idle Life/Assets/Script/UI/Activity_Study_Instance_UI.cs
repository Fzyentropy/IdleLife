using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using TMPro;
using UnityEngine.UI;

public class Activity_Study_Instance_UI : MonoBehaviour
{

    public Activity_Study_Scriptable Activity_Study_Instance;
    
    [SerializeField] private Button _startStudyButton;
    [SerializeField] private Slider Study_Progress_Bar;


    private void Start()
    {
        Setup_Study();
    }
    

    private void Setup_Study()
    {
        var activity = Get_Activity_From_AM(Activity_Study_Instance.Activity_Id);

        //// 按钮状态绑定
        Observable.CombineLatest(
                activity.ObserveEveryValueChanged(a => a.Meet_Unlock_Requirements()),
            activity.ObserveEveryValueChanged(c => c.Can_Start_Activity()),
            (unlocked, can_start) => unlocked && can_start
        ).Subscribe(canInteract => _startStudyButton.interactable = canInteract)
         .AddTo(this);
        
        
        //// 动态更新按钮状态
        ActivityManager.AM.ObserveEveryValueChanged(am => am.current_activity)
            .Subscribe(current => {
                bool isCurrent = current == activity;
            
                // 更新按钮文本
                _startStudyButton.GetComponentInChildren<TMP_Text>().text = isCurrent ? "Stop" : "Start";
            
                // 更新按钮样式（示例：切换颜色）
                // _learnMathBtn.GetComponent<Image>().color = isCurrent ? Color.red : Color.white;
            
                // 如果需要切换Sprite：
                // _learnMathBtn.GetComponent<Image>().sprite = isCurrent ? stopSprite : startSprite;
            })
            .AddTo(this);

        
        //// 点击事件
        _startStudyButton.OnClickAsObservable()
            .Subscribe(_ =>
                {
                    if (ActivityManager.AM.current_activity != null && activity == ActivityManager.AM.current_activity)
                        ActivityManager.AM.StopCurrentActivity();
                    else 
                        Start_Study(activity);
                }
                ).AddTo(this);
        
        
        ////// 活动进度条显示
        ActivityManager.AM.ObserveEveryValueChanged(a => a.current_interval)
            .CombineLatest(
                ActivityManager.AM.ObserveEveryValueChanged(a => a.tick_interval),
                (current, full) => new { current, full })
            .Subscribe(data =>
            {
                Study_Progress_Bar.maxValue = data.full;

                if (ActivityManager.AM.current_activity != null && activity == ActivityManager.AM.current_activity)
                    Study_Progress_Bar.value = data.current;
                else
                    Study_Progress_Bar.value = 0;
            })
            .AddTo(this);
    }
    

    private Activity Get_Activity_From_AM(string activityId)      // 从 AM 的 All_Activity List 中获取指定的活动
    {
        return ActivityManager.AM.All_Activities
            .FirstOrDefault(a => a.Activity_Id == activityId);
    }

    private void Start_Study(Activity activity)       // 点击按钮时调用的方法，开始学习 or 停止当前学习
    {
        // if (activity.Can_Start_Activity())   //假设上方判断按钮interactable的方法变更，可能需要在此处加上判定
        ActivityManager.AM._Start_Activity(activity);
    }
}
