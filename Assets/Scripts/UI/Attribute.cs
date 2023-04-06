using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Attribute : MonoBehaviour
    {
        public TextMeshProUGUI text_Level;
        public TextMeshProUGUI text_Data;
        public Button button_Add;
        public int level;
        public void SetData(int lv,string format)
        {
            text_Level.text = "Lv." + lv;
            text_Data.text = format;
            level = lv;
        }
    }
}