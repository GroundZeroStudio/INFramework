/****************************************************
    文件：IModuleFactory.cs
    作者：Olivia
    功能：服务器查找，在Cache中不存在所需要的服务时，则会从该类中进行查找
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace INFrameworkDesign.ServiceLocator
{
    public interface IModuleFactory
    {
        object CreateModule(ModuleSearchKeys rKeys);
        object CreateAllModules();
    }

}

