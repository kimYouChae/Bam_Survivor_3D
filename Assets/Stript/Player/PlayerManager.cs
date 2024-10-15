using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("===Player Level===")]
    [SerializeField] private int _PLAYERLEVEL = 0;        // 현재 레벨
    [SerializeField] private float _CURREXP     = 0;        // 현재 경험치
    [SerializeField] private float _MAXEXP      = 0;        // max 경험치

    [Header("===Script===")]
    [SerializeField] private MarkerMovement _markerMovement;                    // marker 움직임
    [SerializeField] private MarkerShieldController _markerShieldController;    // 쉴드 컨트롤러
    [SerializeField] private MarkerBulletController _markerBulletController;    // 총알 컨트롤러
    [SerializeField] private MarkerExplosionConteroller _markerExplosionConteroller;    // 총알 폭발시 컨트롤러

    [Header("===Marker===")]
    [SerializeField] List<Marker> _markers;             // Marker 클래스 리스트에 저장
    [SerializeField] List<Slider> _markerHpBar;         // Marker의 hp바 

    [Header("===Layer===")]
    [SerializeField] private LayerMask _markerLayer;             // marker의 layer int 

    [Header("===Transform===")]
    [SerializeField] private Transform _markerHeadTrasform;      // marker head의 transform

    [Header("===Prefab===")]
    [SerializeField] private GameObject _boundaryToScreenObj;      // 스크린 기준 boundary
    [SerializeField] private Transform _boudaryParent;            // boundary 부모 

    // 프로퍼티
    public MarkerMovement markerMovement => _markerMovement;
    public MarkerShieldController markerShieldController => _markerShieldController;
    public MarkerBulletController markerBulletController => _markerBulletController;
    public MarkerExplosionConteroller markerExplosionConteroller => _markerExplosionConteroller;
    public LayerMask markerLayer => _markerLayer;   
    public Transform markerHeadTrasform => _markerHeadTrasform;
    public List<Marker> markers => _markers;
    public List<Slider> markerHpBar => _markerHpBar;

    // marker count return 
    public int F_MarkerListCount() => _markers.Count;
   

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // state 초기화 
        F_InitMarkerState();

        _markerLayer = LayerMask.GetMask("Marker");

        _markerHeadTrasform = _markers[0].transform;

        // MAXHP
        _MAXEXP = F_EXPAccorLevel(_PLAYERLEVEL);

    }

    // level에 따른 경험치 return
    private float F_EXPAccorLevel(int v_level) 
    {
        float a = Mathf.Floor((0.5f * Mathf.Pow(v_level, 2)) * 10f) / 10f;

        return a + (float)v_level + 1.0f;
    }

    // HP 획득
    public void F_AddEXP(float v_exp) 
    {
        _CURREXP += v_exp;

        // 만약 최대 exp 넘으면
        if (_CURREXP >= _MAXEXP)
        {
            Debug.Log( "현재 level : " + _PLAYERLEVEL + " / 현재 MAX" + _MAXEXP + " / 현재 Curr" + _CURREXP );

            // 현재 exp 초기화
            _CURREXP = _CURREXP - _MAXEXP;

            // 플레이어 레벨 ++
            _PLAYERLEVEL++;

            // max 다시계산 
            _MAXEXP = F_EXPAccorLevel(_PLAYERLEVEL);

            // ##TODO : ui 업데이트 ( curr - MAX 값으로 해야함)

        }
        else 
        {
            // ##TODO : ui 업데이트 ( Max / Curr 값으로)

        }
    }

    // marker State 초기화 
    private void F_InitMarkerState() 
    {
        // ## TODO : player에 따라 수정해야함 ( 추후 시작 player 종류가 많아지면 )

        for(int i = 0; i < _markers.Count; i++) 
        {
            _markers[i].markerState.F_SetMarkerState(10f, 10f, 3f, 2f, 2f, 7f);

            // hp , maxHp, speed , 쉴드 쿨타임, 총 쿨타임, unit 감지 범위 
        }
    }

    // skillcard의 효과 적용
    public void F_ApplyCardEffect(SkillCard v_Card ) 
    {
        // skillcard 의 effect 추가 
        v_Card.F_SkillcardEffect();
    
        // marker ui 업데이트 
        UIManager.Instance.F_UpdateMarkerStateText();

    }


}
