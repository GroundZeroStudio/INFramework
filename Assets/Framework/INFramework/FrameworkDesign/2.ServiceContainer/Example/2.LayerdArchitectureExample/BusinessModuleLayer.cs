/****************************************************
    文件：BusinessModuleLayer.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using INFrameworkDesign.ServiceLocator.Default;

namespace INFrameworkDesign.ServiceLocator.LayerdArchitectureExample 
{
    public interface ISystem { }

    public class BusinessModuleLayer : AbstractModuleLayer, IBusinessModuleLayer
    {
        private AssemblyModuleFactory m_Factory;
        protected override AssemblyModuleFactory mFactory
        {
            get
            {
                return new AssemblyModuleFactory(typeof(ISystem).Assembly, typeof(ISystem));
            }
        }
    }
}


