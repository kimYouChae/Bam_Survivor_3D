using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("card select Ui")]
    [SerializeField]
    private CardSelectUI _cardSelectUI;

    [Header("Marker State Ui")]
    [SerializeField] private TextMeshProUGUI _stateText;

    // 프로퍼티
    public CardSelectUI cardSelectUi => _cardSelectUI;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        F_UpdateMarkerStateText();
    }

    // 상태변화가 있을 때 적용
    public void F_UpdateMarkerStateText() 
    {
        _stateText.text
            = "max hp : " + PlayerManager.instance.markers[0].markerState.markerMaxHp + '\n'
            + "move speed: " + PlayerManager.instance.markers[0].markerState.markerMoveSpeed + '\n'
            + "shield cool Time: " + PlayerManager.instance.markers[0].markerState.markerShieldCoolTime + '\n'
            + "shoot cool Time:" + PlayerManager.instance.markers[0].markerState.markerShootCoolTime + '\n'
            + "searRadious" + PlayerManager.instance.markers[0].markerState.markerSearchRadious;
    }

}
