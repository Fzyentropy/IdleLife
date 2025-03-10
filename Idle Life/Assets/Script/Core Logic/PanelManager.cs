using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    
    public static PanelManager PM;
    
    //------------------------------------------------------------------------------------------------------------------
    // Activity Panel 相关

    [Space(10)][Header("左侧Activity列表 和 Panel父节点")]
    public GameObject Activity_List;            // 所有 Activity 组成的 Scroll List
    public GameObject Activity_Panel_Parent;    // 所有 Activity Panel 的父节点

    [Space(10)][Header("Activity Panel 们")] 
    public List<GameObject> Activity_Panel_List;    // 所有的 Activity Panel GameObject
    [Space(5)]
    [Sirenix.OdinInspector.ReadOnly] public GameObject current_activity_panel; // 记录当前打开 Panel 的 GameObject
    
    //------------------------------------------------------------------------------------------------------------------
    // Side Panel 相关
    
    [Space(10)][Header("Side Panel 们")]
    [Space(5)][Header("右侧物品信息 Panel")] 
    public Item_Info_Panel_UI Item_Info_Panel;     // Item Info Panel 指代，即Item具体信息和操作的Panel，将场景中Panel拖拽至此
    [Sirenix.OdinInspector.ReadOnly] [CanBeNull] public Item Current_Open_Item;   // 当前 Item Info Panel 显示的 Item
    
    

    private void Awake()
    {
        PM = this;
    }

    private void Start()
    {
        Check_All_Panel_Setup();
        Check_Item_Panel_Validation();
        Inventory.IVT.On_Inventory_Update += Check_Item_Panel_Validation;
    }




    private void Check_All_Panel_Setup()        // 检测整个 Panel Manager 必要的 Setup 是否完成
    {
        // 检测 左侧活动列表，活动panel父节点，是否拖拽绑定
        if (Activity_List == null || Activity_Panel_Parent == null)      
            Debug.LogError("Activity列表 或 Panel父节点 实例未绑定");
        
        // 检测 左侧活动列表的Active状态 与 活动Panel父节点的Active状态是否一致（以便开关时一同打开/关闭）
        else if (Activity_List.activeSelf != Activity_Panel_Parent.activeSelf)
            Debug.LogError("左侧Activity列表 和 Panel父节点 的Active状态不相同");
        
        // 检测 右侧 Item Info 显示和操作Panel 是否拖拽绑定
        if (Item_Info_Panel == null)
            Debug.LogError("Item Info Panel 未拖拽绑定");
    }
    

    public void Open_Activity_List_And_Panel_Parent()        //  打开 Activity List 按钮调用 方法
    {
        Activity_List.SetActive(!Activity_List.activeSelf);
        Activity_Panel_Parent.SetActive(!Activity_Panel_Parent.activeSelf);
    }

    
    public void Open_Activity_Panel(GameObject activity_panel_to_open)      // 打开某一特定 Activity Panel 的方法
    {
        GameObject panel = Activity_Panel_List.FirstOrDefault(panel => panel == activity_panel_to_open);
        
        if (current_activity_panel != null)
        {
            if (current_activity_panel != panel)       // 若当前 Panel 与要打开的不同，则关闭当前 Panel，打开新 Panel
            {
                current_activity_panel.SetActive(false);
                panel.SetActive(true);
                current_activity_panel = panel;
            }
            else                              // 若当前 Panel 与要打开的 Panel 相同，则关闭当前 Panel
            {
                current_activity_panel.SetActive(false);
                current_activity_panel = null;
            }
        }
        else                     // 若当前无 Panel，直接打开新 Panel
        {
            panel.SetActive(true);
            current_activity_panel = panel;
        }
        

    }


    public void Open_Item_Info_Panel(Item item)         // 点击 Inventory 中的 Item 时，打开右侧 Item 详细信息和操作接口
    {
        
        // if 有别的 Panel 在相应的位置打开，则关闭它们
        {
            
            
        }
        
        // 若当前有 Item 已经打开了 Panel（则也肯定不是输入的 item，因为调用此方法的 Inventory_Item_UI 已经判定过，若是则会调用下方关闭Panel方法）
        if (Current_Open_Item != null)
        {
            Current_Open_Item = item;
            Item_Info_Panel.Check_And_Set_Panel();    // 刷新 Item Info Panel
        }
        
        // 当前没有任何 Item 打卡了 Panel 
        else   
        {
            Current_Open_Item = item;    // 将 Item Info Panel 的 Item 设置为输入的 Item
            Item_Info_Panel.gameObject.SetActive(true);
        }
        
    }


    public void Close_Item_Info_Panel()
    {
        Item_Info_Panel.gameObject.SetActive(false);
        Current_Open_Item = null;
    }

    void Check_Item_Panel_Validation()  // 检查当前打开的Item 是否还在Inventory中拥有，以防因其他活动消耗掉物品时还能继续使用
    {
        if (Current_Open_Item != null)
        {
            // 如果没有该 Item，则关闭 Panel
            if (!Inventory.IVT.Has_Item(Current_Open_Item)) 
            {
                Close_Item_Info_Panel();
            }
            
            // 如果有，则更新一下信息
            else
            {
                Item_Info_Panel.Check_And_Set_Panel();    // 刷新 Item Info Panel
            }
        }
        
    }
    
    

}
