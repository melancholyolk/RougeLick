using System.Collections.Generic;
using RougeLike.Battle.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MapInfo : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public Image image;
        public ScrollRect monstersScrollRect;
        public Button btn;
        public void Init(string name,Sprite sprite,List<MonsterInfo> monsters,int index)
        {
            title.text = name;
            image.sprite = sprite;
            btn.onClick.AddListener(() => MonoLoginView.LoadSceneAsync(index + 1));
            btn.onClick.AddListener(() => MonoLoginView.instance.Close());
            var allMonsters = Fundamental.ListPool<ConfigCharacter>.Get();
            foreach (var info in monsters)
            {
                foreach (var m in info.monsters)
                {
                    if (!allMonsters.Contains(m))
                    {
                        allMonsters.Add(m);
                    }
                }
            }

            for (int i = 0; i < allMonsters.Count; i++)
            {
                var go = new GameObject("Image" + i);
                var img = go.AddComponent<Image>();
                img.sprite = allMonsters[i].icon;
                go.transform.SetParent(monstersScrollRect.content);
            }
        }
    }
}