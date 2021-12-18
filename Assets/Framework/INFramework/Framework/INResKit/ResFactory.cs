using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFramework
{
    /// <summary>
    /// 根据不同的地址返回不同的 Res 对象
    /// 绑定自定义 Res 的 RegisterCustomRes 
    /// </summary>
	public class ResFactory
	{
		private static Func<ResSearchKeys, Res> m_ResCreator = s => null;


		public static Res Create(ResSearchKeys rKeys)
        {
            if (rKeys.Address.StartsWith(ResourcesRes.PREFIX))
            {
                return new ResourcesRes()
                {
                    Name = rKeys.Address,
                    ResType = rKeys.ResType,
                };
            }

            if (rKeys.Address.StartsWith(AssetBundleRes.PREFIX))
            {
                return new AssetBundleRes()
                {
                    Name = rKeys.Address,
                    ResType = rKeys.ResType
                };
            }

            if (!string.IsNullOrEmpty(rKeys.OwnerBundleName))
            {
                return new AssetRes()
                {
                    Name = rKeys.Address,
                    ResType = rKeys.ResType,
                    OwnerBundleName = rKeys.OwnerBundleName
                };
            }

            return m_ResCreator.Invoke(rKeys);
        }

        /// <summary>
        /// 注册自定义的资源的创建功能
        /// </summary>
        /// <param name="rResCreator"></param>
		public static void RegisterCustomRes(Func<ResSearchKeys, Res> rResCreator)
        {
			m_ResCreator = rResCreator;
        }
	}
}

