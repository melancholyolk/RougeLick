using System.Collections.Generic;
using UnityEngine;

public class MonoFireEffect : MonoBehaviour
{
    public List<Transform> models;

    public void Resize(Vector3 size)
    {
        foreach (var transform in models)
        {
            transform.localScale = size;
        }
    }
}
