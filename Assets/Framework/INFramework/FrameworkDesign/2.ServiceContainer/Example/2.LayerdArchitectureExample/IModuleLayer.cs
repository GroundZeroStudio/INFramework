/****************************************************
    文件：IModuleLayer.cs
    作者：Olivia
    功能：Nothing
*****************************************************/
namespace INFrameworkDesign.ServiceLocator.LayerdArchitectureExample
{
    public interface IModuleLayer
    {
        /// <summary>
        /// 层级基类
        /// </summary>
        T GetModule<T>() where T : class;
    }


    /// <summary>
    /// 逻辑层，UI/Game
    /// </summary>
    public interface ILogicLayer : IModuleLayer
    {

    }
    /// <summary>
    /// 业务模块层
    /// </summary>
    public interface IBusinessModuleLayer : IModuleLayer
    {

    }

    /// <summary>
    /// 公共模块层
    /// </summary>
    public interface IPublicModuleLayer : IModuleLayer
    {

    }

    /// <summary>
    /// 基础设施层
    /// </summary>
    public interface IBasicModuleLayer : IModuleLayer
    {

    }

    /// <summary>
    /// 工具层
    /// </summary>
    public interface IUtiltyLayer : IModuleLayer
    {

    }
}

