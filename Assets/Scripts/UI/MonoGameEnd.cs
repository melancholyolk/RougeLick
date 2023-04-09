using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace RougeLike.Battle.UI
{
    public class MonoGameEnd : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public TextMeshProUGUI time;
        public TextMeshProUGUI killNum;

        public void SetInfo(string vtitle,int totaltime,int num)
        {
            title.text = vtitle;
            var t = $"{totaltime/60:00}:{totaltime%60:00}";
            time.text = "存活时间：" + t + "";
            killNum.text = "击杀敌人：" + num + "";
        }
    }

}
