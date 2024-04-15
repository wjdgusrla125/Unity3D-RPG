using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineTrigger : MonoBehaviour
{
    private bool isPlay = false;
    private PlayableDirector director;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        //_collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (director != null && isPlay == false)
        {
            director.Play();
            isPlay = true;
        }
    }
}
