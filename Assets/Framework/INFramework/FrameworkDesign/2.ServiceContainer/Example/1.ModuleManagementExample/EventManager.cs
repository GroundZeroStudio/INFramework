/****************************************************
    文件：EventManager.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.ModuleManagementExample
{
    public interface IEventManager : IModule
    {
        void DoSomething();
    }

    public class EventManager:IEventManager
    {
        private IPoolManager m_PoolManager { get; set; }

        public void DoSomething()
        {
            Debug.Log("EventManager DoSomething");
        }

        public void InitModule()
        {
            m_PoolManager = ModuleManagementConfig.Container.GetModule<IPoolManager>();
        }
    }
}


