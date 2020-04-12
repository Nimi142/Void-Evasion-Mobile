using System;
using UnityEngine;

[Serializable]
public class MyColor
{
    public float[] colorStore = new float[4] {1f, 1f, 1f, 1f};

    public Color color
    {
        get => new Color(colorStore[0], colorStore[1], colorStore[2], colorStore[3]);
        set { colorStore = new float[4] {value.r, value.g, value.b, value.a}; }
    }

    public static implicit operator Color(MyColor instance)
    {
        return instance.color;
    }

    public static implicit operator MyColor(Color newColor)
    {
        return new MyColor {color = newColor};
    }

    public override string ToString()
    {
        return "(" + colorStore[0] + "," + colorStore[1] + "," + colorStore[2] + ")";
    }
}