/****************************************************
    文件：UIManager.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.ModuleManagementExample
{

    public interface IUIManager : IModule
    {
        void DoSomething();
    }

    public class UIManager:IUIManager
    {

        private IResManager m_ResManager { get; set; }
        private IEventManager m_EventManager { get; set; }

        public void DoSomething()
        {
            Debug.Log("UIManager DoSomething");
            m_ResManager.DoSomething();
            m_EventManager.DoSomething();
        }

        public void InitModule()
        {
            m_ResManager = ModuleManagementConfig.Container.GetModule<IResManager>();
            m_EventManager = ModuleManagementConfig.Container.GetModule<IEventManager>();
        }
    }
}