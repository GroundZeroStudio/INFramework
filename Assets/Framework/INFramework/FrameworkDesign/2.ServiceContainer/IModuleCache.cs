/****************************************************
    文件：IModuleCache.cs
    作者：Olivia
    功能：Module容器，数据结构，提供一些增查API
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

namespace INFrameworkDesign.ServiceLocator 
{
    public interface IModuleCache
    {
        object GetModule(ModuleSearchKeys rKeys);
        void AddModule(ModuleSearchKeys rKeys, object rModule);
        object GetAllModules();
    }
}


