/****************************************************
    文件：StartWindow.cs
    作者：TA94
    日期：2021/10/28 23:42:19
    功能：Nothing
*****************************************************/
using INFramework;
using INFramework.Framework.AssetBundles;
using INFramework.Framework.UI;
using UnityEngine;

public class MenuView : Window
{
    protected MenuPanel mView;
    //private AudioClip mClip;

    public override void Awake(params object[] rParams)
    {
        base.Awake(rParams);
        mView = GameObject.GetComponent<MenuPanel>();
        AddButtonClickListener(mView.StartButton, OnBtnClickedStart);
        AddButtonClickListener(mView.SettingButton, OnBtnClickedSetting);
        AddButtonClickListener(mView.ExitButton, OnBtnClickedExit);
        //mClip = ResourceManager.Instanace.LoadResource<AudioClip>(GameConst.MEUN_SOUND);
        //mView.Audio.clip = mClip;
        //mView.Audio.Play();
        GameObject obj = ObjectManager.Instance.InstantiateObject(GameConst.ATTACK, true);
        ObjectManager.Instance.ReleaseObject(obj, 0, true);
        ResourceManager.Instance.AsyncLoadResource("Assets/GameData/Textures/Image2.jpg", OnLoadImageFinish2, ELoadResPriority.RES_MIDDLE, true);
        ResourceManager.Instance.AsyncLoadResource("Assets/GameData/Textures/Image1.jpg", OnLoadImageFinish1, ELoadResPriority.RES_SLOW, true);
        ResourceManager.Instance.AsyncLoadResource("Assets/GameData/Textures/Image3.gif", OnLoadImageFinish3, ELoadResPriority.RES_MIDDLE, true);
    }

    public void OnLoadImageFinish1(string rPath, Object obj, object param1 = null, object param2 = null, object param3 = null)
    {
        if(obj != null)
        {
            Sprite sp = obj as Sprite;
            mView.ImgTest1.sprite = sp;
            Debug.Log("图片1加载完成");
        }
    }

    public void OnLoadImageFinish2(string rPath, Object obj, object param1 = null, object param2 = null, object param3 = null)
    {
        if (obj != null)
        {
            Sprite sp = obj as Sprite;
            mView.ImgTest2.sprite = sp;
            Debug.Log("图片2加载完成");
        }
    }

    public void OnLoadImageFinish3(string rPath, Object obj, object param1 = null, object param2 = null, object param3 = null)
    {
        if (obj != null)
        {
            Sprite sp = obj as Sprite;
            mView.ImgTest3.sprite = sp;
            Debug.Log("图片3加载完成");
        }
    }

    public override void OnUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    ResourceManager.Instanace.ReleaseResouce(mClip, true);
        //    mView.Audio.clip = null;
        //    mClip = null;
        //}
    }

    protected void OnBtnClickedStart()
    {
        Debug.Log("开始游戏");
    }

    protected void OnBtnClickedSetting()
    {
        Debug.Log("设置游戏");
    }

    protected void OnBtnClickedExit()
    {
        Debug.Log("退出");
    }
}

