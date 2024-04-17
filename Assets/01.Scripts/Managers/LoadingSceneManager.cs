using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    public Image ProgressBar;
    public TMP_Text LoadingText;
    public Image[] BG;

    public static bool isSceneLoaded = false;
    
    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadingScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    private IEnumerator LoadScene()
    {
        isSceneLoaded = false;
        
        yield return null;
        
        BGChanger(nextScene);
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0;

        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime/2;

            if (op.progress >= 0.9f)
            {
                if (GameManager.Instance.Player != null)
                {
                    GameManager.Instance.Player.transform.position = Vector3.zero;
                }
                
                ProgressBar.fillAmount = Mathf.Lerp(ProgressBar.fillAmount, 1f, timer);

                if (ProgressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    SoundManager.Instance.PlayBGM(nextScene);
                    isSceneLoaded = true;
                }
            }
            else
            {
                ProgressBar.fillAmount = Mathf.Lerp(ProgressBar.fillAmount, op.progress, timer);
                if (ProgressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }

            float Loading = ProgressBar.fillAmount * 100;
            LoadingText.text = ("LOADING : " + Loading.ToString("N1") + "%");
        }
    }

    private void BGChanger(string sceneName)
    {
        switch (sceneName)
        {
            case "02.Town":
                BG[0].gameObject.SetActive(true);
                break;
            case "03.Stage1":
                BG[1].gameObject.SetActive(true);
                break;
            case "04.Stage2":
                BG[2].gameObject.SetActive(true);
                break;
        }
    }
}
