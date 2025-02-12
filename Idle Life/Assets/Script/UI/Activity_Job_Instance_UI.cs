using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using TMPro;
using UnityEngine.UI;

public class Activity_Job_Instance_UI : MonoBehaviour
{
    [Header("Job 实例 Scriptable")]
    public Activity_Job_Scriptable Activity_Job_Instance;
    
    [Header("UI 元素")]
    [SerializeField] private Button _startJobButton;
    [SerializeField] private Slider Job_Progress_Bar;


    private void Start()
    {
        Setup_Job();
    }
    

    private void Setup_Job()
    {
        var activity = Get_Activity_From_AM(Activity_Job_Instance.Activity_Id);

        //// 按钮状态绑定
        Observable.CombineLatest(
            GameManager.GM.ObserveEveryValueChanged(gm => gm.Player_Stamina),
            activity.ObserveEveryValueChanged(a => a.Meet_Unlock_Requirements()),
            (stamina, unlocked) => stamina >= activity.Required_Stamina && unlocked
        ).Subscribe(canInteract => _startJobButton.interactable = canInteract)
         .AddTo(this);
        
        
        //// 动态更新按钮状态
        ActivityManager.AM.ObserveEveryValueChanged(am => am.current_activity)
            .Subscribe(current => {
                bool isCurrent = current == activity;
            
                // 更新按钮文本
                _startJobButton.GetComponentInChildren<TMP_Text>().text = isCurrent ? "Stop" : "Work";
            
                // 更新按钮样式（示例：切换颜色）
                // _learnMathBtn.GetComponent<Image>().color = isCurrent ? Color.red : Color.white;
            
                // 如果需要切换Sprite：
                // _learnMathBtn.GetComponent<Image>().sprite = isCurrent ? stopSprite : startSprite;
            })
            .AddTo(this);

        
        //// 点击事件
        _startJobButton.OnClickAsObservable()
            .Subscribe(_ =>
                {
                    if (ActivityManager.AM.current_activity != null && activity.Activity_Id == ActivityManager.AM.current_activity.Activity_Id)
                        ActivityManager.AM.StopCurrentActivity();
                    else 
                        Start_Job(activity);
                }
                ).AddTo(this);
        
        
        ////// 活动进度条显示
        ActivityManager.AM.ObserveEveryValueChanged(a => a.current_interval)
            .CombineLatest(
                ActivityManager.AM.ObserveEveryValueChanged(a => a.tick_interval),
                (current, full) => new { current, full })
            .Subscribe(data =>
            {
                Job_Progress_Bar.maxValue = data.full;

                if (ActivityManager.AM.current_activity != null && activity.Activity_Id == ActivityManager.AM.current_activity.Activity_Id)
                    Job_Progress_Bar.value = data.current;
                else
                    Job_Progress_Bar.value = 0;
            })
            .AddTo(this);
    }
    

    private Activity Get_Activity_From_AM(string activityId)      // 从 AM 的 All_Activity List 中获取指定的活动
    {
        return ActivityManager.AM.All_Activities
            .FirstOrDefault(a => a.Activity_Id == activityId);
    }

    private void Start_Job(Activity activity)       // 点击按钮时调用的方法，开始学习 or 停止当前学习
    {
        if (activity.Can_Start_Activity())
        {
            ActivityManager.AM._Start_Activity(activity);
        }
    }
}
