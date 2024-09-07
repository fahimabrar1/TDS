using System;
using UnityEngine;

public class MyDebug
{
    public static void Log(string str)
    {
#if DEBUG || UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log(str);
#endif
    }
    public static void LogWarning(string str)
    {
#if DEBUG || UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.LogWarning(str);
#endif
    }
    public static void LogError(string str)
    {
#if DEBUG || UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.LogError(str);
#endif
    }
}