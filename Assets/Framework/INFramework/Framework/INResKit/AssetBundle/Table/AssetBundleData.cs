using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFramework
{
	public class AssetBundleData
	{
		/// <summary>
		/// Bundle名字
		/// </summary>
		public string Name;
		/// <summary>
		/// 包含的Bundle
		/// </summary>

		public List<AssetData> AssetDataList = new List<AssetData>();
		/// <summary>
		/// 依赖的Bundle名字
		/// </summary>
		public string[] DependencyBundleNames;
	}
}

