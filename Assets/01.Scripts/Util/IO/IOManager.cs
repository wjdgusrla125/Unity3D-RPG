using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;

public class IOManager : MonoBehaviour
{
    public string filePath = "PlayerData.json";
    public GameObject StartBtn;
    void Start()
    {
        bool fileExists = CheckResourceFile(filePath);

        if (fileExists)
        {
            StartBtn.SetActive(true);
        }
        else
        {
            StartBtn.SetActive(false);
        }
    }
    
    bool CheckResourceFile(string filePath)
    {
        string fullPath = Path.Combine("Assets/Resources/", filePath);
        
        if (File.Exists(fullPath))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
