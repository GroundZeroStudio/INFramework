/****************************************************
    文件：UserInputManager.cs
    作者：Olivia
    功能：Nothing
*****************************************************/


using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.LayerdArchitectureExample
{
    public interface IUserInputManager : ILogicController
    {
        void OnInput(KeyCode rKeyCode);
    }

    public class UserInputManager : IUserInputManager
    {
        public void OnInput(KeyCode rKeyCode)
        {
            Debug.Log("输入了：" + rKeyCode);
            var missionSystem = ArchitectureConfig.Architecture.BusinessModuleLayer.GetModule<IMissionSystem>();
            missionSystem.OnEvent("JUMP");
        }
    }

}
