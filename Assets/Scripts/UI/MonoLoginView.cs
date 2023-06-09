using System;
using System.Collections.Generic;
using CustomSerialize;
using RougeLike.Battle.Configs;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoLoginView : SerializedMonoBehaviour
{
    public static MonoLoginView instance;
    public GameObject firstPanel;
    public LoginSecondView secondPanel;
    public ConfigCharacter DesCharacter;
    public List<ConfigCharacter> players;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        DesCharacter.SetConfig(players[0]);
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
        DesCharacter.SetConfig(players[index]);
    }

    public void SetName(string input)
    {
        GameSave.PlayerName = input;
    }
}
