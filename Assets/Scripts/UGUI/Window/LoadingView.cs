/****************************************************
    文件：LoadingView.cs
    作者：TA94
    日期：2021/10/30 16:38:35
    功能：Nothing
*****************************************************/
using INFramework;
using INFramework.Framework.UI;
using UnityEngine;

public class LoadingView : Window
{
    protected LoadingPanel mPanel;
    private string mSceneName;

    public override void Awake(params object[] rParams)
    {
        mPanel = GameObject.GetComponent<LoadingPanel>();
        mSceneName = (string)rParams[0];
    }

    public override void OnUpdate()
    {
        if(mPanel == null)
        {
            return;
        }

        float nProgress = GameMapManager.Instance.LoadingProgress / 100.0f;
        mPanel.SliderProgress.value = nProgress;
        mPanel.TxtProgress.text = $"{GameMapManager.Instance.LoadingProgress}%";
        if(GameMapManager.Instance.LoadingProgress >= 100)
        {
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        if (mSceneName == GameConst.MEUN_SCENE)
        {
            UIManager.Instance.PopupWnd(GameConst.MEUN_PANEL);
        }

        UIManager.Instance.CloseWnd(GameConst.LOADING_PANEL);
    }
}
