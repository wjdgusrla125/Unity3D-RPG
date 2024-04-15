using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPUI : MonoBehaviour
{
    public Slider hpbar;
    //public GameObject HPBar;
    
    private Transform cam;
    public EnemyHealth _enemyHealth;

    private void Awake()
    {
        cam = Camera.main.transform;
        _enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    private void Update()
    {
        MonsterUIUPdate();
    }

    private void MonsterUIUPdate()
    {
        transform.LookAt(transform.position + cam.rotation * Vector3.forward,cam.rotation*Vector3.up);

        hpbar.value = _enemyHealth.currentHP / _enemyHealth.MaxHP;
    }
}
