/****************************************************
    文件：BuildApp.cs
    作者：TA94
    日期：2021/10/31 12:3:16
    功能：打包工具

    ///将AssetBundle下的文件拷贝到Application.streamingAssetsPath
*****************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;

public class BuildApp
{
    private static string m_AppName = LoadFrameConfig.GetFrameConfig().m_AppName;
    public static string m_AndroidPath = Application.dataPath + "/../BuildTarget/Android/";
    public static string m_IOSPath = Application.dataPath + "/../BuildTarget/IOS/";
    public static string m_WindowsPath = Application.dataPath + "/../BuildTarget/Windows/";

    [MenuItem("Build/标准包")]
    public static void Build()
    {
        //打包
        BundleEditor.Build();
        //生成可执行程序
        string abPath = Application.dataPath + "/../AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString() + "/";
        Copy(abPath, Application.streamingAssetsPath);

        string savePath = "";
        if(EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            savePath = m_AndroidPath + m_AppName + "_" + EditorUserBuildSettings.activeBuildTarget + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + ".apk";
        }
        if(EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
        {
            savePath = m_IOSPath + m_AppName + "_" + EditorUserBuildSettings.activeBuildTarget + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + ".apk";
        }
        if(EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64)
        {
            savePath = m_WindowsPath  + m_AppName + "_" + EditorUserBuildSettings.activeBuildTarget + string.Format("_{0:yyyy_MM_dd_HH_mm}/{1}.exe", DateTime.Now, m_AppName);
        }

        BuildPipeline.BuildPlayer(FindEnableEditorScenes(), savePath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);

        DeleteDir(Application.streamingAssetsPath);
    }

    /// <summary>
    /// 获取所有编辑器下的激活场景
    /// </summary>
    /// <returns></returns>
    public static string[] FindEnableEditorScenes()
    {
        List<string> editorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                editorScenes.Add(scene.path);
            }
        }
        return editorScenes.ToArray();
    }

    /// <summary>
    /// 复制文件夹
    /// </summary>
    /// <param name="rSourcePath">源文件夹路径</param>
    /// <param name="rTargetPath">目标文件夹路径</param>
    public static void Copy(string rSourcePath, string rTargetPath)
    {
        try
        {
            if (!Directory.Exists(rTargetPath))
            {
                Directory.CreateDirectory(rTargetPath);
            }
            //源文件目录
            string srcdir = Path.Combine(rTargetPath, Path.GetFileName(rSourcePath));
            if (Directory.Exists(rSourcePath))
            {
                srcdir += Path.DirectorySeparatorChar;
            }

            if (!Directory.Exists(srcdir))
            {
                Directory.CreateDirectory(srcdir);
            }

            string[] files = Directory.GetFileSystemEntries(rSourcePath);
            foreach (string file in files)
            {
                if (Directory.Exists(file))
                {
                    Copy(file, srcdir);
                }
                else
                {
                    File.Copy(file, srcdir + Path.GetFileName(file), true);
                }
            }
            
        }
        catch
        {
            Debug.LogError($"无法从{rSourcePath}复制文件到{rTargetPath}");
        }
    }

    /// <summary>
    /// 删除目录文件夹
    /// </summary>
    /// <param name="rSourcePath"></param>
    public static void DeleteDir(string rSourcePath)
    {
        try
        {
            DirectoryInfo dir = new DirectoryInfo(rSourcePath);
            FileSystemInfo[] fileInfo = dir.GetFileSystemInfos();
            foreach (FileSystemInfo info in fileInfo)
            {
                if(info is DirectoryInfo)
                {
                    DirectoryInfo subdir = new DirectoryInfo(info.FullName);
                    subdir.Delete(true);
                }
                else
                {
                    File.Delete(info.FullName);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }
}
