using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace INFramework
{
	public abstract class Singleton<T> where T: Singleton<T>
	{
		protected static T _instance = null;

		protected Singleton()
        {

        }

		public static T Instance
        {
            get
            {
                if(_instance == null)
                {
                    //非公有的实例构造函数
                    ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    //获取无参的构造函数
                    ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                    if(ctor == null)
                    {
                        throw new Exception("Non-public cotr() not found");
                    }
                    _instance = ctor.Invoke(null) as T;
                }
                return _instance;
            }
        }
	}
}

