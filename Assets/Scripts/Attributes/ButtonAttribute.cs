using System;
using UnityEngine;

public class ButtonAttribute : Attribute
{
    public string Label { get; }

    public ButtonAttribute(string label)
    {
        Label = label;
    }
}
