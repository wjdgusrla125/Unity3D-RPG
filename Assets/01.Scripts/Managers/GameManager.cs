using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private GameObject PlayerPrefab;
    private GameObject player;
    private Scene currentScene;
    private static bool playerInstatiated = false;
    private bool isPaused = false;

    private void Awake()
    {
        base.Awake();
        currentScene = SceneManager.GetActiveScene();
        PlayerPrefab = Resources.Load<GameObject>("Prefabs/Player/Player");
        Init();
    }

    private void Init()
    {
        player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad(player);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    

    public  GameObject Player { get { return player; } }
    public Scene Scene { get { return currentScene; } }
}
