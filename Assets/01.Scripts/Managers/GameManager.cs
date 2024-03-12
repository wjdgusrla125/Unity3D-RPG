using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameObject PlayerPrefab;
    private GameObject player;
    private Scene currentScene;
    private static bool playerInstatiated = false;

    private void Awake()
    {
        base.Awake();
        currentScene = SceneManager.GetActiveScene();
        Invoke("Init",0.01f);
    }

    private void Init()
    {
        player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad(player);
        PlayerIO.LoadData();
    }

    

    public  GameObject Player { get { return player; } }
    public Scene Scene { get { return currentScene; } }
}
