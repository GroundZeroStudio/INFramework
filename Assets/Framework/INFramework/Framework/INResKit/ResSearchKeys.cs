using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFramework
{
	public class ResSearchKeys
	{
		/// <summary>
		/// 资源地址（前缀+资源路径）
		/// </summary>
		public string Address { get; private set; }
		/// <summary>
		/// 资源的类型
		/// </summary>
		public Type ResType { get; private set; } 

		/// <summary>
		/// AB资源需要填充这个值
		/// </summary>
		public string OwnerBundleName { get; private set; }

		public ResSearchKeys(string rAddress, Type rResType = null, string rOwnerBundleName = null)
        {
			Address = rAddress;
			ResType = rResType ?? typeof(UnityEngine.Object);
			OwnerBundleName = rOwnerBundleName;
		}
	}
}

