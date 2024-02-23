using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private GameObject player;
    private Scene currentScene;
    
    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        currentScene = SceneManager.GetActiveScene();
        
        Invoke("Init()", 0.02f);
    }

    private void Init()
    {
        PlayerIO.LoadData();
    }
    
    public GameObject Player
    {
        get { return player; }
    }
    public Scene Scene
    {
        get { return currentScene; }
    }
}
