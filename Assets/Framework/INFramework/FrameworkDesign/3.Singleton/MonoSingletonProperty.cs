using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFrameworkDesign
{
	public static class MonoSingletonProperty<T> where T :MonoBehaviour, ISingleton
	{
		private static T m_Instance;
		public static T Instance
        {
            get
            {
                if(m_Instance == null)
                {
                    m_Instance = MonoSingletonCreator.CreateMonoSingleton<T>();
                }
                return m_Instance;
            }
        }

        public static void Dispose()
        {
            Object.Destroy(m_Instance.gameObject);
            m_Instance = null;
        }
	}
}

