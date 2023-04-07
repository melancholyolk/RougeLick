using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        public async void Init(string name,Sprite sprite,List<MonsterInfo> monsters,int index)
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

            await UniTask.Yield();
            for (int i = 0; i < allMonsters.Count; i++)
            {
                var go = monstersScrollRect.content.GetChild(i);
                var img = go.GetComponent<Image>();
                img.sprite = allMonsters[i].icon;
                go.gameObject.SetActive(true);
            }
        }
    }
}