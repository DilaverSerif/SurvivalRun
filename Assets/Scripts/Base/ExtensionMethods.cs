using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static bool CheckLayer(this GameObject go, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << go.layer));
    }
    
    public static Color SetAlpha(this Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }

    public static float RandomValueVector2(this Vector2 vector)
    {
        return Random.Range(vector.x, vector.y);
    }
    
    public static Transform[] FindWithLayer(GameObject[] list,LayerMask mask)
    {
        Transform[] enemys = new Transform[list.Length];
        for (int i = 0; i < list.Length; i++)
        {
            if(CheckLayer(list[i],mask))
            {
                enemys[i] = list[i].transform;
            }
        }

        return enemys;
    }
    public static bool CheckTag(this GameObject gb, string tag)
    {
        return gb.CompareTag(tag);
    }

#if UNITY_EDITOR
    public static void DrawDisc(Vector3 center, float radius, Color color)
    {
        Handles.color = color;
        Handles.DrawWireDisc(center, Vector3.down, radius);
    }
#endif
}