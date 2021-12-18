/****************************************************
    文件：GameStart.cs
    作者：TA94
    日期：2021/10/21 21:33:37
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using System.Resources;
using INFramework;
using INFramework.Framework.AssetBundles;
using INFramework.Framework.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = System.Object;
using ResourceManager = INFramework.Framework.AssetBundles.ResourceManager;

public class GameStart:MonoSingleton<GameStart>
{
    protected GameObject player;

    private void Awake()
    {
        AssetBundleManager.Instance.LoadAssetBundleConfig();
        ResourceManager.Instance.Init(this);
        ObjectManager.Instance.Init(transform.Find("RecyclePoolTrs"), transform.Find("SceneTrs"));
        UIManager.Instance.Init(transform.Find("UIRoot") as RectTransform, transform.Find("UIRoot/WndRoot") as RectTransform, transform.Find("UIRoot/UICamera").GetComponent<Camera>(),
    transform.Find("UIRoot/EventSystem").GetComponent<EventSystem>());
        GameMapManager.Instance.Init(this);

        ////加载场景
        //GameMapManager.Instance.LoadScene(GameConst.MEUN_SCENE);
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //RegisterUI();
        //GameObject obj = ObjectManager.Instance.InstantiateObject(GameConst.ATTACK, true);
        //ObjectManager.Instance.ReleaseObject(obj);
        //obj = null;

        //ObjectManager.Instance.PreLoadGameObject(GameConst.ATTACK, 5);

        //AudioClip clip = ResourceManager.Instance.LoadResource<AudioClip>(GameConst.MEUN_SOUND);
        //ResourceManager.Instance.ReleaseResouce(clip);

        //GameMapManager.Instance.LoadScene(GameConst.MEUN_SCENE);
    }

    /// <summary>
    /// 注册UI窗口
    /// </summary>
    public void RegisterUI()
    {
        //UIManager.Instance.Register<MenuView>(GameConst.MEUN_PANEL);
        //UIManager.Instance.Register<LoadingView>(GameConst.LOADING_PANEL);
    }

    /// <summary>
    /// 加载配置表
    /// </summary>
    public void LoadConfiger()
    {

    }

    void OnLoadFinish(string path, Object obj, object param1 = null, object param2 = null, object param3 = null)
    {
        player = obj as GameObject;
    }
    
    private void Update()
    {
        UIManager.Instance.OnUpdate();
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        ResourceManager.Instance.ClearCache();
        Resources.UnloadUnusedAssets();
        Debug.Log("清空编辑器缓存");
#endif
    }
}
