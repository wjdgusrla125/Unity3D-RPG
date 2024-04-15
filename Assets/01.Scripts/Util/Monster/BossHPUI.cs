using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    public Slider BossUI;
    public EnemyHealth MS_Boss;
    
    private void Update()
    {
        BossUI.value = MS_Boss.currentHP / MS_Boss.MaxHP;
    }
}
