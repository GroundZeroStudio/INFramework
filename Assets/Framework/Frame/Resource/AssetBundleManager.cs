/****************************************************
    文件：AssetBundleManager.cs
    作者：TA94
    日期：2021/10/20 23:16:12
    功能：1.读取AssetBundle配置表
         2.设置中间类AssetBundleItem进行引用计数
         3.根据路径加载AssetBundle
         4.根据路径卸载AssetBundle
         5.为ResourceManager提供加载中间类，根据AssetBundleItem中间类释放资源，查找中间类
*****************************************************/
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace INFramework.Framework.AssetBundles
{
    public class AssetBundleManager : Singleton<AssetBundleManager>
    {
        protected string m_ABConfigABName = "assetbundleconfig";
        /// <summary>
        /// 资源关系依赖配置,根据AssetBundleConfig二进制配置文件
        /// </summary>
        protected Dictionary<uint, ResouceItem> mResouceItemDic = new Dictionary<uint, ResouceItem>();
        protected Dictionary<uint, AssetBundleItem> mAssetBundleItemDic = new Dictionary<uint, AssetBundleItem>();
        protected ClassObjectPool<AssetBundleItem> mAssetBundleItemPool =
            ObjectManager.Instance.GetOrCreateClassPool<AssetBundleItem>(500);

        protected string ABLoadPath
        {
            get
            {
                return Application.streamingAssetsPath + "/";
            }
        }

        private AssetBundleManager()
        {

        }

        /// <summary>
        /// 读取AssetBundle配置表
        /// </summary>
        /// <returns></returns>
        public bool LoadAssetBundleConfig()
        {
            if (!ResourceManager.Instance.m_LoadFromAssetBundle)
                return false;
            //AssetBundle配置表文件
            string abConfigPath = ABLoadPath + m_ABConfigABName;
            AssetBundle configAB = AssetBundle.LoadFromFile(abConfigPath);
            TextAsset textAsset = configAB.LoadAsset<TextAsset>(m_ABConfigABName);
            if (textAsset == null)
            {
                Debug.LogError("assetbundleconfig is no exist");
                return false;
            }
            else
            {
                using (MemoryStream ms = new MemoryStream(textAsset.bytes))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    AssetBundleConfig assetBundleConfig = (AssetBundleConfig)bf.Deserialize(ms);
                    for (int i = 0; i < assetBundleConfig.ABList.Count; i++)
                    {
                        ABBase rABBase = assetBundleConfig.ABList[i];
                        ResouceItem rItem = new ResouceItem();
                        rItem.mCrc = rABBase.Crc;
                        rItem.mAssetName = rABBase.AssetName;
                        rItem.mABName = rABBase.ABName;
                        rItem.mDepenceAssetBundle = rABBase.ABDependce;
                        if (mResouceItemDic.ContainsKey(rItem.mCrc))
                        {
                            Debug.LogError($"存在重复的资源CRC{rItem.mCrc}, 资源名为{rItem.mAssetName}");
                        }
                        else
                        {
                            mResouceItemDic.Add(rItem.mCrc, rItem);
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 加载资源中间类ResouceItem
        /// </summary>
        /// <param name="nCrc">资源路径crc</param>
        /// <returns>ResouceItem资源相关信息</returns>
        public ResouceItem LoadResouceAssetBundle(uint nCrc)
        {
            ResouceItem resouceItem = null;
            if (!mResouceItemDic.TryGetValue(nCrc, out resouceItem) || resouceItem == null)
            {
                Debug.LogError($"LoadResouceAssetBundle error: can not find crc : {nCrc}  in assetbundleConfig");
                return resouceItem;
            }

            if (resouceItem.mAssetBundle != null)
            {
                return resouceItem;
            }

            resouceItem.mAssetBundle = LoadAssetBundle(resouceItem.mABName);
            if (resouceItem.mDepenceAssetBundle != null)
            {
                for (int i = 0; i < resouceItem.mDepenceAssetBundle.Count; i++)
                {
                    LoadAssetBundle(resouceItem.mDepenceAssetBundle[i]);
                }
            }
            return resouceItem;
        }

        /// <summary>
        /// 加载单个AssetBundle包
        /// </summary>
        /// <param name="rABName"></param>
        /// <returns>ab包路径</returns>
        private AssetBundle LoadAssetBundle(string rABName)
        {
            AssetBundleItem abItem = null;
            uint nCrc = CRC32.GetCRC32(rABName);
            if (!mAssetBundleItemDic.TryGetValue(nCrc, out abItem))
            {
                AssetBundle assetBundle = null;
                string fullPath = Application.streamingAssetsPath + "/" + rABName;
                assetBundle = AssetBundle.LoadFromFile(fullPath);
                if (assetBundle == null)
                {
                    Debug.LogError($"Load AssetBundle Error: {fullPath}");
                }
                abItem = mAssetBundleItemPool.Spwan(true);
                abItem.AssetBundle = assetBundle;
                abItem.RefCount++;
                mAssetBundleItemDic.Add(nCrc, abItem);
            }
            else
            {
                abItem.RefCount++;
            }

            return abItem.AssetBundle;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="rItem"></param>
        public void ReleaseAsset(ResouceItem rItem)
        {
            if (rItem == null)
            {
                return;
            }

            if (rItem.mDepenceAssetBundle != null && rItem.mDepenceAssetBundle.Count > 0)
            {
                for (int i = 0; i < rItem.mDepenceAssetBundle.Count; i++)
                {
                    UnLoadAssetBundle(rItem.mDepenceAssetBundle[i]);
                }
            }
            UnLoadAssetBundle(rItem.mABName);
        }
        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="rABName"></param>
        private void UnLoadAssetBundle(string rABName)
        {
            uint nCrc = CRC32.GetCRC32(rABName);
            AssetBundleItem rItem = null;
            if (mAssetBundleItemDic.TryGetValue(nCrc, out rItem) && rItem != null)
            {
                rItem.RefCount--;
                if (rItem.RefCount <= 0 && rItem.AssetBundle != null)
                {
                    rItem.AssetBundle.Unload(true);
                    rItem.Reset();
                    mAssetBundleItemPool.Recycle(rItem);
                    mAssetBundleItemDic.Remove(nCrc);
                }
            }
        }
        public ResouceItem FindResouceItem(uint nCrc)
        {
            ResouceItem item = null;
            mResouceItemDic.TryGetValue(nCrc, out item);
            return item;
        }
    }

public class AssetBundleItem
{
    //加载好的AB包
    public AssetBundle AssetBundle;
    //引用个数
    public int RefCount;

    public void Reset()
    {
        AssetBundle = null;
        RefCount = 0;
    }
}

public class ResouceItem
{
//-----------------AB资源相关--------------------------
    //资源Crc
    public uint mCrc;
    //资源名字
    public string mAssetName;
    //资源所在ab包
    public string mABName;
    //资源所依赖的资源包
    public List<string> mDepenceAssetBundle;
    //资源所在加载完成的ab包
    public AssetBundle mAssetBundle;
//-----------------资源对象--------------------------
    //直接从ab包中加载的资源对象
    public Object mObj = null;
    //资源唯一标识
    public int mGuid = 0;
    //资源最后所使用的时间
    public float mLastUstTime = 0.0f;
    //引用计数
    protected int mRefCount = 0;
    //跳场景是否清掉
    public bool mClear = true;
    public int RefCount
    {
        get => mRefCount;
        set
        {
            mRefCount = value;
            if (mRefCount < 0)
            {
                Debug.LogError("mRefcount < 0" + mRefCount + "," + (mObj != null ? mObj.name : "name is null"));
            }
        }
    }
}  
}

