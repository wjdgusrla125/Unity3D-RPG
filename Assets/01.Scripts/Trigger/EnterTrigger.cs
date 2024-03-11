using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    public string Name = null;
    
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         if (Name != null)
    //         {
    //             PlayerIO.SaveData();
    //             LoadingSceneManager.LoadingScene(Name);
    //         }
    //     }
    // }

    public void LoadingScene()
    {
        if (Name != null)
        {
            Debug.Log("EnterTrigger loadScene");
            LoadingSceneManager.LoadingScene(Name);
            Invoke("SetPlayerPosition",0.1f);
        }
    }
    
    private void SetPlayerPosition()
    {
        Debug.Log("EnterTrigger Setpos");
        GameManager.Instance.Player.transform.position = Vector3.zero;
    }
}
