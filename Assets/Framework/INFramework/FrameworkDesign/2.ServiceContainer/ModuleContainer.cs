/****************************************************
    文件：ModuleContainer.cs
    作者：Olivia
    功能：Nothing
*****************************************************/
using INFrameworkDesign.ServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace INFrameworkDesign
{
    public class ModuleContainer
    {
        /// <summary>
        /// 实例缓存
        /// </summary>
        private IModuleCache m_Cache;
        /// <summary>
        /// 实例工厂
        /// </summary>
        private IModuleFactory m_Factory;

        public ModuleContainer(IModuleCache rCache, IModuleFactory rFactory)
        {
            m_Cache = rCache;
            m_Factory = rFactory;
        }

        public T GetModule<T>() where T : class
        {
            var moduleSearchKeys = ModuleSearchKeys.Allocate<T>();
            var module = m_Cache.GetModule(moduleSearchKeys);
            if(module == null)
            {
                module = m_Factory.CreateModule(moduleSearchKeys);
                m_Cache.AddModule(moduleSearchKeys, module);
            }
            moduleSearchKeys.Recycle();
            return module as T;
        }

        public IEnumerable<T> GetAllModules<T>() where T : class
        {
            var moduleSearchKeys = ModuleSearchKeys.Allocate<T>();
            var modules = m_Cache.GetAllModules() as IEnumerable<object>;
            if(modules == null || !modules.Any())
            {
                modules = m_Factory.CreateAllModules() as IEnumerable<object>;
                foreach (var module in modules)
                {
                    m_Cache.AddModule(moduleSearchKeys, module);
                }      
            }
            moduleSearchKeys.Recycle();
            return modules.Select(s => s as T);
        }
    }
}


