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
            GameManager.Instance.Player.transform.position = Vector3.zero;
        }
    }
}
