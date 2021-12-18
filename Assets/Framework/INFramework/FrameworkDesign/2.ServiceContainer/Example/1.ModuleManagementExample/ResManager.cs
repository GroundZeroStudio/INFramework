/****************************************************
    文件：ResManager.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.ModuleManagementExample
{
    public  interface IResManager:IModule
    {
        void DoSomething();
    }

    public class ResManager:IResManager
    {
        public IPoolManager PoolManager { get; set; }

        public void DoSomething()
        {
            Debug.Log("ResManager DoSomething");
        }

        public void InitModule()
        {
        }
    }
}

