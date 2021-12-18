using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace INFramework
{
	public class AssetBundleTableExample : MonoBehaviour
	{
        private void Start()
        {
            //var assetBundleTable = AssetBundleTable.Instance;
            //var assetBundleNames = UnityEditor.AssetDatabase.GetAllAssetBundleNames();
            //foreach (var assetBundleName in assetBundleNames)
            //{
            //    // 获取已经被打上了给assetBundle名资产的路径。
            //    var assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
            //    var assetBundleData = new AssetBundleData
            //    {
            //        Name = assetBundleName,
            //        DependencyBundleNames = UnityEditor.AssetDatabase.GetAssetBundleDependencies(assetBundleName, false)
            //    };
            //    foreach (var assetPath in assetPaths)
            //    {
            //        var assetData = new AssetData()
            //        {
            //            OwnerBundleName = assetBundleName,
            //            Name = assetPath.Split('/').Last().Split('.').First(),
            //            AssetType = UnityEditor.AssetDatabase.GetMainAssetTypeAtPath(assetPath)
            //        };
            //        assetBundleData.AssetDataList.Add(assetData);
            //    }
            //    assetBundleTable.AssetBundleDatas.Add(assetBundleData);
            //}

            //assetBundleTable.AssetBundleDatas.ForEach(assetBundleData =>
            //{
            //    assetBundleData.AssetDataList.ForEach(assetData =>
            //    {
            //        Debug.LogFormat("AB:{0}     AssetData:{1}    Type:{2}", assetBundleData.Name, assetData.Name, assetData.AssetType);
            //    });

            //    foreach (var dependencyBundleName in assetBundleData.DependencyBundleNames)
            //    {
            //        Debug.LogFormat("AB:{0}     Depend:{1}", assetBundleData.Name, dependencyBundleName);
            //    }
            //});
        }
    }
}

