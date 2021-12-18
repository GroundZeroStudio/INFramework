/****************************************************
    文件：AbstractModuleLayer.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using INFrameworkDesign.ServiceLocator.Default;

namespace INFrameworkDesign.ServiceLocator.LayerdArchitectureExample
{
    public abstract class AbstractModuleLayer : IModuleLayer
    {
        private ModuleContainer mContainer = null;

        //子类提供
        protected abstract AssemblyModuleFactory mFactory { get; }

        public AbstractModuleLayer()
        {
            mContainer = new ModuleContainer(new DefaultModuleCache(), mFactory);
        }

        public T GetModule<T>() where T : class
        {
            return mContainer.GetModule<T>();
        }
    }
}


