using UnityEngine;

namespace INFrameworkDesign
{
    public class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {
        protected static T m_Instance;

        public static T Instance
        {
            get
            {
                if(m_Instance == null && !m_OnApplicationQuit)
                {
                    m_Instance = MonoSingletonCreator.CreateMonoSingleton<T>();
                }
                return m_Instance;
            }
        }

        public virtual void OnSingletonInit()
        {
            
        }

        protected static bool m_OnApplicationQuit = false;
        public static bool IsApplicationQuit => m_OnApplicationQuit;

        protected virtual void OnApplicationQuit()
        {
            m_OnApplicationQuit = true;
            if (m_Instance == null) return;
            Destroy(m_Instance.gameObject);
            m_Instance = null;
        }

        public virtual void Dispose()
        {
            Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            m_Instance = null;
        }
    }
}

