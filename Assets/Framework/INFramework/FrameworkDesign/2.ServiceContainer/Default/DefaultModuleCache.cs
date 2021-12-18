/****************************************************
    文件：DefaultModuleCache.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace INFrameworkDesign.ServiceLocator.Default
{
    public class DefaultModuleCache : IModuleCache
    {
        private Dictionary<Type, List<object>> m_ModulesByType = new Dictionary<Type, List<object>>();

        public void AddModule(ModuleSearchKeys rKeys, object rModule)
        {
            if (m_ModulesByType.ContainsKey(rKeys.Type))
            {
                m_ModulesByType[rKeys.Type].Add(rModule);
            }
            else
            {
                m_ModulesByType.Add(rKeys.Type, new List<object>() { rModule });
            }
        }

        public object GetAllModules()
        {
            return m_ModulesByType.Values.SelectMany(list => list);
        }

        public object GetModule(ModuleSearchKeys rKeys)
        {
            List<object> output = null;
            if(m_ModulesByType.TryGetValue(rKeys.Type, out output))
            {
                return output.FirstOrDefault();
            }
            return null;
        }
    }
}


