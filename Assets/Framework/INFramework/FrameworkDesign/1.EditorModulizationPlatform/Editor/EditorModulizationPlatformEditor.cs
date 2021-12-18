using INFrameworkDesign.ServiceLocator.Default;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace INFrameworkDesign
{
#if UNITY_EDITOR
    public class EditorModulizationPlatformEditor : EditorWindow
    {
        private ModuleContainer mModuleContainer = null;

        [MenuItem("INFramework/Example/1.EditorModulizationPlatform")]
        public static void Open()
        {
            var editorPlatform = GetWindow<EditorModulizationPlatformEditor>();
            editorPlatform.position = new Rect(Screen.width / 2, Screen.height * 2 / 3, 600, 500);
            var moduleType = typeof(IEditorPlatformModule);
            var cache = new DefaultModuleCache();
            var factory = new AssemblyModuleFactory(moduleType.Assembly, moduleType);

            editorPlatform.mModuleContainer = new ModuleContainer(cache, factory);
            editorPlatform.Show();
        }

        private void OnGUI()
        {
            var modules = mModuleContainer.GetAllModules<IEditorPlatformModule>();
            foreach (var module in modules)
            {
                module.OnGUI();
            }
        }
    }
#endif
}

