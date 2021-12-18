using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace INFramework
{
	public class AssetBundleUtil
	{
        public static string FullPathForAssetBundleName(string assetBundleName)
        {
            return Application.streamingAssetsPath + "/AssetBundles/" + GetPlatformName() + "/" + assetBundleName;
        }

        public static string GetPlatformName()
        {
#if UNITY_EDITOR
            return GetPlatformName(UnityEditor.EditorUserBuildSettings.activeBuildTarget);
#else
            return GetPlatformName(Application.platform);
#endif
        }


        static string GetPlatformName(RuntimePlatform runtimePlatform)
        {
            switch (runtimePlatform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                case RuntimePlatform.LinuxPlayer:
                    return "Linux";
                default:
                    return null;
            }
        }


#if UNITY_EDITOR
        static string GetPlatformName(UnityEditor.BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case UnityEditor.BuildTarget.StandaloneWindows:
                case UnityEditor.BuildTarget.StandaloneWindows64:
                    return "Windows";

                case UnityEditor.BuildTarget.iOS:
                    return "iOS";

                case UnityEditor.BuildTarget.Android:
                    return "Android";
                case UnityEditor.BuildTarget.WebGL:
                    return "WebGL";
                case UnityEditor.BuildTarget.StandaloneLinux64:
                    return "Linux";
                case UnityEditor.BuildTarget.StandaloneOSX:
                    return "OSX";
            }

            return null;
        }
#endif
    }
}

