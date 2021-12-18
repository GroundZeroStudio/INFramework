/****************************************************
    文件：PoolManager.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.ModuleManagementExample
{
    public interface IPoolManager:IModule
    {
        void DoSomething();
    }

    public class PoolManager : IPoolManager
    {
        public void DoSomething()
        {
            Debug.Log("PoolManager DoSomething");
        }

        public void InitModule()
        {
        }
    }
}


