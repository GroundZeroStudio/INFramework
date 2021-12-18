/****************************************************
    文件：LoginController.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.LayerdArchitectureExample 
{
    public interface ILoginController : ILogicController
    {
        void Login();
    }

    public class LoginController : ILoginController
    {
        public void Login()
        {
            var accountSystem = ArchitectureConfig.Architecture.BusinessModuleLayer.GetModule<IAccountSystem>();
            accountSystem.Login("hello", "abc", (success) =>
            {
                if (success)
                {
                    Debug.Log("登录成功");
                }
            });
         
        }
    }

}

