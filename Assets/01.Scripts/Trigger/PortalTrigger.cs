using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    public string SceneName;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            LoadingSceneManager.LoadingScene(SceneName);
            GameManager.Instance.Player.transform.position = Vector3.zero;
        }
    }
}
