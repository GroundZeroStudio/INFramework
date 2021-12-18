using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFramework
{
	public class TypeEventSystem : MonoBehaviour
	{
		/// <summary>
		/// 接口， 只负责存储在字典
		/// </summary>
		interface IRegisterations
        {

        }

		/// <summary>
		/// 多个注册
		/// </summary>
		/// <typeparam name="T"></typeparam>
		class Registerations<T> : IRegisterations
        {
			public Action<T> OnReceives = obj => { };
        }

		private static Dictionary<Type, IRegisterations> mTypeEventDict = new Dictionary<Type, IRegisterations>();

		/// <summary>
		/// 注册
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="onRecevie"></param>
		public static void Register<T>(System.Action<T> onRecevie)
        {
			Type type = typeof(T);
			IRegisterations registers = null;
            if(mTypeEventDict.TryGetValue(type, out registers))
            {
				var reg = registers as Registerations<T>;
				reg.OnReceives += onRecevie;
            }
            else
            {
				var reg = new Registerations<T>();
				reg.OnReceives += onRecevie;
				mTypeEventDict.Add(type, reg);
            }

        }

		/// <summary>
		/// 注销
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="onReceive"></param>
		public static void UnRegister<T>(System.Action<T> onReceive)
        {
			Type type = typeof(T);
			IRegisterations registers = null;
			if(mTypeEventDict.TryGetValue(type, out registers))
            {
				var reg = registers as Registerations<T>;
				reg.OnReceives -= onReceive;
				if(reg.OnReceives == null)
                {
					mTypeEventDict.Remove(type);
                }
            }
            else
            {
				Debug.LogWarningFormat("事件{0不存在", onReceive.ToString());
            }
        }

		/// <summary>
		/// 发送事件
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		public static void Send<T>(T t)
        {
			var type = typeof(T);
			IRegisterations registers = null;
			if(mTypeEventDict.TryGetValue(type, out registers))
            {
				var reg = registers as Registerations<T>;
				reg.OnReceives(t);
            }
        }
	}
}

