
namespace INFrameworkDesign
{
    public abstract class Singleton<T> : ISingleton where T : Singleton<T>
    {
        protected static T m_Instance;
        static object m_Lock = new object();
        public static T Instance
        {
            get
            {
                lock (m_Lock)
                {
                    if(m_Instance == null)
                    {
                        m_Instance = SingletonCreator.CreateSingleton<T>();
                    }
                }
                return m_Instance;
            }
        }

        public virtual void OnSingletonInit()
        {
           
        }

        public virtual void Dispose()
        {
            m_Instance = null;
        }
    }
}

