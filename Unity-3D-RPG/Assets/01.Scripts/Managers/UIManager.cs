using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    private FSMPlayer _player;
    private PlayerHealth _playerHealth;
    private PlayerMana _playerMana;
    
    public GameObject InvenUI;
    public GameObject ItemSlotUI;
    public GameObject Dungeon1UI;
    public Slider playerHPUI;
    public Slider playerMPUI;
    public TMP_Text LifeNum;
    
    public Image[] skillIcons;
    public TMP_Text[] cooldownTexts;
    
    public Transform Dungeon1;
    public Transform Town;
    
    public static bool GamePaused = false;
    bool IsOpenInventory = false;
    
    private void Start()
    {
        _player = GameManager.Instance.Player.GetComponent<FSMPlayer>();
        _playerHealth = GameManager.Instance.Player.GetComponent<PlayerHealth>();
        _playerMana = GameManager.Instance.Player.GetComponent<PlayerMana>();
        
        StartCoroutine(InvenUIActive());
        ResetCooldown();
    }
    private void Update()
    {
        Cooldown();
        UpdatePlayerUI();
        OpenInventory();
    }
    
    public void Pausegame()
    {
        if (GamePaused == false)
        {
            Time.timeScale = 0f;
        }

        GamePaused = true;
    }
    public void Resumegame()
    {
        if (GamePaused == true)
        {
            Time.timeScale = 1f;
        }
        GamePaused = false;
    }

    //스킬쿨타임
    private void ResetCooldown()
    {
        for (int i = 0; i < _player.skillCooldowns.Length; i++)//스킬 쿨타임 초기화
        {
            _player.skillCooldowns[i] = 0f; // 초기 쿨타임 0으로 설정
            UpdateSkillUI(i); // UI 초기화
        }
    }
    private void Cooldown()
    {
        for (int i = 0; i < _player.skillCooldowns.Length; i++)
        {
            _player.skillCooldowns[i] -= Time.deltaTime;
            
            if (_player.skillCooldowns[i] < 0f)
            {
                _player.skillCooldowns[i] = 0f;
            }
            UpdateSkillUI(i);
        }
    }
    private void UpdateSkillUI(int index)//스킬 쿨타임 실행 메소드
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
    private float GetSkillCooldown(int index)//스킬 쿨타임 지정 메소드
    {
        switch (index)
        {
            case 0: return 0f; // BASEATTACK 쿨타임
            case 1: return 10f; // DOUBLEATTACK 쿨타임
            case 2: return 15f; // SPINATTACK 쿨타임
            case 3: return 8f; // BLOCK 쿨타임
            default: return 0f;
        }
    }
    
    //플레이어UI
    private void UpdatePlayerUI()
    {
        if (playerHPUI != null)//HP
        {
            playerHPUI.value = _playerHealth.currentHP / _playerHealth.MaxHP;
        }
        if (playerMPUI != null)//MP
        {
            playerMPUI.value = _playerMana.currentMP / _playerMana.MaxMP;
        }
    }
    private IEnumerator InvenUIActive()
    {
        InvenUI.SetActive(true);
        yield return new WaitForSeconds(0.001f);
        InvenUI.SetActive(false);
    }
    private void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!IsOpenInventory)
            {
                IsOpenInventory = true;
                InvenUI.SetActive(true);
            }
            else
            {
                IsOpenInventory = false;
                InvenUI.SetActive(false);
            }
        }
    }
    
    public void Teleport(string Name)
    {
        switch (Name)
        {
            case "Dungeon1":
                GameManager.Instance.Player.transform.position = Dungeon1.position;
                break;
            case "Town":
                GameManager.Instance.Player.transform.position = Town.position;
                break;
        }
    }
}