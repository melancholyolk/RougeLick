using System;
using System.Collections.Generic;
using RougeLike.Battle.Configs;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MapView : SerializedMonoBehaviour
    {
        public List<Toggle> selectors;
        public ConfigWorldSetting world;
        public GameObject prefab;
        public Transform container;
        private void Start()
        {
            var configs = world.characters;
            for (int i = 0; i < configs.Count; i++)
            {
                var go = Instantiate(prefab, container);
                var comp = go.GetComponent<MapInfo>();
                comp.Init(configs[i].scene,configs[i].sprite,configs[i].process.processs,i);
            }

            for (int i = 0; i < selectors.Count; i++)
            {
                int t = i;
                selectors[i].image.sprite = MonoLoginView.instance.players[i].icon;
                selectors[i].onValueChanged.AddListener((b ) => OnvalueChanged(t,b));
            }
        }

        void OnvalueChanged(int index,bool toggle)
        {
            if (toggle)
            {
                MonoLoginView.instance.SelectPlayer(index);
            }
        }
    }
}