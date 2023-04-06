using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI textDescript;
    public TextMeshProUGUI textLv;
    public TextMeshProUGUI textType;
    public void SetInfo(Sprite sprite,string str,string des,int lv,string type)
    {
        image.sprite = sprite;
        textTitle.text = str;
        textDescript.text = des;
        textLv.text = "Lv:" + lv;
        textType.text = type;
    }
}
