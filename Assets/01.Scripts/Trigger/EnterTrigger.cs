using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    public string Name = null;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Name != null)
            {
                PlayerIO.SaveData();
                LoadingSceneManager.LoadingScene(Name);
            }
        }
    }
}
