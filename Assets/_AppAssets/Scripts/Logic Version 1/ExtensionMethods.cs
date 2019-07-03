using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector3 Sum(this IEnumerable<Vector3> vectors)
    {
        Vector3 sum = Vector3.zero;
        foreach(Vector3 vector in vectors)
        {
            sum += vector;
        }
        return sum;
    }

    public static List<Transform> GetChildren(this Transform parent)
    {
        List<Transform> transforms = new List<Transform>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            transforms.Add(parent.transform.GetChild(i));
        }
        return transforms;
    }
}
