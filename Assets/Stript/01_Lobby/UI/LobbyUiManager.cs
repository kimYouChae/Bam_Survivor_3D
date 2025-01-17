using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUiManager : Singleton<LobbyUiManager>
{
    [Header("===Lobby Component===")]
    [SerializeField] private Button _playerButton;

    [Header("===UI Regions===")]
    [SerializeField] private GameObject _Goods;
    [SerializeField] private GameObject _Lobby;
    [SerializeField] private GameObject _CharacterSelect;

    protected override void Singleton_Awake()
    {
        _playerButton.onClick.AddListener(F_PlayerSelectUiOnOff);                   
    }

    private void F_PlayerSelectUiOnOff() 
    {
        F_OnOffUIRegions(_Lobby);
        F_OnOffUIRegions(_CharacterSelect);
    }

    // 인자로 들어온 ui를 on/off
    private void F_OnOffUIRegions(GameObject _regions)
    {        
        // On -> off
        // Off -> On
        _regions.SetActive(!_regions.activeSelf);
    }

}
