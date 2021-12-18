/****************************************************
    文件：FSM.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.ModuleManagementExample
{
    public interface IFSM : IModule
    {
        void DoSomething();
    }

    public class FSM:IFSM
    {
        public void DoSomething()
        {
            Debug.Log("FSM DoSomething");
        }

        public void InitModule()
        {

        }
    }

}

