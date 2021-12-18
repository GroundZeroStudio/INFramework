using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFramework
{
	public abstract class MonoSingleton<T> : MonoBehaviour where T:MonoSingleton<T>
	{
		protected static T _instance = null;

		public static T Instance
		{
            get
            {
				if(_instance == null)
                {
					_instance = FindObjectOfType<T>();
					if(FindObjectsOfType<T>().Length > 1)
                    {
						Debug.LogErrorFormat("MonoSingleton More than 1 , {0}", typeof(T).Name);
						return _instance;
                    }
                }

				if(_instance == null)
                {
					string instanceName = typeof(T).Name;
					Debug.Log("Instance Name: " + instanceName);
					GameObject instanceGo = GameObject.Find(instanceName);
					if(instanceGo == null)
                    {
						instanceGo = new GameObject(instanceName);
                    }
					_instance = instanceGo.AddComponent<T>();
					DontDestroyOnLoad(_instance);
					Debug.Log("Add New Singleton " + _instance.name + " In Game!");
                }
                else
                {
					Debug.Log("Already exist: " + _instance.name);
                }
				return _instance;
            }
		}

		protected virtual void OnDestroy()
        {
			_instance = null;
        }
	}
}

