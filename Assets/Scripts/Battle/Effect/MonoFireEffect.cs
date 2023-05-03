using System;
using System.Collections.Generic;
using UnityEngine;

public class MonoFireEffect : MonoBehaviour
{
    public List<Transform> models;
    public List<GameObject> HideImmediately;
    
    public void Resize(Vector3 size)
    {
        foreach (var transform in models)
        {
            transform.localScale = size;
        }
    }

    public void Stop()
    {
        if(HideImmediately != null)
        {
            foreach (var game in HideImmediately)
            {
                game.SetActive(false);
            }
        }
        var particle = GetComponentsInChildren<ParticleSystem>();
        if (particle.Length > 0)
        {
            foreach (var p in particle)
            {
                p.Stop();
            }
        }
    }
}
