/****************************************************
    文件：ABConfig.cs
    作者：TA94
    日期：2021/10/16 15:26:11
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResFramConfig : ScriptableObject
{
    /// <summary>
    /// 打包时生成的ab包配置表的二进制路径
    /// </summary>
    public string m_ABbytePath;

    //打包的默认名称
    public string m_AppName;
    /// <summary>
    /// xml文件夹路径
    /// </summary>
    public string m_XmlPath;
    /// <summary>
    /// 二进制文件夹路径
    /// </summary>
    public string m_BinaryPath;
    /// <summary>
    /// 脚本文件路径
    /// </summary>
    public string m_ScriptsPath;
}

[CustomEditor(typeof(ResFramConfig))]
public class ResFramConfigInspecctor : Editor
{
    public SerializedProperty m_ABbytePath;
    public SerializedProperty m_AppName;
    public SerializedProperty m_XmlPath;
    public SerializedProperty m_BinaryPath;
    public SerializedProperty m_ScriptsPath;

    private void OnEnable()
    {
        m_ABbytePath = serializedObject.FindProperty("m_ABbytePath");
        m_AppName = serializedObject.FindProperty("m_AppName");
        m_XmlPath = serializedObject.FindProperty("m_XmlPath");
        m_BinaryPath = serializedObject.FindProperty("m_BinaryPath");
        m_ScriptsPath = serializedObject.FindProperty("m_ScriptsPath");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_ABbytePath, new GUIContent("ab包二进制路径"));
        EditorGUILayout.PropertyField(m_AppName, new GUIContent("打包App名称"));
        EditorGUILayout.PropertyField(m_XmlPath, new GUIContent("xml配置表路径"));
        EditorGUILayout.PropertyField(m_BinaryPath, new GUIContent("二进制配置表路径"));
        EditorGUILayout.PropertyField(m_ScriptsPath, new GUIContent("配置表脚本路径"));
        serializedObject.ApplyModifiedProperties();
    }
}

public class LoadFrameConfig
{
    private const string FrameConfigPath = "Assets/ResFramework/Editor/INFrameConfig.asset";

    public static ResFramConfig GetFrameConfig()
    {
        ResFramConfig config = AssetDatabase.LoadAssetAtPath<ResFramConfig>(FrameConfigPath);
        return config;
    }
}
