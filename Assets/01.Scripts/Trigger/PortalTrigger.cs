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
            Debug.Log("portalTrigger OnTrigEnter");
            LoadingSceneManager.LoadingScene(SceneName);
            Invoke("SetPlayerPosition",0.1f);
        }
    }
    
    private void SetPlayerPosition()
    {
        Debug.Log("portalTrigger Setpos");
        GameManager.Instance.Player.transform.position = Vector3.zero;
    }
}
