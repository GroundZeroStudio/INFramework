/****************************************************
    文件：IService.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

namespace INFrameworkDesign.ServiceLocator.Pattern
{
    public interface IService 
    {
        string Name { get; }
        /// <summary>
        /// 执行
        /// </summary>
        void Execute();
    }

}

