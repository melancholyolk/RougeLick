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
            time.text = totaltime + "";
            killNum.text = num + "";
        }
    }

}
