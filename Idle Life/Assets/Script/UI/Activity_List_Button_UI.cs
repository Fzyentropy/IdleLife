using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Activity_List_Button_UI : MonoBehaviour
{
    [Header("对应的 Activity Panel")]
    public GameObject _Activity_Panel;      // 该 Activity 的 Panel prefab
    
    [Space(10)]
    public Button _Activity_Button;

    

    private void Start()
    {
        Check_Prefab();
        Set_Button();
    }


    private void Check_Prefab()
    {
        if (_Activity_Panel == null)
            Debug.LogError("Activity 对应的 Panel prefab 未设置");
    }
    

    private void Set_Button()
    {
        _Activity_Button.OnClickAsObservable()
            .Subscribe(_ =>
                {
                    PanelManager.PM.Open_Activity_Panel(_Activity_Panel);
                }
            ).AddTo(this);
    }

}
