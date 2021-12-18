/****************************************************
    文件：OfflineDataEditor.cs
    作者：TA94
    日期：2021/10/27 21:26:52
    功能：保存初始数据
*****************************************************/
using UnityEditor;
using UnityEngine;

public class OfflineDataEditor
{
    #region 生成预制体离线数据
    [MenuItem("Assets/生成Prefab离线数据")]
    public static void AssetCreateOfflineData()
    {
        GameObject[] objects = Selection.gameObjects;
        for (int i = 0; i < objects.Length; i++)
        {
            EditorUtility.DisplayProgressBar("添加离线数据", "正在修改" +
                objects[i] + "......", i * 1.0f / objects.Length);
            CreateOfflineData(objects[i]);
        }
        EditorUtility.ClearProgressBar();
    }

    public static void CreateOfflineData(GameObject obj)
    {
        OfflineData offlineData = obj.GetComponent<OfflineData>();
        if (offlineData == null)
        {
            offlineData = obj.AddComponent<OfflineData>();
        }
        offlineData.BindData();
        EditorUtility.SetDirty(obj);
        Debug.Log($"修改了{obj.name}.prefab");
        Resources.UnloadUnusedAssets();
        AssetDatabase.Refresh();
    }
    #endregion

    #region 生成UI离线数据
    [MenuItem("离线数据/生成所有UI Prefab离线数据")]
    public static void CreateAllUIData()
    {
        //查找指定文件夹下所有的预制体
        string[] guidPaths = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/GameData/Prefabs/UGUI" });
        for (int i = 0; i < guidPaths.Length; i++)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(guidPaths[i]);
            EditorUtility.DisplayProgressBar("添加UI 离线数据", "正在扫描路径：" + prefabPath + "......", i * 1.0f / guidPaths.Length);
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (obj == null)
                continue;
            CreateUIData(obj);
        }
        EditorUtility.ClearProgressBar();
    }


    [MenuItem("Assets/生成UI Prefab离线数据")]
    public static void AssetCreateUIData()
    {
        GameObject[] objects = Selection.gameObjects;
        for (int i = 0; i < objects.Length; i++)
        {
            EditorUtility.DisplayProgressBar("添加离线数据", "正在修改" + objects[i] + "......", i * 1.0f / objects.Length);
            CreateUIData(objects[i]);
        }
        EditorUtility.ClearProgressBar();
    }

    public static void CreateUIData(GameObject obj)
    {
        obj.layer = LayerMask.NameToLayer("UI");
        UIOfflineData offlineData = obj.GetComponent<UIOfflineData>();
        if(offlineData == null)
        {
            offlineData = obj.AddComponent<UIOfflineData>();
        }
        offlineData.BindData();
        EditorUtility.SetDirty(obj);
        Debug.Log($"修改了{obj.name}UI");
        Resources.UnloadUnusedAssets();
        AssetDatabase.Refresh();
    }
    #endregion

    #region 生成特效离线数据
    [MenuItem("离线数据/生成所有Effect Prefab离线数据")]
    public static void CreateAllEffectData()
    {
        //查找指定文件夹下所有的预制体
        string[] guidPaths = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/GameData/Prefabs/Effect" });
        for (int i = 0; i < guidPaths.Length; i++)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(guidPaths[i]);
            EditorUtility.DisplayProgressBar("添加Effect 离线数据", "正在扫描路径：" + prefabPath + "......", i * 1.0f / guidPaths.Length);
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (obj == null)
                continue;
            CreateEffectData(obj);
        }
        EditorUtility.ClearProgressBar();
    }


    [MenuItem("Assets/生成UI Prefab离线数据")]
    public static void AssetCreateEffectData()
    {
        GameObject[] objects = Selection.gameObjects;
        for (int i = 0; i < objects.Length; i++)
        {
            EditorUtility.DisplayProgressBar("添加离线数据", "正在修改" + objects[i] + "......", i * 1.0f / objects.Length);
            CreateEffectData(objects[i]);
        }
        EditorUtility.ClearProgressBar();
    }

    public static void CreateEffectData(GameObject obj)
    {
        EffectOfflineData effectData = obj.GetComponent<EffectOfflineData>();
        if(effectData == null)
        {
            effectData = obj.AddComponent<EffectOfflineData>();
        }
        effectData.BindData();
        EditorUtility.SetDirty(obj);
        Debug.Log($"修改了{obj.name} 特效预制体");
        Resources.UnloadUnusedAssets();
        AssetDatabase.Refresh();
    }
    #endregion
}
