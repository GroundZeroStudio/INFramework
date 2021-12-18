using System;
using System.Collections.Generic;

//以类型为id
//注册、注销消息
//发送消息

namespace INFrameworkDesign
{
	interface IRegisteration
    {

    }

	class Registerations<T> : IRegisteration 
	{
		/// <summary>
		/// 不需要List<Action<T>>
		/// 委托本来就可以一对多
		/// </summary>
		public Action<T> OnReceives = obj => { };
	}

	public class TypeEventSystem
	{
		static Dictionary<Type, IRegisteration> m_TypeEventDict = new Dictionary<Type, IRegisteration>();


		/// <summary>
		/// 注册事件
		/// </summary>
		public static void Register<T>(Action<T> rOnReceive)
        {
			var type = typeof(T);
			IRegisteration registerations = null;
			if(m_TypeEventDict.TryGetValue(type, out registerations))
			{
				var reg = registerations as Registerations<T>;
				reg.OnReceives += rOnReceive;
            }
            else
            {
				Registerations<T> reg = new Registerations<T>();
				reg.OnReceives += rOnReceive;
				m_TypeEventDict.Add(type, reg);
            }
        }

		/// <summary>
		/// 注销事件
		/// </summary>
		public static void UnRegister<T>(Action<T> rOnReceive)
        {
            var type = typeof(T);
            IRegisteration registerations = null;
            if (m_TypeEventDict.TryGetValue(type, out registerations))
            {
                var reg = registerations as Registerations<T>;
                reg.OnReceives -= rOnReceive;
            }
        }

		/// <summary>
		/// 发送事件
		/// </summary>
		public static void Send<T>(T t)
        {
			var type = typeof(T);
			IRegisteration registerstions = null;
			if(m_TypeEventDict.TryGetValue(type, out registerstions))
            {
				var reg = registerstions as Registerations<T>;
				reg.OnReceives(t);
            }
        }
	}
}

