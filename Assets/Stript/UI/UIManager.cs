using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{

    [Header("===Card select Ui===")]
    [SerializeField]
    private CardSelectUI _cardSelectUI;

    [Header("===Marker State Ui===")]
    [SerializeField] private TextMeshProUGUI _stateText;

    [Header("===EXP BAR===")]
    [SerializeField] private Slider _expSliderBar;
    [SerializeField] private TextMeshProUGUI _playerLevel;
    [SerializeField] private Button _GetExpButton;

    // 프로퍼티
    public CardSelectUI cardSelectUi => _cardSelectUI;

    protected override void Singleton_Awake()
    {
        _GetExpButton.onClick.AddListener(F_TEST);
    }

    private void Start()
    {
        // 플레이어 state 출력 
        F_UpdateMarkerStateText();

    }

    // ##TODO : 임시 경험치 획득 버튼 만들기 
    private void F_TEST() 
    {
        PlayerManager.Instance.F_AddEXP(0.7f);
    }

    // EXP Slider Bar 적용
    public void F_UpdateInGameUI(float v_value , int v_level) 
    {
        // Slider bar 업데이트 
        _expSliderBar.value     = v_value;
        _playerLevel.text       = v_level.ToString();
    }

    // Card Ui On
    public void F_ReadyToOpenCardUi() 
    {
        // 시간 멈추기 
        Time.timeScale = 0;

        // Card select ui 켜기
        _cardSelectUI.F_ShowCard();
    }

    // 상태변화가 있을 때 적용
    public void F_UpdateMarkerStateText() 
    {
        _stateText.text
            = "max hp : " + PlayerManager.Instance.markers.markerState.markerMaxHp + '\n'
            + "move speed: " + PlayerManager.Instance.markers.markerState.markerMoveSpeed + '\n'
            + "shield cool Time: " + PlayerManager.Instance.markers.markerState.markerShieldCoolTime + '\n'
            + "shoot cool Time:" + PlayerManager.Instance.markers.markerState.markerBulletShootCoolTime + '\n'
            + "searRadious" + PlayerManager.Instance.markers.markerState.markerSearchRadious;
    }


}
