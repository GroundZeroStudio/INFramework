/****************************************************
    文件：AssemblyModuleFactory.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace INFrameworkDesign.ServiceLocator.Default 
{
    public class AssemblyModuleFactory:IModuleFactory
    {
        private List<Type> mConcreteTypeCache;
        /// <summary>
        /// 抽象类型与具体类型对应的字典
        /// </summary>
        private Dictionary<Type, Type> mAbstractToConcrete = new Dictionary<Type, Type>();


        public AssemblyModuleFactory(Assembly rAssembly, Type rBaseModuleType)
        {
            //具体类型
            mConcreteTypeCache = rAssembly.GetTypes()
                .Where(t => rBaseModuleType.IsAssignableFrom(t) && !t.IsAbstract)
                .ToList();

            //获取具体类型中不是 IModule 接口的抽象类型
            //具体类型父接口类型
            foreach (var type in mConcreteTypeCache)
            {
                var interfacnes = type.GetInterfaces();
                foreach (var _interfance in interfacnes)
                {
                    //不是Module接口的父类型
                    if(rBaseModuleType.IsAssignableFrom(_interfance) && _interfance != rBaseModuleType)
                    {
                        mAbstractToConcrete.Add(_interfance, type);
                    }
                }
            }
        }

        public object CreateAllModules()
        {
            return mConcreteTypeCache.Select(t => t.GetConstructors().First().Invoke(null));
        }

        public object CreateModule(ModuleSearchKeys rKeys)
        {
            if (rKeys.Type.IsAbstract)
            {
                if (mAbstractToConcrete.ContainsKey(rKeys.Type))
                {
                    return mAbstractToConcrete[rKeys.Type].GetConstructors().First().Invoke(null);
                }
            }
            else
            {
                if (mConcreteTypeCache.Contains(rKeys.Type))
                {
                    return rKeys.Type.GetConstructors().First().Invoke(null);
                }
            }
            return null;
        }
    }
}


