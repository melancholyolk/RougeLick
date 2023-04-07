using System;
using CustomSerialize;
using RougeLike.Battle.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RankView : MonoBehaviour
    {
        public RectTransform content;
        public TMP_Dropdown filter;
        public GameObject prefab;
        private void Start()
        {
            ShowRank();
        }

        public void ShowRank()
        {
            int count = content.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                Destroy(content.GetChild(i).gameObject);
            }
            var list = GameSave.ReadXML();
            list.Sort((x, y) =>
            {
                if (filter.value == 0)
                {
                    return Int32.Parse(y.Item2) - Int32.Parse(x.Item2);
                }
                else
                {
                    return Int32.Parse(y.Item3) - Int32.Parse(x.Item3);
                }
            });
            for (int i = 0; i < list.Count; i++)
            {
                var go = Instantiate(prefab, content);
                var comp = go.GetComponent<RankItem>();
                comp.no.text = "No."+ (i + 1);
                comp.playerName.text = list[i].Item1;
                comp.kill.text = list[i].Item2;
                var second = Int32.Parse(list[i].Item3);
                var h = second / 3600;
                var m = second % 3600 / 60;
                var s = second % 60;
                comp.lifeTime.text = $"{h:00}:{m:00}:{s:00}";
            }
        }
    }
}