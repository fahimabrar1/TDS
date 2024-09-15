using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

[CustomEditor(typeof(MonoBehaviour), true, isFallback = true)]
[CanEditMultipleObjects]
public class ButtonDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var methods = target.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => m.GetCustomAttributes(typeof(ButtonAttribute), false).Length > 0)
            .ToArray();

        foreach (var methodInfo in methods)
        {
            var buttonAttribute = (ButtonAttribute)methodInfo.GetCustomAttributes(typeof(ButtonAttribute), false)[0];
            string buttonLabel = buttonAttribute.Label ?? methodInfo.Name;

            // Customize the appearance of the button
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 16;
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.normal.background = MakeTex(600, 1, new Color(0, 0.5f, 0)); // Green background

            if (GUILayout.Button(buttonLabel, buttonStyle, GUILayout.Height(40))) // Large button
            {
                foreach (var t in targets) // Support multi-object editing
                {
                    methodInfo.Invoke(t, null);
                }
            }
        }
    }

    // Helper method to create a texture
    private Texture2D MakeTex(int width, int height, Color color)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = color;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}
