/****************************************************
    文件：UIManager.cs
    作者：TA94
    日期：2021/10/28 21:22:24
    功能：Nothing
*****************************************************/
using INFramework.Framework.AssetBundles;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace INFramework.Framework.UI
{
    public enum EUIMsgID
    {
        NONE = 0,
    }
    public class UIManager : Singleton<UIManager>
    {
        //UI父节点
        private RectTransform mUIRoot;
        //窗口节点
        private RectTransform mWndRoot;
        //UI射线机
        private Camera mUICamera;
        //EventSystem节点
        private EventSystem mEventSystem;
        //屏幕的宽高比
        private float mCanvasRate = 0;

        private string m_UIPrefabPath = "Assets/GameData/Prefabs/UGUI/Panel/";
        /// <summary>
        /// 所有打开的窗口
        /// </summary>
        private Dictionary<string, Window> mWindowDic = new Dictionary<string, Window>();
        /// <summary>
        /// 注册窗口
        /// </summary>
        private Dictionary<string, System.Type> mRegisterWindows = new Dictionary<string, System.Type>();
        /// <summary>
        /// 打开窗口列表
        /// </summary>
        private List<Window> mWindowList = new List<Window>();

        private UIManager()
        {

        }

        public void Init(RectTransform UIRoot, RectTransform wdnRoot, Camera UICamera, EventSystem eventSystem)
        {
            mUIRoot = UIRoot;
            mWndRoot = wdnRoot;
            mUICamera = UICamera;
            mEventSystem = eventSystem;
            mCanvasRate = Screen.height / (mUICamera.orthographicSize * 2);
        }

        /// <summary>
        /// 设置UI资源路径
        /// </summary>
        /// <param name="rPath"></param>
        public void SetUIPrefabPath(string rPath)
        {
            m_UIPrefabPath = rPath;
        }

        public void Register<T>(string rWndName) where T:Window
        {
            mRegisterWindows[rWndName] = typeof(T);
        }


        /// <summary>
        /// 关闭所有窗口
        /// </summary>
        public void CloseAllWnd()
        {
            for (int i = mWindowList.Count - 1; i >= 0; i--)
            {
                CloseWnd(mWindowList[i]);
            }
        }

        /// <summary>
        /// 打开唯一指定窗口
        /// </summary>
        /// <param name="rWndName"></param>
        /// <param name="bIsTop"></param>
        /// <param name="rParams"></param>
        public void SwitchStateWithName(string rWndName, bool bIsTop, params object[] rParams)
        {
            CloseAllWnd();
            PopupWnd(rWndName, bIsTop, rParams);
        }

        /// <summary>
        /// 根据窗口名字隐藏
        /// </summary>
        /// <param name="rWndName"></param>
        public void HideWnd(string rWndName)
        {
            Window window = FindWindWithName<Window>(rWndName);
            HideWnd(window);
        }

        /// <summary>
        /// 根据窗口对象隐藏
        /// </summary>
        /// <param name="rWnd"></param>
        public void HideWnd(Window rWnd)
        {
            if (rWnd != null)
            {
                rWnd.OnDispose();
                if (rWnd.GameObject != null && rWnd.GameObject.activeSelf)
                {
                    rWnd.GameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 查找窗口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rWndName"></param>
        /// <returns></returns>
        public T FindWindWithName<T>(string rWndName) where T : Window
        {
            Window wnd = null;
            if (mWindowDic.TryGetValue(rWndName, out wnd))
            {
                return wnd as T;
            }
            return null;
        }

        /// <summary>
        /// 根据窗口名关闭窗口
        /// </summary>
        /// <param name="rWndName"></param>
        /// <param name="bDestroy"></param>
        public void CloseWnd(string rWndName, bool bDestroy = false)
        {
            Window wnd = FindWindWithName<Window>(rWndName);
            CloseWnd(wnd, bDestroy);
        }

        /// <summary>
        /// 根据窗口对象关闭窗口
        /// </summary>
        /// <param name="rWnd"></param>
        /// <param name="bDestroy"></param>
        public void CloseWnd(Window rWnd, bool bDestroy = false)
        {
            if (rWnd != null)
            {
                rWnd.OnDispose();
                rWnd.OnClose();
                if (mWindowDic.ContainsKey(rWnd.Name))
                {
                    mWindowDic.Remove(rWnd.Name);
                    mWindowList.Remove(rWnd);
                }

                if (bDestroy)
                {
                    ObjectManager.Instance.ReleaseObject(rWnd.GameObject, 0, true);
                }
                else
                {
                    ObjectManager.Instance.ReleaseObject(rWnd.GameObject, bRecycleParent: false);
                }
                rWnd.GameObject = null;
                rWnd = null;
            }
        }

        /// <summary>
        /// 打开一个窗口
        /// </summary>
        /// <param name="rWndName"></param>
        /// <param name="bIsTop"></param>
        /// <param name="rParams"></param>
        /// <returns></returns>
        public Window PopupWnd(string rWndName, bool bIsTop = true, params object[] rParams)
        {
            Window window = FindWindWithName<Window>(rWndName);
            if (window == null)
            {
                if (mRegisterWindows.TryGetValue(rWndName, out Type wndType))
                {
                    window = System.Activator.CreateInstance(wndType) as Window;
                }
                else
                {
                    Debug.LogError("找不到窗口对应的脚本， 窗口名是：" + rWndName);
                    return null;
                }

                GameObject wndObj = ObjectManager.Instance.InstantiateObject(m_UIPrefabPath + rWndName, false, false);
                if (wndObj == null)
                {
                    Debug.LogError("创建UI预制体失败，窗口名是：" + rWndName);
                    return null;
                }

                //检查是否已经打开过窗口
                if (!mWindowDic.ContainsKey(rWndName))
                {
                    mWindowDic.Add(rWndName, window);
                    mWindowList.Add(window);
                }

                window.GameObject = wndObj;
                window.Transform = wndObj.transform;
                window.Name = rWndName;
                window.Awake(rParams);
                wndObj.transform.SetParent(mWndRoot, false);

                if (bIsTop)
                {
                    wndObj.transform.SetAsLastSibling();
                }
                window.OnShow(rParams);
            }
            else
            {
                OnShow(rWndName, bIsTop, rParams);
            }
            return window;
        }

        /// <summary>
        /// 根据名字显示窗口
        /// </summary>
        /// <param name="rWndName"></param>
        /// <param name="bIsTop"></param>
        /// <param name="rParams"></param>
        protected void OnShow(string rWndName, bool bIsTop, params object[] rParams)
        {
            Window window = FindWindWithName<Window>(rWndName);
            OnShow(window, bIsTop, rParams);
        }

        /// <summary>
        /// 根据窗口类型显示窗口
        /// </summary>
        /// <param name="rWndow"></param>
        /// <param name="bIsTop"></param>
        /// <param name="rParams"></param>
        protected void OnShow(Window rWndow, bool bIsTop, params object[] rParams)
        {
            if (rWndow != null)
            {
                if (rWndow.GameObject != null && !rWndow.GameObject.activeSelf)
                {
                    rWndow.GameObject.SetActive(true);
                }
                if (bIsTop)
                {
                    rWndow.Transform.SetAsLastSibling();
                }
                rWndow.OnShow(rParams);
            }
        }

        /// <summary>
        /// 隐藏或者显示UI根节点
        /// </summary>
        /// <param name="bShow"></param>
        public void ShowOrHideUI(bool bShow)
        {
            if (mUIRoot.gameObject.activeSelf != bShow)
            {
                mUIRoot.gameObject.SetActive(bShow);
            }
        }

        /// <summary>
        /// 设置默认选择对象
        /// </summary>
        /// <param name="obj"></param>
        public void SetNormalSelectObj(GameObject obj)
        {
            if (mEventSystem == null)
            {
                mEventSystem = EventSystem.current;
            }
            mEventSystem.firstSelectedGameObject = obj;
        }

        /// <summary>
        /// 发送消息给窗口
        /// </summary>
        /// <param name="rWndName"></param>
        /// <param name="nMsgID"></param>
        /// <param name="rParams"></param>
        /// <returns></returns>
        public bool OnSendMessage(string rWndName, EUIMsgID nMsgID = 0, params object[] rParams)
        {
            Window wnd = FindWindWithName<Window>(rWndName);
            if(wnd != null)
            {
                return wnd.OnMessage(nMsgID, rParams);
            }
            return false;
        }

        public void OnUpdate()
        {
            for (int i = 0; i < mWindowList.Count; i++)
            {
                if (mWindowList[i] != null)
                {
                    mWindowList[i].OnUpdate();
                }
            }
        }
    }
}


