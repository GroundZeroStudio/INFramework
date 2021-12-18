using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFramework
{
	public static class MathUtil
	{
		/// <summary>
		/// 输入百分比返回是否命中概率
		/// </summary>
		/// <param name="percent"></param>
		/// <returns></returns>
		public static bool Percent(int percent)
        {
			return Random.Range(0, 100) <= percent;
        }
	}
}

