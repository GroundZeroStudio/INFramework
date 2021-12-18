using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace INFramework.Playground
{
    public class AssetBundleBuilder : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("INFramework/INResKit/Build AssetBundle")]
        static void BuildAssetBundles()
        {
            string outputPath = Application.streamingAssetsPath + "/AssetBundles/" + AssetBundleUtil.GetPlatformName();
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            BuildPipeline.BuildAssetBundles(outputPath,
                BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();
        }
#endif
    }
}

