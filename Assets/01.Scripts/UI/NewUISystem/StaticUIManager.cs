using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaticUIManager : Singleton<StaticUIManager> //싱글톤
{
    //public Field
    [Header("스킬")]
    public Image[] skillIcons;
    public TMP_Text[] cooldownTexts;
    [Header("체력 및 마나")]
    public Slider playerHPUI;
    public Slider playerMPUI;
    public TMP_Text playerHPText;
    public TMP_Text playerMPText;
    [Header("능력치")] 
    public TMP_Text offence;
    public TMP_Text defence;
    [Header("골드")] 
    public IntVariable gold;
    public TMP_Text goldText;
    [Header("목숨")] 
    public GameObject[] lifeCount;

    //private Field
    private FSMPlayer _player;
    private PlayerHealth _playerHealth;
    private PlayerMana _playerMana;
    private PlayerStats _playerStats;
    
    //Unity callback
    private void Awake()
    {
        base.Awake();
        
        Invoke("Init", 0.01f);
    }

    private void Update()
    {
        CoolDown();     //스킬 쿨타임
        UpdatePlayerStatus(); //플레이어 능력치 갱신
        UpdateGold();   //골드
        UpdateLifeCount(); //목숨
    }
    
    //일반 메소드
    private void Init()
    {
        _player = GameManager.Instance.Player.GetComponent<FSMPlayer>();
        _playerHealth = GameManager.Instance.Player.GetComponent<PlayerHealth>();
        _playerMana = GameManager.Instance.Player.GetComponent<PlayerMana>();
        _playerStats = GameManager.Instance.Player.GetComponent<PlayerStats>();
        
        ResetCoolTime(); //쿨타임 리셋
    }

    //쿨타임 실행
    private void CoolDown()
    {
        for (int i = 0; i < _player.skillCooldowns.Length; i++)
        {
            _player.skillCooldowns[i] -= Time.deltaTime;

            if (_player.skillCooldowns[i] < 0f)
                _player.skillCooldowns[i] = 0f;
            
            UpdateSkillUI(i);
        }
    }
    
    //쿨타임 초기화
    private void ResetCoolTime()
    {
        for (int i = 0; i < _player.skillCooldowns.Length; i++) // 스킬 쿨타임 초기화
        {
            _player.skillCooldowns[i] = 0f; // 초기 쿨타임 0으로 설정
            
            UpdateSkillUI(i);
        }
    }
    
    //쿨타임 UI 반영
    private void UpdateSkillUI(int index)
    {
        // 쿨타임이 남은 경우 UI 업데이트
        if (_player.skillCooldowns[index] > 0f)
        {
            skillIcons[index].fillAmount = _player.skillCooldowns[index] / GetSkillCooldown(index);
            cooldownTexts[index].text = Mathf.CeilToInt(_player.skillCooldowns[index]).ToString();
        }
        else
        {
            skillIcons[index].fillAmount = 0f;
            cooldownTexts[index].text = "";
        }
    }
    
    //스킬 쿨타임 지정 메소드 *수정 필요
    private float GetSkillCooldown(int index)
    {
        switch (index)
        {
            case 0: return 0f;  // BASEATTACK 쿨타임
            case 1: return 10f; // DOUBLEATTACK 쿨타임
            case 2: return 15f; // SPINATTACK 쿨타임
            case 3: return 8f;  // BLOCK 쿨타임
            default: return 0f;
        }
    }
    
    //체력 및 마나 업데이트
    private void UpdatePlayerStatus()
    {
        if(playerHPUI != null)
            playerHPUI.value = _playerHealth.currentHP / _playerHealth.MaxHP;
        
        if(playerMPUI != null)
            playerMPUI.value = _playerMana.currentMP / _playerMana.MaxMP;

        if (playerHPText != null)
            playerHPText.text = _playerHealth.currentHP.ToString();
        
        if (playerMPText != null)
            playerMPText.text = _playerMana.currentMP.ToString();

        if (_playerStats.Offence != null)
            offence.text = _playerStats.Offence.value.ModifiedValue.ToString();
        
        if (_playerStats.Defence != null)
            defence.text = _playerStats.Defence.value.ModifiedValue.ToString();
    }
    
    //골드 업데이트
    private void UpdateGold()
    {
        if (goldText)
            goldText.text = gold.Value.ToString();
    }
    
    //하트 업데이트
    private void UpdateLifeCount()
    {
        for (int i = 0; i < lifeCount.Length; i++)
        {
            lifeCount[i].SetActive(false);
        }

        for (int i = 0; i < _playerHealth.LifeCount; i++)
        {
            lifeCount[i].SetActive(true);
        }
    }
}
