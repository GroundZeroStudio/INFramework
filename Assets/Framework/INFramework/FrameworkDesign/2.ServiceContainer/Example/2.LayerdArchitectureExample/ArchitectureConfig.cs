/****************************************************
    文件：ArchitectureConfig.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.LayerdArchitectureExample
{
    public class ArchitectureConfig : IArchitecture
    {
        public ILogicLayer LogicLayer { get; private set; }
        public IBusinessModuleLayer BusinessModuleLayer { get; private set; }
        public IPublicModuleLayer PublicModuleLayer { get; private set; }
        public IUtiltyLayer UtiltyLayer { get; private set; }
        public IBasicModuleLayer BasicModuleLayer { get; private set; }

        public static ArchitectureConfig Architecture = null;

        [RuntimeInitializeOnLoadMethod]
        static void Config()
        {
            Architecture = new ArchitectureConfig();
            //逻辑层配置
            Architecture.LogicLayer = new LogicLayer();

            //主动创建对象
            var logicController = Architecture.LogicLayer.GetModule<ILogicController>();
            var userInputManager = Architecture.LogicLayer.GetModule<IUserInputManager>();
            //业务模块配置
            Architecture.BusinessModuleLayer = new BusinessModuleLayer();

            var accountSystem = Architecture.BusinessModuleLayer.GetModule<IAccountSystem>();
            var missionSystem = Architecture.BusinessModuleLayer.GetModule<IMissionSystem>();
        }
    }
}


