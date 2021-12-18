/****************************************************
    文件：SerializeHelper.cs
    作者：TA94
    日期：2021/10/31 14:11:44
    功能：Nothing
*****************************************************/
using INFramework.Framework.AssetBundles;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class SerializeHelper
{

    /// <summary>
    /// 类序列化成xml
    /// </summary>
    /// <param name="rPath">类路径</param>
    /// <param name="rObj">类对象</param>
    /// <returns></returns>
    public static bool XmlSerialize(string rPath, System.Object rObj)
    {
        try
        {
            using (FileStream fs = new FileStream(rPath, FileMode.Create, FileAccess.ReadWrite))
            {
                using(StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    XmlSerializer xs = new XmlSerializer(rObj.GetType());
                    xs.Serialize(sw, rObj);
                }
            }
            return true;
        }
        catch(Exception e)
        {
            Debug.LogError("此类无法转换成xml" + rObj.GetType() + "," + e);
        }

        return false;
    }

    /// <summary>
    /// 编辑器下读取xml
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rPath"></param>
    /// <returns></returns>
    public static T XmlDeserialize<T>(string rPath) where T:class
    {
        T t = default(T);
        try
        {
            using(FileStream fs = new FileStream(rPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                t = (T)xs.Deserialize(fs);
            }
        }
        catch(Exception e)
        {
            Debug.LogError($"此xml无法转换成而类：{rPath}, {e}");
        }
        return t;
    }

    /// <summary>
    /// 编辑器下读取xml
    /// </summary>
    /// <param name="rPath"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static System.Object XmlDeserialize(string rPath, Type type)
    {
        System.Object obj = null;
        try
        {
            using (FileStream fs = new FileStream(rPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                XmlSerializer xs = new XmlSerializer(type);
                obj = xs.Deserialize(fs);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"此xml无法转换成而类：{rPath}, {e}");
        }
        return obj;
    }

    /// <summary>
    /// 运行时读取xml
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rPath"></param>
    /// <returns></returns>
    public static T XmlDeserializeRuntime<T>(string rPath) where T : class
    {
        T t = default(T);
        TextAsset textAsset = ResourceManager.Instance.LoadResource<TextAsset>(rPath);
        if(textAsset == null)
        {
            Debug.LogError($"cant load TextAsset:{rPath}");
            return null;
        }
        try
        {
            using(MemoryStream stream = new MemoryStream(textAsset.bytes))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                t = (T)xs.Deserialize(stream);
            }
            ResourceManager.Instance.ReleaseResouce(rPath, true);
        }
        catch(Exception e)
        {
            Debug.LogError($"load TextAsset exception : {rPath}, {e}");
        }
        return t;
    }

    /// <summary>
    /// 类转化成二进制
    /// </summary>
    /// <param name="rPath"></param>
    /// <param name="rObj"></param>
    /// <returns></returns>
    public static bool BinarySerialize(string rPath, System.Object rObj)
    {
        try
        {
            using(FileStream fs = new FileStream(rPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, rObj);
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"此类无法转换成二进制 {rObj.GetType()}, {e}");
        }
        return false;
    }

    /// <summary>
    /// 二进制文件转化成类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rPath"></param>
    /// <returns></returns>
    public static T BinaryDeserialize<T>(string rPath) where T : class
    {
        T t = default(T);
        TextAsset textAsset = ResourceManager.Instance.LoadResource<TextAsset>(rPath);
        if(textAsset == null)
        {
            Debug.LogError($"cant load TextAsset:{rPath}");
        }
        try
        {
            using(MemoryStream ms = new MemoryStream(textAsset.bytes))
            {
                BinaryFormatter bf = new BinaryFormatter();
                t = (T)bf.Deserialize(ms);
            }
            ResourceManager.Instance.ReleaseResouce(rPath, true);
        }
        catch(Exception e)
        {
            Debug.LogError($"load TextAsset expection:{rPath}, {e}");
        }

        return t;
    }
}
