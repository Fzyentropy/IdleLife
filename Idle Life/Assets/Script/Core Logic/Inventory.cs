using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    //------------------------------------------------------------------------------------------------------------------
    // 所有 Item 索引

    public List<Item_Scriptable> All_Item_Scriptables;         // 所有 Item 集合索引
    public List<Item> All_Items;     // 备选方案，所有 Item 集合索引，使用 Item 类实例
    
    //------------------------------------------------------------------------------------------------------------------
    // 玩家当前拥有的 Item
    
    public int Player_Inventory_Slot_Amount { get; private set; }        // 玩家仓库大小，能存储的 Item 数量
    [ShowInInspector] public Dictionary<string, int> Player_Items { get; private set; }     // 玩家所拥有的 Item,  string (Item名称),  int (Item数量)  @@@@@@@@@@@
    // public Dictionary<Item, int> Player_Items;    // 备选方案，使用 Item实例 Dictionary
    
    //------------------------------------------------------------------------------------------------------------------
    // Inventory 更新事件

    public event Action On_Inventory_Update; 
    
    //------------------------------------------------------------------------------------------------------------------
    // 系统参数：Inventory 单例, Resource 路径

    public static Inventory IVT;
    private const string PATH_ITEMS = "Scriptable_Objects/Items";
    
    
    
    
    

    private void Awake()
    {
        IVT = this;
        Load_All_Items_From_Folder();       // 从 Resource 文件夹中读取
        // Load_All_Items_From_Item_List();     // 从 Item Scriptable Object List 中读取

        Load_Player_Inventory();
    }

    

    
    ////// 所有 Item 的初始化(Item Scriptable => Item), 索引, 加载玩家 Item  ---------------------------------------------

    
    private void Load_All_Items_From_Folder()       // 从 Resources 文件夹加载所有 Item_Scriptable，并转化成 Item 实例存储进 Item List
    {
        All_Items = new List<Item>();
        
        // 加载所有 Item Scriptable
        var itemArray = Resources.LoadAll<Item_Scriptable>(PATH_ITEMS);
        
        foreach (var item_scriptable in itemArray)
        {
            var _item = Create_Item_From_Scriptable(item_scriptable);
            if (_item != null)
            {
                All_Items.Add(_item);
            }
        }
        
        Debug.Log($"已加载{All_Items.Count}项活动");

    }


    private void Load_All_Items_From_Item_List()        // 将一个 Item_Scriptable 转化为 Item 实例
    {
    }


    private Item Create_Item_From_Scriptable(Item_Scriptable itemScriptable)
    {
        
        if (itemScriptable is Item_Scriptable item_instance)
        {
            try
            {
                var item = new Item_NormalItem()        // 普通物品
                {
                    Item_Id = item_instance.Item_Id,
                    Item_Label = item_instance.Item_Label,
                    Item_Image = item_instance.Item_Sprite,
                    Item_Description = item_instance.Item_Description,
                    Item_Type = item_instance.Item_Type,
                    
                    Item_Modules = DeepCopyModules(item_instance.item_modules)
                };
                
                return item;
            }
            catch (Exception e)
            {
                Debug.LogError($"加载物品{item_instance.Item_Id}失败: {e.Message}");
                return null;
            }
        }
        
        if (itemScriptable is Item_Scriptable_NormalItem item_NormalItem_instance)
        {
            try
            {
                var item = new Item_NormalItem()        // 普通物品
                {
                    Item_Id = item_NormalItem_instance.Item_Id,
                    Item_Label = item_NormalItem_instance.Item_Label,
                    Item_Image = item_NormalItem_instance.Item_Sprite,
                    Item_Description = item_NormalItem_instance.Item_Description,
                    Item_Type = item_NormalItem_instance.Item_Type,
                    
                    Item_Modules = DeepCopyModules(item_NormalItem_instance.item_modules)
                };
                
                return item;
            }
            catch (Exception e)
            {
                Debug.LogError($"加载物品{item_NormalItem_instance.Item_Id}失败: {e.Message}");
                return null;
            }
        }
        
        if (itemScriptable is Item_Scriptable_ShopItem item_ShopPurchase_instance)
        {
            try
            {
                var item = new Item_ShopPurchase()        // 商店购买物品
                {
                    Item_Id = item_ShopPurchase_instance.Item_Id,
                    Item_Label = item_ShopPurchase_instance.Item_Label,
                    Item_Image = item_ShopPurchase_instance.Item_Sprite,
                    Item_Description = item_ShopPurchase_instance.Item_Description,
                    Item_Type = item_ShopPurchase_instance.Item_Type,
                    
                    Item_Modules = DeepCopyModules(item_ShopPurchase_instance.item_modules),
                    
                    // 商店购买物品 的特殊属性
                    Item_Purchase_Price = item_ShopPurchase_instance.Item_Purchase_Price
                };
                
                return item;
            }
            catch (Exception e)
            {
                Debug.LogError($"加载物品{item_ShopPurchase_instance.Item_Id}失败: {e.Message}");
                return null;
            }
        }
        
        // 其他类型 Item 的初始化
        /*if (itemScriptable is Item_NormalItem_Scriptable item_NormalItem_instance)
        {
            try
            {
                var item = new Item_NormalItem()        // 普通物品
                {
                    Item_Id = item_NormalItem_instance.Item_Id,
                    Item_Label = item_NormalItem_instance.Item_Label,
                    Item_Image = item_NormalItem_instance.Item_Sprite,
                    Item_Type = item_NormalItem_instance.Item_Type
                };
                
                return item;
            }
            catch (Exception e)
            {
                Debug.LogError($"加载物品{item_NormalItem_instance.Item_Id}失败: {e.Message}");
                return null;
            }
        }*/

        return null;
    }
    
    
    private List<ItemModule> DeepCopyModules(List<ItemModule> source)
    {
        var copy = new List<ItemModule>();
        foreach (var module in source)
        {
            // 实现深拷贝逻辑，可以使用JSON序列化等方式
            // 这里使用简单的手动拷贝示例
            if (module is ItemModule_Upgrade upgrade)
            {
                copy.Add(new ItemModule_Upgrade() { expRequired = upgrade.expRequired, Upgrade_To = upgrade.Upgrade_To});
            }
            else if (module is ItemModule_ShopItem shopItem)
            {
                copy.Add(new ItemModule_ShopItem() { Item_Price = shopItem.Item_Price, Item_Total_Amount = shopItem.Item_Total_Amount});
            }
            else if (module is ItemModule_Equipment equipment)
            {
                copy.Add(new ItemModule_Equipment() { Equipment_Type = equipment.Equipment_Type});
            }
            else if (module is ItemModule_Use use)
            {
                copy.Add(new ItemModule_Use(){use_funcs = use.use_funcs});
            }
            
            // 添加其他模块的拷贝逻辑  TODO 扩展 Item Module
        }
        return copy;
    }


    public Item Get_Item_By_ID_From_IVT(string item_id)             // 通过一个 ID string 获取 Item 实例的方法
    {
        return All_Items.FirstOrDefault(item => item.Item_Id == item_id);
    }
    


    public void Load_Player_Inventory()         // 初始化/加载 玩家 Inventory Dictionary
    {
        // TODO 将来修改为从存档中加载数据
        
        Player_Items = new Dictionary<string, int>();
        Player_Inventory_Slot_Amount = 10;
    }



    //------------------------------------------------------------------------------------------------------------------
    
    
    
    //////  库存 加入、取出 处理方法

    public void Add_Item_To_Inventory(string itemID, int itemAmount)      // 向仓库内添加物品
    {
        
        if (Player_Items.ContainsKey(itemID))   // 若已经存在该物品，则增加数量
        {
            Player_Items[itemID]++;
        }
        
        else   // 若未拥有该物品
        {
            // 检查仓库是否已满，TODO
            
            Player_Items.Add(itemID,itemAmount);    // 添加该物品
        }
        
        On_Inventory_Update?.Invoke();      // 触发 Inventory 更新事件
        
    }


    public void Remove_Item_From_Inventory(string itemID, int itemAmount)      // 从仓库内取出物品
    {
        if (Player_Items.ContainsKey(itemID) && Player_Items[itemID] >= itemAmount)     // 若有该物品，且数量足够扣除
        {
            // 扣除指定数量的该物品
            if (Player_Items[itemID] > itemAmount)
                Player_Items[itemID] -= itemAmount;
            else
                Player_Items.Remove(itemID);

        }
        
        On_Inventory_Update?.Invoke();      // 触发 Inventory 更新事件
    }




}
