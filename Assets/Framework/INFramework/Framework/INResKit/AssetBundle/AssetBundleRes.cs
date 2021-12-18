using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFramework
{
    public class AssetBundleRes : Res
    {
        public const string PREFIX = "ab://";
        private static AssetBundleManifest m_Manifest;
        public static AssetBundleManifest Manifest
        {
            get
            {
                if(!m_Manifest)
                {
                    var bundleName = AssetBundleUtil.FullPathForAssetBundleName(AssetBundleUtil.GetPlatformName());
                    var bundle = AssetBundle.LoadFromFile(bundleName);
                    m_Manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                }
                return m_Manifest;
            }
        }

        ResLoader m_ResLoader = new ResLoader();
        public override void Load()
        {
            var assetBundleFileName = Name.Remove(0, PREFIX.Length);
            var dependBundleNames = Manifest.GetDirectDependencies(assetBundleFileName);
            foreach (var dependBundleName in dependBundleNames)
            {
                m_ResLoader.Load<AssetBundle>("ab://" + dependBundleName);
            }
            var path = AssetBundleUtil.FullPathForAssetBundleName(assetBundleFileName);
            Asset = AssetBundle.LoadFromFile(path);            //非压缩ab包
            State = EResState.Loaded;
            DispatchOnloadEvent(true);
        }

        private void LoadDependenyBundleAsync(Action onLoadDone)
        {
            var assetBundleFileName = Name.Remove(0, PREFIX.Length);
            var dependBundles = Manifest.GetDirectDependencies(assetBundleFileName);
            if (dependBundles.Length == 0)
            {
                onLoadDone();
            }

            var loadedCount = 0;
            foreach (var dependBundle in dependBundles)
            {
                m_ResLoader.LoadAsync<AssetBundle>("ab://" + dependBundle, (succeed, res) =>
                {
                    loadedCount++;
                    if (loadedCount == dependBundle.Length)
                    {
                        Debug.Log("11111111111111111111");
                        onLoadDone();
                    }
                });
            }
        }

        public override void LoadAsync()
        {
            State = EResState.Loading;
            LoadDependenyBundleAsync(() =>
            {

                var assetBundleFileName = Name.Remove(0, PREFIX.Length);
                var path = AssetBundleUtil.FullPathForAssetBundleName(assetBundleFileName);
                var request = AssetBundle.LoadFromFileAsync(path);
                request.completed += operation =>
                {
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@");
                    Asset = request.assetBundle;
                    Debug.Log(Asset);
                    State = EResState.Loaded;
                    DispatchOnloadEvent(true);
                };
            });
        }

        public override void Unload()
        {
            var assetBundle = Asset as AssetBundle;
            if(assetBundle != null)
            {
                assetBundle.Unload(true);
            }
            m_ResLoader.UnloadAllAssets();
            m_ResLoader = null;
        }
    }
}

