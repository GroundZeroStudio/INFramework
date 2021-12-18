/****************************************************
    文件：ObjectManager.cs
    作者：TA94
    日期：2021/10/20 23:17:14
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace INFramework.Framework.AssetBundles
{
    public class ObjectManager : Singleton<ObjectManager>
    {
        /// <summary>
        /// 回收节点
        /// </summary>
        public Transform RecyclePoolTrs;

        public Transform SceneTrs;

        //对象池
        protected Dictionary<uint, List<ResouceObj>> mObjectPoolDict = new Dictionary<uint, List<ResouceObj>>();
        //ResouceObj缓存池
        protected Dictionary<int, ResouceObj> mResouceObjDict = new Dictionary<int, ResouceObj>();
        //ResouceObj的类对象池
        protected ClassObjectPool<ResouceObj> mResouceObjClassPool;
        //根据异步的guid储存ResourceObj，来判断是否正在异步加载
        protected Dictionary<long, ResouceObj> mAsyncResObjs = new Dictionary<long, ResouceObj>();

        private ObjectManager()
        {

        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(Transform rRecyclePoolTrs, Transform rSceneTrs)
        {
            mResouceObjClassPool = GetOrCreateClassPool<ResouceObj>(1000);
            RecyclePoolTrs = rRecyclePoolTrs;
            SceneTrs = rSceneTrs;
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void ClearCache()
        {
            List<uint> tempList = new List<uint>();
            foreach (var key in mObjectPoolDict.Keys)
            {
                List<ResouceObj> st = mObjectPoolDict[key];
                for (int i = st.Count - 1; i >= 0; i--)
                {
                    ResouceObj resObj = st[i];
                    if(!System.Object.ReferenceEquals(resObj.mCloneObj, null) && resObj.mClear)
                    {
                        //清除ResouceObj缓存池
                        mResouceObjDict.Remove(resObj.mCloneObj.GetInstanceID());
                        GameObject.Destroy(resObj.mCloneObj);
                        resObj.Reset();
                        mResouceObjClassPool.Recycle(resObj);
                        st.Remove(resObj);
                    }
                }

                if(st.Count <= 0)
                {
                    tempList.Add(key);
                }
            }

            //清除ResouceObj资源池
            for (int i = 0; i < tempList.Count; i++)
            {
                uint temp = tempList[i];
                if (mObjectPoolDict.ContainsKey(temp))
                {
                    mObjectPoolDict.Remove(temp);
                }
            }
            tempList.Clear();
        }

        /// <summary>
        /// 清除某个资源在对象池中所有的对象
        /// </summary>
        /// <param name="nCrc"></param>
        public void ClearPoolObject(uint nCrc)
        {
            List<ResouceObj> st = null;
            if(!mObjectPoolDict.TryGetValue(nCrc, out st) || st == null)
            {
                return;
            }

            for (int i = st.Count - 1; i >= 0; i--)
            {
                ResouceObj resObj = st[i];
                if (resObj.mClear)
                {
                    int tempGuid = resObj.mCloneObj.GetInstanceID();
                    GameObject.Destroy(resObj.mCloneObj);
                    mResouceObjDict.Remove(tempGuid);
                    mResouceObjClassPool.Recycle(resObj);
                    st.Remove(resObj);
                }
            }

            if(st.Count <= 0)
            {
                mObjectPoolDict.Remove(nCrc);
            }

        }

        /// <summary>
        /// 取消异步加载
        /// </summary>
        /// <param name="nGuid"></param>
        public void CancelLoad(long nGuid)
        {
            ResouceObj resObj = null;
            if(mAsyncResObjs.TryGetValue(nGuid, out resObj) &&ResourceManager.Instance.CancelLoad(resObj))
            {
                mAsyncResObjs.Remove(nGuid);
                resObj.Reset();
                mResouceObjClassPool.Recycle(resObj);
            }
        }

        /// <summary>
        /// 是否正在异步加载
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool IsingAsyncLoad(long nGuid)
        {
            return mAsyncResObjs[nGuid] != null;
        }

        /// <summary>
        /// 该对象是否是对象池创建的
        /// </summary>
        public bool IsObjectManagerCreate(GameObject rObj)
        {
            ResouceObj resObj = mResouceObjDict[rObj.GetInstanceID()];
            return resObj == null ? false :true;
        }

        /// <summary>
        /// 预加载
        /// </summary>
        /// <param name="rPath">资源路径</param>
        /// <param name="nCount">预加载个数</param>
        /// <param name="bClear">跳转场景是否清除</param>
        public void PreLoadGameObject(string rPath, int nCount, bool bClear = false)
        {
            List<GameObject> tempGameObjectList = new List<GameObject>(nCount);
            for (int i = 0; i < nCount; i++)
            {
                GameObject obj = InstantiateObject(rPath, false, bClear);
                tempGameObjectList.Add(obj);
            }

            for (int i = 0; i < nCount; i++)
            {
                GameObject obj = tempGameObjectList[i];
                ReleaseObject(obj);
                obj = null;
            }

            tempGameObjectList.Clear();
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="rPath"></param>
        /// <param name="bClear"></param>
        /// <returns></returns>
        public GameObject InstantiateObject(string rPath,bool bSetSceneObj = false, bool bClear = true)
        {
            uint crc = CRC32.GetCRC32(rPath);
            ResouceObj resouceObj = GetObjectFromPool(crc);
            if (resouceObj == null)
            {
                resouceObj = mResouceObjClassPool.Spwan(true);
                resouceObj.mCrc = crc;
                resouceObj.mClear = bClear;
                //ResourceManager提供加载方法
                resouceObj = ResourceManager.Instance.LoadResource(rPath, resouceObj);
                if ( resouceObj.mResItem.mObj != null)
                {
                    resouceObj.mCloneObj = GameObject.Instantiate(resouceObj.mResItem.mObj) as GameObject;
                    resouceObj.mOfflineData = resouceObj.mCloneObj.GetComponent<OfflineData>();
                }
            }

            if (bSetSceneObj)
            {
                resouceObj.mCloneObj.transform.SetParent(SceneTrs, false);
            }

            int tempGuid = resouceObj.mCloneObj.GetInstanceID();
            if (!mResouceObjDict.ContainsKey(tempGuid))
            {
                mResouceObjDict.Add(tempGuid, resouceObj);
            }
            return resouceObj.mCloneObj;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="rPath">资源路径</param>
        /// <param name="rDealFinish">加载完成回调函数</param>
        /// <param name="nPriority">加载优先级</param>
        /// <param name="bSetSceneObj">是否设置在场景节点</param>
        /// <param name="rParam1">回调参数1</param>
        /// <param name="rParam2">回调参数2</param>
        /// <param name="rParam3">回调参数3</param>
        /// <param name="bClear">跳转场景是否清除</param>
        public long InstantiateObjectAsync(string rPath, OnAsyncObjFinish rDealFinish, ELoadResPriority nPriority, bool bSetSceneObj = true, object
            rParam1 = null, object rParam2 = null, object rParam3 = null, bool bClear = true)
        {
            if(string.IsNullOrEmpty(rPath))
            {
                return 0;
            }
            uint crc = CRC32.GetCRC32(rPath);
            //从缓存池中获取
            ResouceObj resObj = GetObjectFromPool(crc);
            if(resObj != null)
            {
                if (bSetSceneObj)
                {
                    if(resObj.mCloneObj != null)
                    {
                        resObj.mCloneObj.transform.SetParent(SceneTrs);
                    }
                    if (rDealFinish != null)
                    {
                        rDealFinish(rPath, resObj.mCloneObj, rParam1, rParam2, rParam3);
                    }
                }
                return resObj.mGuid;
            }
            else
            {
                long guid = ResourceManager.Instance.CreateGuid();
                resObj = mResouceObjClassPool.Spwan(true);
                resObj.mCrc = crc;
                resObj.mSetSceneObj = bSetSceneObj;
                resObj.mClear = bClear;
                resObj.mDealFinish = rDealFinish;
                resObj.mParam1 = rParam1;
                resObj.mParam2 = rParam2;
                resObj.mParam3 = rParam3;
                //TODO 从ResourceManager中加载资源
                ResourceManager.Instance.AsyncLoadResource(rPath, resObj, OnLoadResouceObjFinish, nPriority);
                return guid;
            }
            

        }

        /// <summary>
        /// 资源加载完成回调
        /// </summary>
        /// <param name="rPath">路径</param>
        /// <param name="rResObj">中间类</param>
        /// <param name="rParam1">参数1</param>
        /// <param name="rParam2">参数2</param>
        /// <param name="rParam3">参数3</param>
        void OnLoadResouceObjFinish(string rPath, ResouceObj rResObj, object rParam1, object rParam2, object rParam3)
        {
            if(rResObj == null)
            {
                return;
            }

            if(rResObj.mResItem.mObj == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"异步资源加载的资源为空,路径为：{rPath}");
#endif
            }
            else
            {
                rResObj.mCloneObj = GameObject.Instantiate(rResObj.mResItem.mObj) as GameObject;
                rResObj.mOfflineData = rResObj.mCloneObj.GetComponent<OfflineData>();
            }

            //如果加载完成就移除
            if (mAsyncResObjs.ContainsKey(rResObj.mGuid))
            {
                mAsyncResObjs.Remove(rResObj.mGuid);

            }

            if(rResObj.mCloneObj != null && rResObj.mSetSceneObj)
            {
                rResObj.mCloneObj.transform.SetParent(SceneTrs, false);
            }

            if(rResObj.mDealFinish != null)
            {
                int tempGuid = rResObj.mCloneObj.GetInstanceID();
                if (!mResouceObjDict.ContainsKey(tempGuid))
                {
                    mResouceObjDict.Add(tempGuid, rResObj);
                }
                rResObj.mDealFinish(rPath, rResObj.mCloneObj, rParam1, rParam2, rParam3);
            }
        }

        //从对象池中获取对象
        public ResouceObj GetObjectFromPool(uint nCrc)
        {
            List<ResouceObj> objList = null;
            if (mObjectPoolDict.TryGetValue(nCrc, out objList) && objList != null && objList.Count > 0)
            {
                ResouceObj resObj = objList[0];
                ResourceManager.Instance.IncreaseResouceRef(resObj);
                objList.RemoveAt(0);
                GameObject obj = resObj.mCloneObj;
                if (!System.Object.ReferenceEquals(obj, null))
                {
                    resObj.mOfflineData.ResetProp();
                    resObj.mAlready = false;
#if UNITY_EDITOR
                    if (obj.name.EndsWith("(Recycle)"))
                    {
                        obj.name = obj.name.Replace("(Recycle)", "");
                    }
#endif        
                }
                return resObj;
            }
            return null;
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="rObj"></param>
        /// <param name="nMaxCacheCount"></param>
        /// <param name="bDestroyCache">是否清除缓存</param>
        /// <param name="bRecycleParent"></param>
        public void ReleaseObject(GameObject rObj, int nMaxCacheCount = -1, bool bDestroyCache = false,
            bool bRecycleParent = true)
        {
            if (rObj == null)
            {
                return;
            }

            ResouceObj resObj = null;
            int tempGuid = rObj.GetInstanceID();
            if (!mResouceObjDict.TryGetValue(tempGuid, out resObj))
            {
                Debug.Log($"{rObj.name}这个对象不是ObjectManager创建的！");
                return;
            }

            if (resObj == null)
            {
                Debug.LogError("缓存的ResouceObj为空");
                return;
            }

            if (resObj.mAlready)
            {
                Debug.LogError($"{resObj.mCloneObj.name}该对象已经放回对象池了，请检查是否清空引用");
                return;
            }
#if UNITY_EDITOR
            rObj.name += "(Recycle)";
#endif

            List<ResouceObj> st = null;
            //不缓存
            if (nMaxCacheCount == 0)
            {
                //从对象池中移除
                mResouceObjDict.Remove(tempGuid);
                //释放对象，删除实例化对象，删除内存中的对象
                ResourceManager.Instance.ReleaseResouce(resObj, bDestroyCache);
                //回收对象中间类
                resObj.Reset();
                mResouceObjClassPool.Recycle(resObj);
            }
            else
            {
                //回收到对象池
                //先判断池子里有没有
                if (!mObjectPoolDict.TryGetValue(resObj.mCrc, out st) || st == null)
                {
                    st = new List<ResouceObj>();
                    mObjectPoolDict.Add(resObj.mCrc, st);
                }
                //判断个数
                if (resObj.mCloneObj != null)
                {
                    if (bRecycleParent)
                    {
                        resObj.mCloneObj.transform.SetParent(RecyclePoolTrs);
                    }
                    else
                    {
                        resObj.mCloneObj.SetActive(false);
                    }

                    if (nMaxCacheCount < 0 || st.Count < nMaxCacheCount)
                    {
                        st.Add(resObj);
                        resObj.mAlready = true;
                        ResourceManager.Instance.DecreaseResouceRef(resObj);
                    }
                    else
                    {
                        mResouceObjDict.Remove(tempGuid);
                        ResourceManager.Instance.ReleaseResouce(resObj, bDestroyCache);
                        resObj.Reset();
                        mResouceObjClassPool.Recycle(resObj);
                    }
                }
            }

    }
        
    
        public OfflineData FindOfflineData(GameObject obj)
        {
            OfflineData data = null;
            ResouceObj resObj = null;
            if(mResouceObjDict.TryGetValue(obj.GetInstanceID(), out resObj) && resObj != null)
            {
                data = resObj.mOfflineData;
            }
            return data;
        }

        #region 类对象池
        private Dictionary<Type, object> mClassPoolRecordDict = new Dictionary<Type, object>();
        /// <summary>
        /// 创建类对象池，创建完成以后外面可以保存classObjectPool<T>, 然后调用spwan和Recycle来创建了回收类对象
        /// </summary>
        /// <param name="nMaxCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ClassObjectPool<T> GetOrCreateClassPool<T>(int nMaxCount) where T : class, new()
        {
            Type type = typeof(T);
            object outObj = null;
            {
                if (!mClassPoolRecordDict.TryGetValue(type, out outObj) || outObj == null)
                {
                    ClassObjectPool<T> newPool =  new ClassObjectPool<T>(nMaxCount);
                    mClassPoolRecordDict.Add(type, newPool);
                    return newPool;
                }
            }
            return outObj as ClassObjectPool<T>;
        } 

        #endregion

    
    }
}

