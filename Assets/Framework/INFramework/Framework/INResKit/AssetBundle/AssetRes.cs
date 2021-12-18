using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFramework
{
    public class AssetRes : Res
    {
        /// <summary>
        /// 所属的 AssetBundle 名字
        /// </summary>
        public string OwnerBundleName { get; set; }

        /// <summary>
        /// 用于获取 加载对应 AssetBundle 的 ResLoader
        /// </summary>
        private ResLoader mResLoader = new ResLoader();

        public override void Load()
        {
            //加载资源所在的ab资源包
            var bundle = mResLoader.Load<AssetBundle>(AssetBundleRes.PREFIX + OwnerBundleName);

            Asset = bundle.LoadAsset(Name, ResType);

            State = EResState.Loaded;

            DispatchOnloadEvent(true);
        }

        public override void LoadAsync()
        {
            State = EResState.Loading;

            mResLoader.LoadAsync<AssetBundle>(AssetBundleRes.PREFIX + OwnerBundleName, (succeed, res) =>
            {
                var bundle = res.Asset as AssetBundle;
                var assetRequest = bundle.LoadAssetAsync(Name, ResType);
                assetRequest.completed += operation =>
                {
                    Asset = assetRequest.asset;

                    State = EResState.Loaded;

                    DispatchOnloadEvent(true);
                };
            });
        }

        public override void Unload()
        {
            // prefab 资源不能用 Resources.UnloadAsset 清除
            if (Asset is GameObject)
            {

            }
            else
            {
                Resources.UnloadAsset(Asset);
            }

            Asset = null;

            mResLoader.UnloadAllAssets();
            mResLoader = null;
        }

        public override bool MatchResSearchKeysWithoutName(ResSearchKeys rResSearchKeys)
        {
            return rResSearchKeys.OwnerBundleName == OwnerBundleName && rResSearchKeys.ResType == ResType;
        }
    }
}

