using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTrigger : MonoBehaviour
{
    //pubilc Field
    public String Name;
    public PopupUI StageUIPopup;
    
    //Private Field
    private List<PopupUI> _allPopupList;
    
    
    //Unity 콜백
    private void Awake()
    {
        Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (Name)
            {
                case "Stage1":
                    StageUIPopup.gameObject.SetActive(true);
                    break;
            }
        }
    }

    //일반 메소드
    private void Init()
    {
        _allPopupList = new List<PopupUI>()
        {
            StageUIPopup
        };

        foreach (var popup in _allPopupList)
        {
            popup._closeButton.onClick.AddListener(() => ClosePopup(popup));
        }
    }

    private void ClosePopup(PopupUI popup)
    {
        popup.gameObject.SetActive(false);
    }
}
