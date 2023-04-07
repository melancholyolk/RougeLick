using RougeLike.Interact;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RougeLike.Battle.UI
{
    public class MonoTreasureInfo : MonoBehaviour
    {
        public List<Sprite> sprites = new List<Sprite>();

        public Image image;

        public TextMeshProUGUI descript;

        public void SetInfo(int type, string str)
        {
            image.sprite = sprites[type];
            descript.text = str;
        }

    }
}

