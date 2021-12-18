/********************************************************************
	author:		Olivia
	reated:	    2021/12/07 20:40
	function:   资源对象基类
	purpose:	
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace INFramework
{
    public enum EResState
    {
        //未加载
        NotLoad = 0,
        //加载中
        Loading = 1,
        //加载完成
        Loaded = 2,
    }

	public abstract class Res : SimpleRC
	{
        public EResState State { get; protected set; }

        /// <summary>
        /// 资源名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 加载的资源
        /// </summary>
        public Object Asset { get; set; }

        public Type ResType { get; set; }

        #region 向外提供异步加载完成的事件
        protected event Action<bool, Res> m_Onload = null;

        public void RegisterOnloadEventOnce(Action<bool, Res> rOnload)
        {
            m_Onload += rOnload;
        }

        protected void DispatchOnloadEvent(bool bSucceed)
        {
            if(m_Onload != null)
            {
                Debug.Log(Asset);
                m_Onload.Invoke(bSucceed, this);
                m_Onload = null;
            }
        }
        #endregion

        #region 中断异步任务
        protected Coroutine m_LoadAsyncTask = null;
        public void StopLoadAsyncTask()
        {
            if(m_LoadAsyncTask != null)
            {
                CoroutineRunner.Instance.StopCoroutine(m_LoadAsyncTask);
                m_LoadAsyncTask = null;
            }
        }
        #endregion

        public abstract void Load();

        public abstract void LoadAsync();

        public abstract void Unload();

        protected override void OnZeroRef()
        {
            Unload();
            ResMgr.Instance.RemoveRes(Name);
        }

        public virtual bool MatchResSearchKeysWithoutName(ResSearchKeys rResSearchKeys)
        {
            return rResSearchKeys.ResType == ResType;
        }
    }
}

