using System.Collections;
using System.Collections.Generic;
using RougeLike.Battle.Configs;
using Sirenix.OdinInspector;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;
using UnityEngine.UI;

public class LoginSecondView : SerializedMonoBehaviour
{
    public TextMeshProUGUI title; 
    public GameObject map;
    public GameObject character;
    public GameObject rank;
    public GameObject setting;

    void SetPanel(int index)
    {
        switch (index)
        {
            case 1:
                title.text = "选择地图";
                map.SetActive(true);
                setting.SetActive(false);
                character.SetActive(false);
                rank.SetActive(false);
                break;
            case 2:
                title.text = "角色设置";
                map.SetActive(false);
                setting.SetActive(false);
                character.SetActive(true);
                rank.SetActive(false);
                break;
            case 3:
                title.text = "排行榜";
                map.SetActive(false);
                setting.SetActive(false);
                character.SetActive(false);
                rank.SetActive(true);
                break;
            case 4:
                title.text = "设置";
                map.SetActive(false);
                setting.SetActive(true);
                character.SetActive(false);
                rank.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open(int index)
    {
        gameObject.SetActive(true);
        SetPanel(index);
    }
}