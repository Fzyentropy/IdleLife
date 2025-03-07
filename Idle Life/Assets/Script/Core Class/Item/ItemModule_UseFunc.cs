using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_ItemModule_UseFunc       // Item使用 接口
{
    void Use_Item(Item item);
}

public abstract class ItemModule_UseFunc : ScriptableObject, I_ItemModule_UseFunc       // Item使用 策略模式 父类
{
    public abstract void Use_Item(Item item);
}


