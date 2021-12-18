/****************************************************
    文件：LogicLayer.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using INFrameworkDesign.ServiceLocator.Default;
using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.LayerdArchitectureExample
{
    public interface ILogicController{}
    public class LogicLayer : AbstractModuleLayer, ILogicLayer
    {
        protected override AssemblyModuleFactory mFactory
        {
            get
            {
                return new AssemblyModuleFactory(typeof(ILogicController).Assembly, typeof(ILogicController));   
            }
        }
    }
}

