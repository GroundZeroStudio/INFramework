using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace INFramework
{
	public class AssetBundleSetting
	{
#if UNITY_EDITOR
        public static string AssetBundleName2Url(string name)
        {
            string retUrl = Application.persistentDataPath + "/AssetBundles/" +
                            GetPlatformName() + "/" + name;

            if (File.Exists(retUrl))
            {
                return retUrl;
            }

            return Application.streamingAssetsPath + "/AssetBundles/" +
                   GetPlatformName() + "/" + name;
        }

        static string GetPlatformName()
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "Windows";

                case BuildTarget.iOS:
                    return "iOS";

                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.WebGL:
                    return "WebGL";
                case BuildTarget.StandaloneLinux64:
                    return "Linux";
                case BuildTarget.StandaloneOSX:
                    return "OSX";
            }

            return null;
        }
#endif


    }
}

