using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Testing_Script_Global : MonoBehaviour
{
    
    //------------------------------------------------------------------------------------------------------------------
    
    [Space(5)][Header("游戏加速速率 - F")]
    [Sirenix.OdinInspector.ReadOnly] public float copy_am_tick_speed;   // 复制 AM中的 游戏Tick速度
    public float temp_accelerated_tick_speed;       // 临时 游戏Tick速度
    
    [Space(5)][Header("玩家基础体力回复速率 - S")]
    [Sirenix.OdinInspector.ReadOnly] public float copy_player_stamina_base_restore_rate;    // 复制 GM中的 玩家基础体力回复速率
    public float temp_accelerated_player_stamina_base_restore_rate;     // 临时 玩家基础体力回复速率
    
    
    //------------------------------------------------------------------------------------------------------------------
    
    private void Start()
    {
        Copy_Player_Variables();      // 复制各脚本中玩家状态参数，以便修改后恢复原状
        Put_Item_To_Inventory_At_Start();      // 开局往 Inventory 放入一些 Item
    }

    void Update()
    {
        SpeedUp_Activity();     // 按 F 加速游戏 - 调用
        SpeedUp_Stamina_Restore();    // 按 S 加速体力值回复 - 调用
    }


    private void Copy_Player_Variables()    // 复制各脚本中玩家状态参数，以便修改后恢复原状 - 具体方法
    {
        copy_am_tick_speed = ActivityManager.AM.tick_speed;     // 游戏 Tick 速度
        copy_player_stamina_base_restore_rate = GameManager.GM.Player_Stamina_Base_Restore_Rate;     // 基础体力值回复速率
    }
    
    
    //------------------------------------------------------------------------------------------------------------------

    public void SpeedUp_Activity()       // 按 F 加速 - 具体实现
    {
        // 加速时，游戏速度设为 临时加速值
        if (Input.GetKey(KeyCode.F))
            ActivityManager.AM.tick_speed = temp_accelerated_tick_speed;
        
        // 平时，游戏速度设为 AM中设置的值
        else
            ActivityManager.AM.tick_speed = copy_am_tick_speed;
        
    }

    private void SpeedUp_Stamina_Restore()    // 按 S 加速体力值回复 - 具体实现
    {
        // 加速体力回复时，体力回复速率 设为 临时加速值
        if (Input.GetKey(KeyCode.S))
            GameManager.GM.Player_Stamina_Base_Restore_Rate = temp_accelerated_player_stamina_base_restore_rate;
        
        // 平时，体力回复速率 设为 AM中设置的值
        else
            GameManager.GM.Player_Stamina_Base_Restore_Rate = copy_player_stamina_base_restore_rate;
    }

    public void Put_Item_To_Inventory_At_Start()        // 开局往 Inventory 放入一些 Item - 具体实现
    {
        Inventory.IVT.Player_Items.Add("Item_Townhouse", 2);
        Inventory.IVT.Player_Items.Add("Food", 3);
        Inventory.IVT.Player_Items.Add("Weapon_Sword", 1);
        Debug.Log("Item 已添加");
    }
    
    //------------------------------------------------------------------------------------------------------------------
    
    
    
}
