using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : Singleton<PopupUIManager> //싱글톤
{
    //public Field
    public PopupUI _inventoryPopup;
    public PopupUI _characterInfoPopup;
    public PopupUI _shopPopup;
    [Space]
    public KeyCode _escapeKey = KeyCode.Escape;
    public KeyCode _inventoryKey = KeyCode.I;
    public KeyCode _charInfoKey  = KeyCode.C;
    public KeyCode _shopKey = KeyCode.V;
    
    //private Field
    private LinkedList<PopupUI> _activePopupLList;  //실시간 팝업 관리 LinkedList
    private List<PopupUI> _allPopupList;            //전체 팝업 목록 List
        
    //Unity 콜백
    private void Awake()
    {
        base.Awake();
        
        _activePopupLList = new LinkedList<PopupUI>();
        Init();
        Invoke("InitCloseAll", 0.01f);
    }

    private void Update()
    {
        // ESC 누를 경우 링크드리스트의 First 닫기
        if (Input.GetKeyDown(_escapeKey))
        {
            if (_activePopupLList.Count > 0)
            {
                ClosePopup(_activePopupLList.First.Value);
            }
        }

        // 단축키 조작
        ToggleKeyDownAction(_inventoryKey, _inventoryPopup);
        ToggleKeyDownAction(_charInfoKey,  _characterInfoPopup);

        if (LoadingSceneManager.nextScene == "02.Town")
        {
            ToggleKeyDownAction(_shopKey,  _shopPopup);
        }
    }
    
    //일반 메소드
    private void Init()
    {
        // 1. 리스트 초기화
        _allPopupList = new List<PopupUI>()
        {
            _inventoryPopup, _characterInfoPopup, _shopPopup
        };

        // 2. 모든 팝업에 이벤트 등록
        foreach (var popup in _allPopupList)
        {
            // 헤더 포커스 이벤트
            popup.OnFocus += () =>
            {
                _activePopupLList.Remove(popup);
                _activePopupLList.AddFirst(popup);
                RefreshAllPopupDepth();
            };

            // 닫기 버튼 이벤트
            popup._closeButton.onClick.AddListener(() => ClosePopup(popup));
        }
        
        OpenPopup(_inventoryPopup);
    }
    
    //시작 시 모든 팝업 닫기
    private void InitCloseAll()
    {
        foreach (var popup in _allPopupList)
        {
            ClosePopup(popup);
        }
    }
    
    //단축 키 입력에 따라 팝업 열고 닫기
    private void ToggleKeyDownAction(in KeyCode key, PopupUI popup)
    {
        if (Input.GetKeyDown(key))
        {
            ToggleOpenClosePopup(popup);
        }
    }
    
    //팝업 상태에 따라 열고 닫기
    private void ToggleOpenClosePopup(PopupUI popup)
    {
        if (!popup.gameObject.activeSelf) OpenPopup(popup);
        else ClosePopup(popup);
    }
    
    //팝업을 열고 LinkedList 상단의 추가
    private void OpenPopup(PopupUI popup)
    {
        _activePopupLList.AddFirst(popup);
        popup.gameObject.SetActive(true);
        RefreshAllPopupDepth();
    }
    
    //팝업을 닫고 LinkedList에서 제거
    private void ClosePopup(PopupUI popup)
    {
        _activePopupLList.Remove(popup);
        popup.gameObject.SetActive(false);
        RefreshAllPopupDepth();
    }
    
    //LinkedList내 모든 팝업의 자식 순서 재배치
    private void RefreshAllPopupDepth()
    {
        foreach (var popup in _activePopupLList)
        {
            popup.transform.SetAsFirstSibling();
        }
    }
}