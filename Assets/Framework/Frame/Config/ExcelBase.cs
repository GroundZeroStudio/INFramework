/****************************************************
    文件：ExcelManager.cs
    作者：TA94
    日期：2021/10/31 14:45:47
    功能：
            1.编辑器下生成基本数据
            2.运行时初始化保存数据，降低搜索复杂度  
*****************************************************/
using System;
using UnityEngine;

[Serializable]
public class ExcelBase 
{
    public virtual void Construction() { }

    public virtual void Init() { }
}
