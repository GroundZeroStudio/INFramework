/****************************************************
    文件：ModuleSearchKeys.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace INFrameworkDesign
{
    public class ModuleSearchKeys
    {
        public string Name { get; set; }
        public Type Type { get; set; }

        //私有构造，防止用户自己new
        private ModuleSearchKeys() { }

        private static Stack<ModuleSearchKeys> m_Pool = new Stack<ModuleSearchKeys>(10);
        public static ModuleSearchKeys Allocate<T>()
        {
            ModuleSearchKeys outputKeys = null;
            outputKeys = m_Pool.Count != 0 ? m_Pool.Pop() : new ModuleSearchKeys();
            outputKeys.Type = typeof(T);
            return outputKeys;
        }

        public void Recycle()
        {
            Type = null;
            Name = null;
            m_Pool.Push(this);
        }
    }

}

