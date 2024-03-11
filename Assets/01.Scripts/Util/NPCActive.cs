using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCActive : MonoBehaviour
{
    public GameObject UI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UI.SetActive(true);
            UIManager.Instance.InvenUI.SetActive(true);
        }
    }
}
