using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Show_Current_Activity_UI : MonoBehaviour
{

    [Header("UI元素")] 
    public GameObject Current_Activity_Bar;
    public Slider Progress_Bar;
    public TMP_Text Ongoing_Text;
    public TMP_Text Activity_Text;
    
    
    
    void Start()
    {
        Set_Current_Activity_Object();
        Set_Up_Current_Activity_UI();
    }

    private void Set_Current_Activity_Object()
    {
        if (Current_Activity_Bar == null)
            Current_Activity_Bar = gameObject;
    }

    public void Set_Up_Current_Activity_UI()
    {
        
        ////// 活动进度条显示
        ActivityManager.AM.ObserveEveryValueChanged(a => a.current_interval)
            .CombineLatest(
                ActivityManager.AM.ObserveEveryValueChanged(a => a.tick_interval),
                (current, full) => new { current, full })
            .Subscribe(data =>
            {
                Progress_Bar.maxValue = data.full;
                Progress_Bar.value = data.current;
            })
            .AddTo(this);
        
        
        ////// 是否有 Activity 决定 Active 状态
        ActivityManager.AM.ObserveEveryValueChanged(am => am.current_activity)
            .Subscribe(activity =>
            {
                if (activity != null)
                {
                    Activity_Text.text = $"{activity.Activity_Type}: {activity.Activity_Label}";
                    Current_Activity_Bar.SetActive(true);
                }
                else
                {
                    Current_Activity_Bar.SetActive(false);
                    Activity_Text.text = "";
                }
            })
            .AddTo(this);

    }
}
