using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// AssetPathToGUID 在路径中获取资产的GUID。
/// GetAllAssetBundleNames 返回资产数据库中的所有AssetBundle名称
/// GetAssetBundleDependencies 给定一个assetBundleName，返回它依赖的AssetBundle列表
/// GetAssetPathsFromAssetBundleAndAssetName 获取标记有assetBundleName和命名为assetName的所有资产的资产路径。
/// GUIDToAssetPath 将GUID转换为其当前资产路径。
/// LoadAssetAtPath 返回给定路径assetPath处类型为type的第一个资产对象。
/// RemoveAssetBundleName 从资产数据库中删除assetBundle名称。forceRemove标志用于指示是否要删除它，即使它正在使用中。
/// </summary>
namespace INFramework
{
	public class AssetBundleTable : Singleton<AssetBundleTable>
	{
		private AssetBundleTable()
        {

        }

		public List<AssetBundleData> AssetBundleDatas = new List<AssetBundleData>();

		public void Load()
        {
#if UNITY_EDITOR
			var assetBundleNames = UnityEditor.AssetDatabase.GetAllAssetBundleNames();
            foreach (var assetBundleName in assetBundleNames)
            {
                //所标记assetBundleName的
                var assetBundleData = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
            }
#endif
        }
	}
}

