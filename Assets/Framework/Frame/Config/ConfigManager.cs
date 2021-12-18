/****************************************************
    文件：ConfigManager.cs
    作者：TA94
    日期：2021/10/31 14:45:17
    功能：Nothing
*****************************************************/
using INFramework;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager :Singleton<ConfigManager>
{
    protected Dictionary<string, ExcelBase> mAllExcelData = new Dictionary<string, ExcelBase>();

    private ConfigManager()
    {

    }

    /// <summary>
    /// 加载数据表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rPath"></param>
    /// <returns></returns>
    public T LoadData<T>(string rPath) where T : ExcelBase
    {
        if (string.IsNullOrEmpty(rPath))
        {
            return null;
        }

        if (mAllExcelData.ContainsKey(rPath))
        {
            Debug.LogError($"加载重复的配置：{rPath}");
            return mAllExcelData[rPath] as T;
        }

        T data = SerializeHelper.BinaryDeserialize<T>(rPath);
#if UNITY_EDITOR
        if(data == null)
        {
            Debug.Log($"{rPath}不存在, 从xml加载数据了！");
            string xmlPath = rPath.Replace("Binary", "Xml");
            data = SerializeHelper.XmlDeserialize<T>(rPath);
        }
#endif
        if(data != null)
        {
            data.Init();
        }
        mAllExcelData.Add(rPath, data);
        return data;

    }

    /// <summary>
    /// 根据路径查找配置数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rPath"></param>
    /// <returns></returns>
    public T FindData<T>(string rPath) where T:ExcelBase
    {
        if (string.IsNullOrEmpty(rPath))
        {
            return null;
        }

        ExcelBase excelBase = null;
        if(mAllExcelData.TryGetValue(rPath, out excelBase))
        {
            return excelBase as T;
        }
        else
        {
            excelBase = LoadData<T>(rPath);
        }
        return (T)excelBase;
    }
}

public class GameConfigTable
{
    //配置表路径
}
