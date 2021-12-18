using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFramework
{
	public class AssetData : MonoBehaviour
	{
		/// <summary>
		/// 资源名
		/// </summary>
		public string Name;
		/// <summary>
		/// 资源所在的Bundle
		/// </summary>
		public string OwnerBundleName;
		/// <summary>
		/// 资源类型
		/// </summary>
		public Type AssetType;
	}
}

