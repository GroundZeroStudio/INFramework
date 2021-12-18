/****************************************************
    文件：Cache.cs
    作者：Olivia
    功能：Nothing
*****************************************************/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.Pattern
{
    public class Cache 
    {
        List<IService> m_Services = new List<IService>();

        public IService GetService(string serviceName)
        {
            //(条件)在确定的条件下，只有一个或者0个值；如果一个以上的值符合条件 则报错。
            return m_Services.SingleOrDefault(s => s.Name == serviceName);
        }

        public void AddService(IService service)
        {
            m_Services.Add(service);
        }
    }
}


