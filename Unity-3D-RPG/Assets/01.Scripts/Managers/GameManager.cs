using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameObject Player;

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void Awake()
    {
        if(GameObject.FindGameObjectWithTag("Player"))
            Player = GameObject.FindGameObjectWithTag("Player");
    }

    
}
