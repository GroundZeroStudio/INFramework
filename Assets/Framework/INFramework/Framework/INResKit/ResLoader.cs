using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace INFramework
{

	public class ResLoader
	{
		private ResTable m_LoadedReses = new ResTable();

		public T Load<T>(string rOwnerBundleName, string rAssetName) where T : Object
        {
			var res = LoadRes(new ResSearchKeys(rAssetName, typeof(T), rOwnerBundleName));
			if(res == null)
            {
				Debug.LogError("不存在资源类型：" + rAssetName + ":" + typeof(T) + ":" + rOwnerBundleName);
				return null;
            }
			return res.Asset as T;
        }

		public void LoadAsync<T>(string rOwnerBundleName, string rAssetName, Action<bool, Res> rOnLoad)
        {
			LoadResAsync(new ResSearchKeys(rAssetName, typeof(T), rOwnerBundleName), rOnLoad);
        }

		public T Load<T>(string rAssetPathOrName) where T : Object
        {
			Res res = LoadRes(new ResSearchKeys(rAssetPathOrName, typeof(T)));
            if (res == null)
            {
				Debug.LogError($"资源文件：{rAssetPathOrName}不存在");
				return null;
            }
			return res.Asset as T;
		}

        public void LoadAsync<T>(string rAddress, Action<bool, Res> rOnLoad)
        {
            LoadResAsync(new ResSearchKeys(rAddress, typeof(T)), rOnLoad);
        }

        Res GetResFromCache(ResSearchKeys rKeys)
        {
			//先判断当前脚本有没有加载过资源
			var res = m_LoadedReses.GetResWithSearchKeys(rKeys);

			if(res != null)
            {
				return res;
            }
			//判断共享资源池中是否加载过
			res = ResMgr.Instance.GetRes(rKeys);
			if(res != null)
            {
				Add2LoadedReses(res);
				return res;
            }

			return null;
		}

		void Add2LoadedReses(Res rRes)
        {
			rRes.Retain();
			m_LoadedReses.Add(rRes);
        }

		public Res LoadRes(ResSearchKeys rKeys)
        {
			Res res = GetResFromCache(rKeys);
			if(res != null)
            {
				if(res.State == EResState.Loaded)
                {
					return res;
                }
				//异步加载
				if(res.State == EResState.Loading)
                {
					res.StopLoadAsyncTask();
					res.Load();
					return res;
                }

				if(res.State == EResState.NotLoad)
                {
					throw new Exception(string.Format("{0}状态异常{1}", rKeys.Address, res.State));
                }
            }

			res = ResFactory.Create(rKeys);
			if (res == null) return null;
			ResMgr.Instance.AddRes(res);
			Add2LoadedReses(res);
			res.Load();
			return res;
        }

		public void LoadResAsync(ResSearchKeys rKeys, Action<bool, Res> rOnLoad)
        {
			var res = GetResFromCache(rKeys);
			if(res != null)
            {
				if(res.State == EResState.Loaded)
                {
					rOnLoad(true, res);
                }
				else if(res.State == EResState.Loading)
                {
					res.RegisterOnloadEventOnce(rOnLoad);
                }
                else
                {
					Debug.LogErrorFormat("{0}状态异常{1}", rKeys.Address, res.State);
					rOnLoad(false, null);
                }
				return;
            }
			//如果都未记录，则通过ResFactory.Create创建资源
            res = ResFactory.Create(rKeys);
            if (res == null)
            {
                rOnLoad(false, null);
                return;
            }
			//记录到资源共享池中
			ResMgr.Instance.AddRes(res);
			//添加到ResLoader
			Add2LoadedReses(res);
			//注册加载的事件
			res.RegisterOnloadEventOnce(rOnLoad);
			//加载资源
			res.LoadAsync();
        }

        public void UnloadAllAssets()
        {
            foreach (var resKeyValue in m_LoadedReses)
            {
				resKeyValue.Release();
            }
			m_LoadedReses.Clear();
        }
	}
}

