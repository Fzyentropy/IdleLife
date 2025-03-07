using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string Item_Id;
    public string Item_Label;
    public Sprite Item_Image;
    public string Item_Description;
    public string Item_Type;
    
    public List<ItemModule> Item_Modules = new List<ItemModule>();    // Item 组件集合，用于定义一个 Item 拥有的功能模块
}
