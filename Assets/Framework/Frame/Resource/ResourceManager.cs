/****************************************************
    文件：ResourceManager.cs
    作者：TA94
    日期：2021/10/20 23:16:59
    功能：底层资源加载

    //TODO： 1.根据手机内存使用情况释放资源池
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFramework.Framework.AssetBundles
{

    public enum ELoadResPriority
    {
        RES_HIGHT = 0,      //最高优先级
        RES_MIDDLE = 1,     //一般优先级
        RES_SLOW = 2,       //低优先级
        RES_NUM,
    }


    //实例化对象中间类
    public class ResouceObj
    {
        //文件路径对应的crc
        public uint mCrc = 0;
        //克隆的对象
        public GameObject mCloneObj = null;
        //跳转场景是否销毁
        public bool mClear = true;
        //资源guid
        public int mGuid = 0;
        //未实例化的资源
        public ResouceItem mResItem = null;
        //是否已经放回对象池
        public bool mAlready = false;
        //----------------------------
        //是否放在场景节点下
        public bool mSetSceneObj = false;
        //加载实例化对象后的回调函数
        public OnAsyncObjFinish mDealFinish = null;
        //加载实例化对象后的回调函数参数
        public object mParam1, mParam2, mParam3 = null;
        //实例化对象初始数据
        public OfflineData mOfflineData = null;
        
        public void Reset()
        {
            mCrc = 0;
            mCloneObj = null;
            mClear = true;
            mGuid = 0;
            mResItem = null;
            mAlready = false;
            mSetSceneObj = false;
            mDealFinish = null;
            mParam1 = mParam2 = mParam3 = null;
            mOfflineData = null;
        }
    }
    
    public class AsyncLoadResParam
    {
        public uint mCrc;
        public string mPath;
        public bool mIsSprite = false;
        public ELoadResPriority mPriority = ELoadResPriority.RES_SLOW;
        public List<AsyncCallBack> mCallBackList = new List<AsyncCallBack>();

        public void Reset()
        {
            mCrc = 0;
            mPath = "";
            mIsSprite = false;
            mPriority = ELoadResPriority.RES_SLOW;
            mCallBackList.Clear();
        }
    }

    public class AsyncCallBack
    {
        //加载完成的回调（针对ObjectManager）
        public OnAsyncFinish mDealFinish = null;
        //ObjectManager对应的中间类
        public ResouceObj mResObj = null;
        //-----------------------------------------------------
        //加载完成的回调
        public OnAsyncObjFinish mDealObjFinish = null;
        //回调参数
        public object mParam1 = null, mParam2 = null, mParam3 = null;

        public void Reset()
        {
            mDealObjFinish = null;
            mDealFinish = null;
            mParam1 = null;
            mParam2 = null;
            mParam3 = null;

            mResObj = null;
        }

    }

    public delegate void OnAsyncObjFinish(string path, Object obj, object param1 = null, object param2 = null, object param3 = null);

    public delegate void OnAsyncFinish(string path, ResouceObj obj, object param1 = null, object param2 = null, object param3 = null);

    public class ResourceManager : Singleton<ResourceManager>
    {
        public bool m_LoadFromAssetBundle = false;

        protected long _guid = 0;

        //缓存使用的资源列表, crc根据文件路径获取
        public Dictionary<uint, ResouceItem> AssetDic { get; set; } = new Dictionary<uint, ResouceItem>();
        //缓存引用计数为0的资源列表，达到缓存最大的时候释放这个列表里面最早没用的资源。该资源在内存中，防止再次使用又从磁盘加载
        protected CMapList<ResouceItem> mNoRefrenceAssetMapList = new CMapList<ResouceItem>();

        //中间类，回调类的类对象池
        protected ClassObjectPool<AsyncLoadResParam> mAsyncLoadResParamPool =
            ObjectManager.Instance.GetOrCreateClassPool<AsyncLoadResParam>(50);

        protected ClassObjectPool<AsyncCallBack> mAsyncCallBackPool =
            ObjectManager.Instance.GetOrCreateClassPool<AsyncCallBack>(100);
        
        //Mono脚本
        private MonoBehaviour mStartMono;
        //正在异步加载的资源列表
        protected List<AsyncLoadResParam>[] mLoadingAssetList = new List<AsyncLoadResParam>[(int)ELoadResPriority.RES_NUM];
        //正在异步加载的Dic
        protected Dictionary<uint, AsyncLoadResParam> mLoadingAssetDic = new Dictionary<uint, AsyncLoadResParam>();

        //最长连续卡着加载资源的时间，单位微秒
        private const long MAXLOADRESTIME = 200000;
        //资源最大缓存个数
        private const int MAXCACHECOUNT = 500;

        private ResourceManager()
        {

        }

        public void Init(MonoBehaviour mono)
        {
            for (int i = 0; i < (int)ELoadResPriority.RES_NUM; i++)
            {
                mLoadingAssetList[i] = new List<AsyncLoadResParam>();
            }
            
            mStartMono = mono;
            mStartMono.StartCoroutine(AsyncLoadCor());
        }

        public long CreateGuid()
        {
            return _guid++;
        }

        /// <summary>
        /// 取消异步加载
        /// </summary>
        /// <param name="rResObj"></param>
        public bool CancelLoad(ResouceObj rResObj)
        {
            AsyncLoadResParam para = null;
            if (mLoadingAssetDic.TryGetValue(rResObj.mCrc, out para) && mLoadingAssetList[(int)para.mPriority].Contains(para))
            {
                for (int i = para.mCallBackList.Count; i >= 0; i--)
                {
                    AsyncCallBack tempCallBack = para.mCallBackList[i];
                    //一个AsyncLoadResParam可能加载多个相同的资源，需要判断是否是想要取消的目标
                    if (tempCallBack != null && tempCallBack.mResObj == rResObj)
                    {
                        tempCallBack.Reset();
                        mAsyncCallBackPool.Recycle(tempCallBack);
                        para.mCallBackList.RemoveAt(i);
                    }
                }

                if (para.mCallBackList.Count <= 0)
                {
                    mLoadingAssetList[(int)para.mPriority].Remove(para);
                    mLoadingAssetDic.Remove(para.mCrc);
                    para.Reset();
                    mAsyncLoadResParamPool.Recycle(para);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 清空场景
        /// </summary>
        public void ClearCache()
        {
            List<ResouceItem> tempList = new List<ResouceItem>();
            foreach (ResouceItem item in AssetDic.Values)
            {
                if (item.mClear)
                {
                    tempList.Add(item);
                }
            }

            for (int i = 0; i < tempList.Count; i++)
            {
                DestoryResouceItem(tempList[i], true);
            }
            tempList.Clear();
        }

        /// <summary>
        /// 根据ResouceObj增加引用计数
        /// </summary>
        public int IncreaseResouceRef(ResouceObj rResObj, int nCount = 1)
        {
            return rResObj != null ? IncreaseResouceRef(rResObj.mCrc, nCount):0;
        }

        /// <summary>
        /// 根据路径增加引用计数
        /// </summary>
        public int IncreaseResouceRef(uint nCrc, int nCount = 1)
        {
            ResouceItem item = null;
            if (!AssetDic.TryGetValue(nCrc, out item) || item == null)
            {
                return 0;
            }

            item.RefCount += nCount;
            item.mLastUstTime = Time.realtimeSinceStartup;
            return item.RefCount;
        }
        
        /// <summary>
        /// 根据ResouceOb减少引用计数
        /// </summary>
        public int DecreaseResouceRef(ResouceObj rResObj, int nCount = 1)
        {
            return rResObj != null ? DecreaseResouceRef(rResObj.mCrc, nCount):0;
        }

        /// <summary>
        /// 根据路径减少引用计数
        /// </summary>
        public int DecreaseResouceRef(uint nCrc, int nCount = 1)
        {
            ResouceItem item = null;
            if (!AssetDic.TryGetValue(nCrc, out item) || item == null)
            {
                return 0;
            }

            item.RefCount -= nCount;
            //TODO 是否会小于0
            return item.RefCount;
        }
        
        /// <summary>
        /// 预加载资源
        /// </summary>
        public void PreloadRes(string rPath)
        {
            if (string.IsNullOrEmpty(rPath))
            {
                return;
            }

            uint crc = CRC32.GetCRC32(rPath);
            ResouceItem item = GetCacheResouceItem(crc, 0);
            if (item != null)
            {
                return;
            }

            Object obj = null;
#if UNITY_EDITOR
            if (!m_LoadFromAssetBundle)
            {
                item = AssetBundleManager.Instance.FindResouceItem(crc);
                if(item != null || item.mObj != null)
                {
                    obj = item.mObj as Object;
                }
                else
                {
                    if(item == null)
                    {
                        item = new ResouceItem();
                        item.mCrc = crc;
                    }
                    obj = LoadAssetByEditor<Object>(rPath);
                }
            }
#endif
            if (obj == null)
            {
                item = AssetBundleManager.Instance.LoadResouceAssetBundle(crc);
                if (item != null && item.mAssetBundle != null)
                {
                    if (item.mObj != null)
                    {
                        obj = item.mObj;
                    }
                    else
                    {
                        obj = item.mAssetBundle.LoadAsset<Object>(item.mAssetName);
                    }
                }
            }

            CacheResource(rPath, ref item, crc, obj);
            //跳场景不清空缓存
            item.mClear = false;
            ReleaseResouce(rPath, false);
        }

        /// <summary>
        /// 同步加载资源，针对给ObjectMangaer的接口
        /// </summary>
        /// <param name="path"></param>
        /// <param name="resObj"></param>
        /// <returns></returns>
        public ResouceObj LoadResource(string path, ResouceObj resObj)
        {
            if (resObj == null)
            {
                return null;
            }

            uint crc = resObj.mCrc == 0 ? CRC32.GetCRC32(path): resObj.mCrc;
            ResouceItem item = GetCacheResouceItem(crc);
            if (item != null)
            {
                resObj.mResItem = item;
                return resObj;
            }

            Object obj = null;
#if UNITY_EDITOR
            if (!m_LoadFromAssetBundle)
            {
                item = AssetBundleManager.Instance.FindResouceItem(crc);
                if (item != null && item.mObj != null)
                {
                    obj = item.mObj as Object;
                }
                else       
                {        
                    if(item == null)
                    {
                        item = new ResouceItem();
                        item.mCrc = crc;
                    }
                    obj = LoadAssetByEditor<Object>(path);
                }
            }
#endif
            if (obj == null)
            {
                item = AssetBundleManager.Instance.LoadResouceAssetBundle(crc);
                if (item != null && item.mAssetBundle != null)
                {
                    if (item.mObj != null)
                    {
                        obj = item.mObj as Object;
                    }
                    else
                    {
                        obj = item.mAssetBundle.LoadAsset<Object>(item.mAssetName);
                    }
                }
            }
            CacheResource(path, ref item, crc, obj);
            item.mClear = resObj.mClear;
            resObj.mResItem = item;
            return resObj;
        }
        
        /// <summary>
        /// 同步资源加载，外部直接调用，仅加载不需要实例化的资源，例如Texture，音频等等
        /// </summary>
        public T LoadResource<T>(string path) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            uint crc = CRC32.GetCRC32(path);
            ResouceItem item = GetCacheResouceItem(crc);
            if (item != null)
            {
                return item.mObj as T;
            }

            T obj = null;
#if UNITY_EDITOR
            if (!m_LoadFromAssetBundle)
            {
                item = AssetBundleManager.Instance.FindResouceItem(crc);
                if (item != null && item.mObj != null)
                {
                    obj = item.mObj as T;
                }
                else
                {
                    if (item == null)
                    {
                        item = new ResouceItem();
                        item.mCrc = crc;
                    }
                    obj = obj = LoadAssetByEditor<T>(path);
                }
            }
#endif
            if (obj == null)
            {
                item = AssetBundleManager.Instance.LoadResouceAssetBundle(crc);
                if (item != null && item.mAssetBundle != null)
                {
                    if (item.mObj != null)
                    {
                        obj = item.mObj as T;
                    }
                    else
                    {
                        obj = item.mAssetBundle.LoadAsset<T>(item.mAssetName);
                    }
                }
            }

            CacheResource(path, ref item, crc, obj);
            return obj;
        }

        /// <summary>
        /// 根据ResouceObj卸载资源
        /// </summary>
        /// <param name="rResObj">实例化对象中间类</param>
        /// <param name="bDestroyCache">是否从内存够中删除缓存</param>
        /// <returns></returns>
        public bool ReleaseResouce(ResouceObj rResObj, bool bDestroyCache = false)
        {
            if (rResObj == null)
            {
                return false;
            }

            ResouceItem item = null;
            if (!AssetDic.TryGetValue(rResObj.mCrc, out item) || item == null)
            {
                Debug.LogError($"AssetDic里不存在该资源{rResObj.mCloneObj.name}, 可能释放了多次");
            }
            GameObject.Destroy(rResObj.mCloneObj);
            item.RefCount--;
            DestoryResouceItem(item, bDestroyCache);
            return true;
        }

        /// <summary>
        /// 不需要实例化的资源卸载，根据对象
        /// </summary>
        public bool ReleaseResouce(Object rObj, bool bDestroyCache = false)
        {
            if (rObj == null)
            {
                return false; 
            }
            ResouceItem item = null;
            foreach (ResouceItem res in AssetDic.Values)
            {
                if (res.mGuid == rObj.GetInstanceID())
                {
                    item = res;
                }
            }

            if (item == null)
            {
                Debug.LogError($"AssetDic里不存在该资源{rObj.name}, 可能释放了多次");
                return false;
            }

            item.RefCount--;
            DestoryResouceItem(item, bDestroyCache);
            return true;
        }

        /// <summary>
        /// 不需要实例化的资源卸载，根据路径
        /// </summary>
        public bool ReleaseResouce(string rPath, bool bDestroyCache = false)
        {
            if (string.IsNullOrEmpty(rPath))
            {
                return false; 
            }

            uint crc = CRC32.GetCRC32(rPath);
            ResouceItem item = null;
            if(!AssetDic.TryGetValue(crc, out item) || item == null)
            {
                Debug.LogError($"AssetDic里不存在该资源,path = {rPath}, 可能释放了多次");
                return false;
            }
            item.RefCount--;
            DestoryResouceItem(item, bDestroyCache);
            return true;
        }
        
        /// <summary>
        /// 缓存加载的资源
        /// </summary>
        void CacheResource(string rPath, ref ResouceItem rItem, uint nCrc, Object rObj, int nAddRefCount = 1)
        {
            //缓存太多，清除最早没有使用的资源
            
            if (rItem == null)
            {
                Debug.LogError($"ResouceItem is null, path: {rPath}");
            }

            if (rObj == null)
            {
                Debug.LogError($"ResourceLoad Fail, path:{rPath} ");
            }

            rItem.mObj = rObj;
            rItem.mGuid = rObj.GetInstanceID();
            rItem.mLastUstTime = Time.realtimeSinceStartup;
            rItem.RefCount += nAddRefCount;
            ResouceItem oldItem = null;
            if (AssetDic.TryGetValue(nCrc, out oldItem))
            {
                AssetDic[rItem.mCrc] = rItem;
            }
            else
            {
                AssetDic.Add(rItem.mCrc, rItem);
            }
        }

        /// <summary>
        /// 清除缓存最早没有使用的资源
        /// </summary>
        protected void WashOut()
        {
            if(mNoRefrenceAssetMapList.Size() > MAXCACHECOUNT)
            {
                for (int i = 0; i < MAXCACHECOUNT / 2; i++)
                {
                    ResouceItem item = mNoRefrenceAssetMapList.Back();
                    DestoryResouceItem(item, true);
                }
            }

            //当前手机内存使用率清除最早没用的资源
            // {
            //     if(mNoRefrenceAssetMapList.size() <= 0)
            //         break;
            //     ResouceItem item = mNoRefrenceAssetMapList.Back();
            //     DestoryResouceItem(item, true);
            // }
        }

        /// <summary>
        /// 回收一个资源
        /// </summary>
        protected void DestoryResouceItem(ResouceItem rItem, bool bDestroyCache = false)
        {
            if(rItem == null || rItem.RefCount > 0) return;
            //是否缓存
            if (!bDestroyCache)
            {
                mNoRefrenceAssetMapList.InsertToHead(rItem);
                return;
            }
            
            if (!AssetDic.Remove(rItem.mCrc))
            {
                return;
            }

            mNoRefrenceAssetMapList.Reflesh(rItem);

            //释放assetbundle引用
            AssetBundleManager.Instance.ReleaseAsset(rItem);
            //清空资源对应的对象池
            ObjectManager.Instance.ClearPoolObject(rItem.mCrc);

            //释放资源对象
            if (rItem.mObj != null)
            {
                rItem.mObj = null;
#if UNITY_EDITOR
                Resources.UnloadAsset(rItem.mObj);
                Resources.UnloadUnusedAssets();
#endif
            }
        }

#if UNITY_EDITOR
        protected T LoadAssetByEditor<T>(string path) where T:UnityEngine.Object
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
        }
#endif

        /// <summary>
        /// 获取一个缓存资源
        /// </summary>
        ResouceItem GetCacheResouceItem(uint nCrc, int nAddrefcount = 1)
        {
            ResouceItem item = null;
            if (AssetDic.TryGetValue(nCrc, out item))
            {
                if (item != null)
                {
                    item.RefCount += nAddrefcount;
                    item.mLastUstTime = Time.realtimeSinceStartup;

                    // if (item.RefCount <= 1)
                    // {
                    //     mNoRefrenceAssetMapList.Remove(item);
                    // }
                    
                }
            }

            return item;
        }

        /// <summary>
        /// 异步加载资源（不需要实例化的资源，例如 贴图、音频）
        /// </summary>
        /// <param name="rPath">资源路径</param>
        /// <param name="rDealFinish">加载资源完成后的回调</param>
        /// <param name="nPriority">加载优先级</param>
        /// <param name="rParam1">参数1</param>
        /// <param name="rParam2">参数2</param>
        /// <param name="rParam3">参数3</param>
        /// <param name="nCrc">CRC</param>
        public void AsyncLoadResource(string rPath, OnAsyncObjFinish rDealFinish, ELoadResPriority nPriority, bool bIsSprite = false, object rParam1 = null, object rParam2 = null,
            object rParam3 = null, uint nCrc = 0)
        {
            if (nCrc == 0)
            {
                nCrc = CRC32.GetCRC32(rPath);
            }

            ResouceItem item = GetCacheResouceItem(nCrc);
            if (item != null)
            {
                if (rDealFinish != null)
                {
                    rDealFinish(rPath, item.mObj, rParam1, rParam2, rParam3);
                }
                return;
            }
            
            //判断是否在加载中
            AsyncLoadResParam param = null;
            if (!mLoadingAssetDic.TryGetValue(nCrc, out param) || param == null)
            {
                param = mAsyncLoadResParamPool.Spwan(true);
                param.mCrc = nCrc;
                param.mPath = rPath;
                param.mPriority = nPriority;
                param.mIsSprite = bIsSprite;
                mLoadingAssetDic.Add(nCrc, param);
                mLoadingAssetList[(int)nPriority].Add(param);
            }
            //往回调列表里加回调
            AsyncCallBack callBack = mAsyncCallBackPool.Spwan(true);
            callBack.mDealObjFinish = rDealFinish;
            callBack.mParam1 = rParam1;
            callBack.mParam2 = rParam2;
            callBack.mParam3 = rParam3;
            param.mCallBackList.Add(callBack);
        }
        
        /// <summary>
        /// 针对ObjectManager的异步加载接口
        /// </summary>
        /// <param name="rPath"></param>
        /// <param name="rResObj"></param>
        /// <param name="rDealFinish"></param>
        /// <param name="nPriority"></param>
        public void AsyncLoadResource(string rPath, ResouceObj rResObj, OnAsyncFinish rDealFinish, ELoadResPriority nPriority)
        {
            if (string.IsNullOrEmpty(rPath))
            {
                return;
            }

            ResouceItem item = GetCacheResouceItem(rResObj.mCrc);
            if(item != null)
            {
                rResObj.mResItem = item;
                if(rDealFinish != null)
                {
                    rDealFinish(rPath, rResObj);
                }
            }

            //判断是否在加载中
            AsyncLoadResParam param = null;
            if (!mLoadingAssetDic.TryGetValue(rResObj.mCrc, out param) || param == null)
            {
                param = mAsyncLoadResParamPool.Spwan(true);
                param.mCrc = rResObj.mCrc;
                param.mPath = rPath;
                param.mPriority = nPriority;
                mLoadingAssetDic.Add(rResObj.mCrc, param);
                mLoadingAssetList[(int)nPriority].Add(param);
            }
            //往回调列表里加回调
            AsyncCallBack callBack = mAsyncCallBackPool.Spwan(true);
            callBack.mDealFinish = rDealFinish;
            callBack.mResObj = rResObj;
            param.mCallBackList.Add(callBack);

        }

        /// <summary>
        /// 异步加载
        /// </summary>
        IEnumerator AsyncLoadCor()
        {
            List<AsyncCallBack> callBacks = null;
            //上一次yield的时间
            long lastYiledTime = System.DateTime.Now.Ticks;
            while (true)
            {
                bool bhaveYeild = false;
                for (int i = 0; i < (int)ELoadResPriority.RES_NUM ; i++)
                {
                    if (mLoadingAssetList[(int)ELoadResPriority.RES_HIGHT].Count > 0)
                    {
                        i = (int)ELoadResPriority.RES_HIGHT;
                    }
                    else if(mLoadingAssetList[(int)ELoadResPriority.RES_MIDDLE].Count > 0)
                    {
                        i = (int)ELoadResPriority.RES_MIDDLE;
                    }

                    List<AsyncLoadResParam> loadingList = mLoadingAssetList[i];
                    if(loadingList.Count <= 0) continue;
                    AsyncLoadResParam loadingItem = loadingList[0];
                    loadingList.RemoveAt(0);
                    callBacks = loadingItem.mCallBackList;

                    Object obj = null;
                    ResouceItem item = null;
#if UNITY_EDITOR
                    if (!m_LoadFromAssetBundle)
                    {
                        if (loadingItem.mIsSprite)
                        {
                            obj = LoadAssetByEditor<Sprite>(loadingItem.mPath);
                        }
                        else
                        {
                            obj = LoadAssetByEditor<Object>(loadingItem.mPath);
                        }

                        //模拟异步加载
                        yield return new WaitForSeconds(0.5f);
                        item = AssetBundleManager.Instance.FindResouceItem(loadingItem.mCrc);
                        if(item == null)
                        {
                            item = new ResouceItem();
                            item.mCrc = loadingItem.mCrc;
                        }
                    }
#endif

                    if (obj == null)
                    {
                        item = AssetBundleManager.Instance.LoadResouceAssetBundle(loadingItem.mCrc);
                        if (item != null && item.mAssetBundle != null)
                        {
                            AssetBundleRequest abRequest = null;
                            if (loadingItem.mIsSprite)
                            {
                                abRequest = item.mAssetBundle.LoadAssetAsync<Sprite>(item.mAssetName);
                            }
                            else
                            {
                                abRequest = item.mAssetBundle.LoadAssetAsync(item.mAssetName);
                            }
                            

                            yield return abRequest;
                            if (abRequest.isDone)
                            {
                                obj = abRequest.asset;
                            }
                            lastYiledTime = System.DateTime.Now.Ticks;
                        }
                    }
                    
                    CacheResource(loadingItem.mPath, ref item, loadingItem.mCrc, obj, callBacks.Count);
                    for (int j = 0; j < callBacks.Count; j++)
                    {
                        AsyncCallBack callBack = callBacks[j];
                        if(callBack != null && callBack.mDealFinish != null && callBack.mResObj != null)
                        {
                            ResouceObj tempObject = callBack.mResObj;
                            tempObject.mResItem = item;
                            callBack.mDealFinish(loadingItem.mPath, tempObject, tempObject.mParam1, tempObject.mParam2, tempObject.mParam3);
                            callBack.mDealFinish = null;
                            tempObject = null;
                        }

                        if (callBack != null && callBack.mDealObjFinish != null)
                        {
                            callBack.mDealObjFinish(loadingItem.mPath, obj, callBack.mParam1, callBack.mParam2,
                                callBack.mParam3);
                            callBack.mDealObjFinish = null;
                        }
                        callBack.Reset();
                        mAsyncCallBackPool.Recycle(callBack);
                    }

                    obj = null;
                    callBacks.Clear();
                    mLoadingAssetDic.Remove(loadingItem.mCrc);
                    
                    loadingItem.Reset();
                    mAsyncLoadResParamPool.Recycle(loadingItem);
                    if (System.DateTime.Now.Ticks - lastYiledTime > MAXLOADRESTIME)
                    {
                        yield return null;
                        lastYiledTime = System.DateTime.Now.Ticks;
                        bhaveYeild = true;
                    }
                }

                if (!bhaveYeild || System.DateTime.Now.Ticks - lastYiledTime > MAXLOADRESTIME)
                {
                    lastYiledTime = System.DateTime.Now.Ticks;
                    yield return null;
                }
            }
        }
    }




}

  