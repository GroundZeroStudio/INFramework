/****************************************************
    文件：BuildEditor.cs
    作者：TA94
    日期：2021/10/16 15:33:25
    功能：Nothing
*****************************************************/
using INFramework.Framework.AssetBundles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using FileMode = System.IO.FileMode;

public class BundleEditor
{
    private static string m_BuildTargetPath = Application.dataPath + "/../AssetBundle/" +EditorUserBuildSettings.activeBuildTarget.ToString();
    private static string ABCONFIGPATH = "Assets/ResFramework/Editor/Resource/ABConfig.asset";
    private static string ABBYTEPATH = LoadFrameConfig.GetFrameConfig().m_ABbytePath;//"Assets/GameData/Data/ABData/AssetBundleConfig.bytes";
    //key要设置的AB包名，key是需要被标记的资源路径
    private static Dictionary<string, string> _allFileDirABPath = new Dictionary<string, string>();
    //key要设置的预制体名字，value需要被标记的预制体及其依赖资源的路径
    private static Dictionary<string, List<string>> _allPrefabFile = new Dictionary<string, List<string>>();
    //标记ab的文件资源
    private static List<string> _allMarkABFilePath = new List<string>();
    //被写入配置的ab包有效路径
    private static List<string> _configFilePath = new List<string>();
    private static void ClearABPath()
    {
        _configFilePath.Clear();
        _allFileDirABPath.Clear();
        _allPrefabFile.Clear();
        _allMarkABFilePath.Clear();
    }

    [MenuItem("Tools/打包")]
    public static void Build()
    {
        ClearABPath();
        #region 过滤冗余
        ABConfig abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(ABCONFIGPATH);
        //将所有文件夹打包信息保存下来
        foreach (var fileDirInfo in abConfig._allFileDirPath)
        {
            if (_allFileDirABPath.ContainsKey(fileDirInfo.ABName))
            {
                Debug.LogError($"存在重复的打包文件{fileDirInfo.ABName}， 路径为{fileDirInfo.Path}");
            }
            else
            {
                _allFileDirABPath.Add(fileDirInfo.ABName, fileDirInfo.Path);
                _allMarkABFilePath.Add(fileDirInfo.Path);
                //每个文件夹都是一个ab包
                _configFilePath.Add(fileDirInfo.Path);
            }
        }
        //以单个预制体为ab包，保存包名以及依赖的资源路径
        string[] allPrefabs = AssetDatabase.FindAssets("t:Prefab", abConfig._PrefabFilePath.ToArray());
        for (int index = 0; index < allPrefabs.Length; index++)
        {
            string guid = allPrefabs[index];
            //预制体路径
            string path = AssetDatabase.GUIDToAssetPath(guid);
            //一个预制体也是一个有效路径（所以其他的资源会在其它ab包中）
            _configFilePath.Add(path);
            if (!ContainPath(path))
            {
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                EditorUtility.DisplayProgressBar("查找Prefab：", "Prefab：" + obj.name, index * 1.0f / allPrefabs.Length);
                string[] dependecs = AssetDatabase.GetDependencies(path);
                List<string> tempDepences = new List<string>();
                for (int i = 0; i < dependecs.Length; i++)
                {
                    if (!ContainPath(dependecs[i]) && !dependecs[i].EndsWith(".cs"))
                    {
                        tempDepences.Add(dependecs[i]);
                        _allMarkABFilePath.Add(dependecs[i]);
                    }
                    //Debug.Log($"{obj.name}所依赖的资源路径为：{dependecs[i]}");
                }

                if (_allPrefabFile.ContainsKey(obj.name))
                {
                    Debug.LogError($"存在重复的ABName{obj.name}");
                }
                else
                {
                    _allPrefabFile.Add(obj.name, tempDepences);
                }
            }

        }
        #endregion

        foreach (var assetName in _allFileDirABPath.Keys)
        {
            SetAssetBundleName(assetName, _allFileDirABPath[assetName]);
        }

        foreach (var assetName in _allPrefabFile.Keys)
        {
            SetAssetBundleName(assetName, _allPrefabFile[assetName]);
        }

        BuildAssetBundle();

        //清除标记
        var oldABNames = AssetDatabase.GetAllAssetBundleNames();
        for (int i = 0; i < oldABNames.Length; i++)
        {
            AssetDatabase.RemoveAssetBundleName(oldABNames[i], true);
            EditorUtility.DisplayProgressBar("清除AB包名", "名字：" + oldABNames[i], i * 1.0f / oldABNames.Length);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
    }

    private static void BuildAssetBundle()
    {
        //保存AssetBundleNames
        string[] rAllAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
        //key是路径名，value是ab包名字
        Dictionary<string, string> rResDic = new Dictionary<string, string>();
        for (int i = 0; i < rAllAssetBundleNames.Length; i++)
        {
            //一个ab包下的所有资源路径
            string[] allBundlePath = AssetDatabase.GetAssetPathsFromAssetBundle(rAllAssetBundleNames[i]);
            for (int index = 0; index < allBundlePath.Length; index++)
            {
                Debug.Log($"AB包：{rAllAssetBundleNames[i]}---下面包含的资源文件路径：{allBundlePath[index]}");
                if (allBundlePath[index].EndsWith(".cs") || !VaildPath(allBundlePath[index])) continue;
                rResDic.Add(allBundlePath[index], rAllAssetBundleNames[i]);
            }
        }
        if (!Directory.Exists(m_BuildTargetPath))
        {
            Directory.CreateDirectory(m_BuildTargetPath);
        }
        //删除无效的AB包
        DeleteAB();
        //生成配置
        WriteData(rResDic);
        //打包
        AssetBundleManifest manifest  = BuildPipeline.BuildAssetBundles(m_BuildTargetPath, BuildAssetBundleOptions.ChunkBasedCompression,
            EditorUserBuildSettings.activeBuildTarget);
        if (manifest == null)
        {
            Debug.LogError("AssetBundle 打包失败！");
        }
        else
        {
            Debug.Log("AssetBundle 打包完毕");
        }
    }

    private static void WriteData(Dictionary<string, string> rResDict)
    {
        AssetBundleConfig abConfig = new AssetBundleConfig();
        abConfig.ABList = new List<ABBase>();
        foreach (var path in rResDict.Keys)
        {
            ABBase abBase = new ABBase();
            abBase.Path = path;
            abBase.Crc = CRC32.GetCRC32(path);
            abBase.ABName = rResDict[path];
            abBase.AssetName = path.Remove(0, path.LastIndexOf("/") + 1);
            abBase.ABDependce = new List<string>();
            string[] depences = AssetDatabase.GetDependencies(path);
            for (int i = 0; i < depences.Length; i++)
            {
                string tempPath = depences[i];

                if (path == tempPath || path.EndsWith(".cs"))
                    continue;

                if (rResDict.TryGetValue(tempPath, out string ABName))
                {
                    if (ABName == rResDict[path]) continue;
                    if (!abBase.ABDependce.Contains(ABName))
                    {
                        abBase.ABDependce.Add(ABName);
                    }
                }
            }
            abConfig.ABList.Add(abBase);
        }

        string xmlPath = Application.dataPath + "/AssetBundleConfig.xml";
        if (File.Exists(xmlPath))
        {
            File.Delete(xmlPath);
        }

        //写入xml
        FileStream fs = new FileStream(xmlPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
        XmlSerializer xs = new XmlSerializer(abConfig.GetType());
        xs.Serialize(sw, abConfig);
        sw.Close();
        fs.Close();

        foreach (var abBase in abConfig.ABList)
        {
            abBase.Path = "";
        }
        //写入二进制
        FileStream fs1 = new FileStream(ABBYTEPATH, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        fs1.Seek(0, SeekOrigin.Begin);
        fs1.SetLength(0);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs1, abConfig);
        fs1.Close();
        AssetDatabase.Refresh();
        SetAssetBundleName("assetbundleconfig", ABBYTEPATH);
    }

    private static void DeleteAB()
    {
        //获取所有的AB包
        string[] allBuildsName = AssetDatabase.GetAllAssetBundleNames();
        DirectoryInfo directoryInfo = new DirectoryInfo(m_BuildTargetPath);
        FileInfo[] files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            if (ContainABName(files[i].Name, allBuildsName) || files[i].Name.EndsWith(".meta") || files[i].Name.EndsWith(".manifest")
                || files[i].Name.EndsWith("assetbundleconfig"))
            {
                continue;
            }
            else
            {
                Debug.Log($"此AB包已经失效：{files[i].Name}");
                if (File.Exists(files[i].FullName))
                {
                    File.Delete(files[i].FullName);
                }

                if(File.Exists(files[i] + ".manifest"))
                {
                    File.Delete(files[i].FullName + ".manifest");
                }
            }
        }
    }

    private static void SetAssetBundleName(string rAssetBundleName, string rPath)
    {
        AssetImporter importer = AssetImporter.GetAtPath(rPath);
        if (importer == null)
        {
            Debug.LogError($"未找到资源路径{rPath}，请检查");
        }
        else
        {
            importer.assetBundleName = rAssetBundleName;
        }
    }

    private static void SetAssetBundleName(string rAssetBundleName, List<string> rPaths)
    {
        for (int i = 0; i < rPaths.Count; i++)
        {
            SetAssetBundleName(rAssetBundleName, rPaths[i]);
        }
    }
    
    private static bool ContainPath(string path)
    {
        for (int i = 0; i < _allMarkABFilePath.Count; i++)
        {
            if (path == _allMarkABFilePath[i] || path.Contains(_allMarkABFilePath[i]) && (path.Replace(_allMarkABFilePath[i], "")[0] == '/'))
            {
                return true;
            }
        }
        
        return false;
    }

    private static bool ContainABName(string name, string[] rStr)
    {
        for (int i = 0; i < rStr.Length; i++)
        {
            if (name == rStr[i])
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 此资源是否包含在其他ab中，不是的话才是一个有效的配置路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static bool VaildPath(string path)
    {
        for (int i = 0; i < _configFilePath.Count; i++)
        {
            if (path.Contains(_configFilePath[i]))
            {
                return true;
            }
        }

        return false;
    }
}
