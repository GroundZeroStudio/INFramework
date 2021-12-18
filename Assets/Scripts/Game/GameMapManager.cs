/****************************************************
    文件：GameMapManager.cs
    作者：TA94
    日期：2021/10/30 15:57:51
    功能：Nothing
*****************************************************/
using INFramework.Framework.AssetBundles;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace INFramework.Framework.UI 
{
    public class GameMapManager : Singleton<GameMapManager>
    {
        //场景开始加载回调
        public Action LoadSceneStartCallBack;
        //场景加载完成回到
        public Action LoadSceneEndCallBack;
        //当前场景名
        public string CurrentMapName { get; set; }
        //场景加载进度
        public int LoadingProgress = 0;
        private MonoBehaviour mMono;
        private bool mAlreadyLoadScene = false;
        public bool AlreadyLoadScene => mAlreadyLoadScene;

        private GameMapManager()
        {

        }

        public void Init(MonoBehaviour rMono)
        {
            mMono = rMono;
        }

        /// <summary>
        /// 设置场景参数
        /// </summary>
        /// <param name="rName"></param>
        public void SetSceneSetting(string rName)
        {
           
        }

        public void LoadScene(string rName)
        {
            LoadingProgress = 0;
            mMono.StartCoroutine(LoadSceneAsync(rName));
            UIManager.Instance.PopupWnd(GameConst.LOADING_PANEL, true, rName);
        }

        IEnumerator LoadSceneAsync(string rName)
        {
            if(LoadSceneStartCallBack != null)
            {
                LoadSceneStartCallBack();
            }
            mAlreadyLoadScene = false;
            ClearCache();
            AsyncOperation unLoadScene = SceneManager.LoadSceneAsync(GameConst.EMPTY_SCENE, LoadSceneMode.Single);//, LoadSceneMode.Single
            while(unLoadScene != null && !unLoadScene.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
            LoadingProgress = 0;
            int targetProgress = 0;
            AsyncOperation asyncScene = SceneManager.LoadSceneAsync(rName);
            if(asyncScene != null && !asyncScene.isDone)
            {
                asyncScene.allowSceneActivation = false;
                while (asyncScene.progress < 0.9f)
                {
                    targetProgress = (int)asyncScene.progress * 100;
                    yield return new WaitForEndOfFrame();
                    //平滑过渡
                    while(LoadingProgress < targetProgress)
                    {
                        ++LoadingProgress;
                        yield return new WaitForEndOfFrame();
                    }
                }
                CurrentMapName = rName;
                SetSceneSetting(rName);
                //自行加载剩余的10%
                targetProgress = 100;
                while(LoadingProgress < targetProgress - 2)
                {
                    ++LoadingProgress;
                    yield return new WaitForEndOfFrame();
                }
                LoadingProgress = 100;
                asyncScene.allowSceneActivation = true;
                mAlreadyLoadScene = false;
                if(LoadSceneEndCallBack != null)
                {
                    LoadSceneEndCallBack();
                }
            }
        }

        /// <summary>
        /// 清除场景资源
        /// </summary>
        private void ClearCache()
        {
            ObjectManager.Instance.ClearCache();
            ResourceManager.Instance.ClearCache();
        }
    }
}


