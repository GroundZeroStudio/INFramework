/****************************************************
    文件：ModulePlatformReadme.cs
    作者：Olivia
    功能：Nothing
*****************************************************/
using UnityEngine;

namespace INFrameworkDesign
{
    public class ModulePlatformReadme : IEditorPlatformModule
    {
        public void OnGUI()
        {
            GUILayout.Label("编辑器模块化结构 使用说明", new GUIStyle()
            {
                fontSize = 20,
                fontStyle = FontStyle.Bold
            });

            GUILayout.Label("1. 在 Editor 目录下创建一个类");
            GUILayout.Label("2. 该类实现 IEditorPlatformModule 接口即可");
            GUILayout.Label("using UnityEngine;");
            GUILayout.Label("");
            GUILayout.Label("namespace FrameworkDesign2021");
            GUILayout.Label("{");
            GUILayout.Label("    public class ModulePlatformReadme : IEditorPlatformModule");
            GUILayout.Label("    {");
            GUILayout.Label("        public void OnGUI()");
            GUILayout.Label("        {");
            GUILayout.Label("            // 写一些代码");
            GUILayout.Label("        }");
            GUILayout.Label("    }");
            GUILayout.Label("}");
        }
    }
}


