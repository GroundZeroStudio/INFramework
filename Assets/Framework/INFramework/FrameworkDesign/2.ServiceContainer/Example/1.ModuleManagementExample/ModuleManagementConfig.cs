/****************************************************
    文件：ModuleManagementConfig.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using INFrameworkDesign.ServiceLocator.Default;
using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.ModuleManagementExample
{
    public class ModuleManagementConfig : MonoBehaviour
    {
        public static ModuleContainer Container = null;

        private void Awake()
        {
            var baseType = typeof(IModule);
            var cache = new DefaultModuleCache();
            var factory = new AssemblyModuleFactory(baseType.Assembly, baseType);

            Container = new ModuleContainer(cache, factory);
            //主动去创建对象
            var poolManager = Container.GetModule<IPoolManager>();
            var fsm = Container.GetModule<IFSM>();
            var resManger = Container.GetModule<IResManager>();
            var eventManager = Container.GetModule<IEventManager>();
            var uiManager = Container.GetModule<IUIManager>();

            //初始化模块
            var modules = Container.GetAllModules<IModule>();
            foreach (var module in modules)
            {
                module.InitModule();
            }
        }

        private void Start()
        {
            var uiManager = Container.GetModule<IUIManager>();
            uiManager.DoSomething();
        }
    }
}


