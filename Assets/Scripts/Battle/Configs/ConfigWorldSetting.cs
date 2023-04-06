using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace RougeLike.Battle.Configs
{
    public class ConfigWorldSetting : SerializedScriptableObject
    {
        public List<WorldInfo> characters;
    }
    public class WorldInfo
    {
        public ConfigCharacter character;
        public ConfigProcess process;
        public string scene;
        public Sprite sprite;
    }
}

