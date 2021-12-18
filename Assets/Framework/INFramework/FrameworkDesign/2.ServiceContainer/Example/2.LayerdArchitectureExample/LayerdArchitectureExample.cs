/****************************************************
    文件：LayerdArchitectureExample.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.LayerdArchitectureExample
{
    public class LayerdArchitectureExample : MonoBehaviour
    {
        private ILoginController m_LoginController;
        private IUserInputManager m_UserInputManager;

        private void Start()
        {
            m_LoginController = ArchitectureConfig.Architecture.LogicLayer.GetModule<ILoginController>();
            m_UserInputManager = ArchitectureConfig.Architecture.LogicLayer.GetModule<IUserInputManager>();

            m_LoginController.Login();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                m_UserInputManager.OnInput(KeyCode.Space);
            }
        }

    }

}
