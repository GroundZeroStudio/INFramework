/****************************************************
    文件：IArchitecture.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

namespace INFrameworkDesign.ServiceLocator.LayerdArchitectureExample 
{

    public interface IArchitecture
    {
        ILogicLayer LogicLayer { get; }
        IBusinessModuleLayer BusinessModuleLayer { get; }
        IPublicModuleLayer PublicModuleLayer { get; }
        IUtiltyLayer UtiltyLayer { get; }
        IBasicModuleLayer BasicModuleLayer { get; }
    }

}
