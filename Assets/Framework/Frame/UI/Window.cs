/****************************************************
    文件：Window.cs
    作者：TA94
    日期：2021/10/28 21:22:33
    功能：窗口类，持有UI预制体的相关引用
*****************************************************/
using INFramework.Framework.AssetBundles;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace INFramework.Framework.UI 
{
    public class Window
    {
        public GameObject GameObject { get; set; }

        public Transform Transform { get; set; }
        public string Name { get; set; }

        protected List<Button> mButtons { get; set; } = new List<Button>();

        protected List<Toggle> mToggles { get; set; } = new List<Toggle>();

        public virtual bool OnMessage(EUIMsgID nMsgID, params object[] rParams)
        {
            return true;
        }
        public virtual void Awake(params object[] rParams) { }
        public virtual void OnShow(params object[] rParams) { }
        public virtual void OnUpdate() { }
        public virtual void OnDispose() { }
        public virtual void OnClose() 
        {
            RemoveAllButtonListener();
            RemoveAllToggleListener();
            mButtons.Clear();
            mToggles.Clear();
        }

        public bool SetReplaceImage(string rPath, Image rImage, bool bSetNativeSize = false)
        {
            if(rImage == null)
            {
                return false;
            }

            Sprite sp = ResourceManager.Instance.LoadResource<Sprite>(rPath);
            if(sp != null)
            {
                if(rImage.sprite != null)
                    rImage.sprite = null;
                rImage.sprite = sp;
                if (bSetNativeSize)
                {
                    rImage.SetNativeSize();
                }
                return true;
            }

            return false;
        }

        public void SetReplaceImageAsync(string rPath, Image rImage, bool bSetNativeSize)
        {
            if(rImage == null)
            {
                return;
            }

            ResourceManager.Instance.AsyncLoadResource(rPath, OnLoadSpriteFinish, ELoadResPriority.RES_MIDDLE, true, rImage, bSetNativeSize);
        }

        void OnLoadSpriteFinish(string rPath, Object rObj, object rParam1 = null, object rParam2 = null, object rParam3 = null)
        {
            if(rObj == null)
            {
                Sprite sp = rObj as Sprite;
                Image image = rParam1 as Image;
                bool setNativeSize = (bool)rParam2;
                if(sp != null)
                {
                    if (image != null) image.sprite = null;
                    image.sprite = sp;
                    if (setNativeSize)
                    {
                        image.SetNativeSize();
                    }
                }
            }
        }

        public void RemoveAllButtonListener()
        {
            foreach (var button in mButtons)
            {
                button.onClick.RemoveAllListeners();
            }
        }

        public void RemoveAllToggleListener()
        {
            foreach (var toggle in mToggles)
            {
                toggle.onValueChanged.RemoveAllListeners();
            }
        }

        public void AddButtonClickListener(Button rButton, UnityAction rAction)
        {
            if(rButton != null)
            {
                if(!mButtons.Contains(rButton))
                {
                    mButtons.Add(rButton);
                }
                rButton.onClick.RemoveAllListeners();
                rButton.onClick.AddListener(rAction);
                rButton.onClick.AddListener(ButtonPlaySound);
            }
        }

        protected void ButtonPlaySound()
        {

        }

        public void AddToggleClickListener(Toggle rToggle, UnityAction<bool> rAction)
        {
            if(rToggle != null)
            {
                if (!mToggles.Contains(rToggle))
                {
                    mToggles.Add(rToggle);
                }
                rToggle.onValueChanged.RemoveAllListeners();
                rToggle.onValueChanged.AddListener(rAction);
                rToggle.onValueChanged.AddListener(TogglePlaySound);
            }
        }

        protected void TogglePlaySound(bool bIson)
        {

        }

    }
}



