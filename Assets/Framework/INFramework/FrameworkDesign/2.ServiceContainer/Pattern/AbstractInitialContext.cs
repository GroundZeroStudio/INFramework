/****************************************************
    文件：AbstractInitialContext.cs
    作者：Olivia
    功能：服务器模式查找器，用于实现查找创建IService实例
*****************************************************/
using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.Pattern
{
    public abstract class AbstractInitialContext
    {
        public abstract IService LookUp(string name);
    }
}


