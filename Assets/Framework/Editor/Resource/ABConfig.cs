/****************************************************
    文件：ABConfig.cs
    作者：TA94
    日期：2021/10/16 15:26:11
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ABConfig", menuName = "CreateABConfig", order = 0)]
public class ABConfig : ScriptableObject
{
    //单个文件夹路径集合（Prefab）
    public List<string> _PrefabFilePath;

    public List<FileDirABName> _allFileDirPath;
}

/// <summary>
/// 文件件路径 
/// </summary>
[Serializable]
public class FileDirABName
{
    public string ABName;
    public string Path;
}
