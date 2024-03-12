using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    public string Name = null;

    public void LoadingScene()
    {
        if (Name != null)
        {
            LoadingSceneManager.LoadingScene(Name);
            Invoke("SetPlayerPosition",0.1f);
        }
    }
    
    private void SetPlayerPosition()
    {
        GameManager.Instance.Player.transform.position = Vector3.zero;
    }
}
