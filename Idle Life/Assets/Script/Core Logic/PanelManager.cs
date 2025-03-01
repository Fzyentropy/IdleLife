using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    
    ///////  所有 Action Panel prefab

    [Header("左侧Activity列表 和 Panel父节点")]
    public GameObject Activity_List;            // 所有 Activity 组成的 Scroll List
    public GameObject Activity_Panel_Parent;    // 所有 Activity Panel 的父节点

    [Header("Activity Panel 们")] 
    public List<GameObject> Activity_Panel_List;
    
    [Space(10)]
    public GameObject current_panel; // 记录当前打开 Panel 的 GameObject

    public static PanelManager PM;


    private void Awake()
    {
        PM = this;
    }

    private void Start()
    {
        Check_Activity_List_And_Panel_Parent();
    }




    private void Check_Activity_List_And_Panel_Parent()
    {
        if (Activity_List == null || Activity_Panel_Parent == null)
            Debug.LogError("Activity列表 或 Panel父节点 实例未绑定");
        
        else if (Activity_List.activeSelf != Activity_Panel_Parent.activeSelf)
            Debug.LogError("左侧Activity列表 和 Panel父节点 的Active状态不相同");
    }
    

    public void Open_Activity_List_And_Panel_Parent()        //  打开 Activity List 按钮调用 方法
    {
        Activity_List.SetActive(!Activity_List.activeSelf);
        Activity_Panel_Parent.SetActive(!Activity_Panel_Parent.activeSelf);
    }

    
    public void Open_Activity_Panel(GameObject activity_panel_to_open)
    {
        GameObject panel = Activity_Panel_List.FirstOrDefault(panel => panel == activity_panel_to_open);
        
        if (current_panel != null)
        {
            if (current_panel != panel)       // 若当前 Panel 与要打开的不同，则关闭当前 Panel，打开新 Panel
            {
                current_panel.SetActive(false);
                panel.SetActive(true);
                current_panel = panel;
            }
            else                              // 若当前 Panel 与要打开的 Panel 相同，则关闭当前 Panel
            {
                current_panel.SetActive(false);
                current_panel = null;
            }
        }
        else                     // 若当前无 Panel，直接打开新 Panel
        {
            panel.SetActive(true);
            current_panel = panel;
        }
        

    }
    
    

}
