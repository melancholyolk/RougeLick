using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RougeLike.Map.Config
{
    /// <summary>
    /// 场景生成配置
    /// </summary>
    public class ConfigScene : SerializedScriptableObject
    {
        public List<ConfigGameObject> configs;
    }

    public class ConfigGameObject
    {
        public enum GenMethod
        {
            RandomSort,
            RandomWalk,
            GeometricSegmentation,
            Automaton,
            Fractal
        }
        public GameObject gameObject;
        public GenMethod method;
        public int num;
    }
}