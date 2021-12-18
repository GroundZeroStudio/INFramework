/****************************************************
    文件：ServiceLocator.cs
    作者：TA94
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.Pattern 
{
    public class ServiceLocator
    {
        private readonly Cache m_Cache = new Cache();

        private readonly AbstractInitialContext m_Context;

        public ServiceLocator(AbstractInitialContext context)
        {
            m_Context = context;
        }

        public IService GetService(string name)
        {
            var rService = m_Cache.GetService(name);
            if(rService == null)
            {
                rService = m_Context.LookUp(name);
                m_Cache.AddService(rService);
            }
            if(rService == null)
            {
                throw new System.Exception("Service:" + name + "不存在");
            }
            return rService;
        }
    }
}


