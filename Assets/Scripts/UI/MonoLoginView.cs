using System;
using System.Collections.Generic;
using RougeLike.Battle.Configs;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoLoginView : SerializedMonoBehaviour
{
    public static MonoLoginView instance;
    public GameObject firstPanel;
    public LoginSecondView secondPanel;
    public List<ConfigCharacter> players;
    /// <summary>
    /// 最后选择的人物数据
    /// </summary>
    [NonSerialized]
    public ConfigCharacter selectedCharacter;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        firstPanel.SetActive(true);
        secondPanel.Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void OpenSecondPanel(int index)
    {
        firstPanel.SetActive(false);
        secondPanel.Open(index);
    }

    public void CloseSecondPanel()
    {
        secondPanel.Close();
        firstPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    public static void LoadSceneAsync(int index)
    {
        var handle = SceneManager.LoadSceneAsync(index);
    }

    public void SelectPlayer(int index)
    {
        selectedCharacter = players[index];
    }
}
