/****************************************************
    文件：ModuleContainerModule.cs
    作者：Olivia
    功能：Nothing
*****************************************************/
using UnityEngine;

namespace INFrameworkDesign
{
    public class ModuleContainerModule : IEditorPlatformModule
    {
        public void OnGUI()
        {
            GUILayout.Label("ModuleContainer 使用说明", new GUIStyle()
            {
                fontSize = 20,
                fontStyle = FontStyle.Bold
            });

            GUILayout.Label("1. 指定接口类型");
            GUILayout.Label("public interface IModule {}");
            GUILayout.Label("2. 创建默认的模块缓存");
            GUILayout.Label("var cache = new DefaultModuleCache();");
            GUILayout.Label("3. 创建默认的模块工厂");
            GUILayout.Label("var factory = new AssemblyModuleFactory(typeof(IModule).Assembly,typeof(IModule));");
            GUILayout.Label("4. 创建模块容器的实例");
            GUILayout.Label("var container = new ModuleContainer(cache,factory);");
            GUILayout.Label("5. 获取 Module/Modules");
            GUILayout.Label("var module = container.GetModule<IHelloModule>();");
            GUILayout.Label("var modules = container.GetModule<IModules>();");
        }
    }

}
