using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("===Player Level===")]
    [SerializeField] private int _PLAYERLEVEL;        // 현재 레벨
    [SerializeField] private float _CURREXP;          // 현재 경험치
    [SerializeField] private float _MAXEXP;           // max 경험치

    [Header("===Sub State===")]
    [SerializeField] private int    _revivalCount;              // 부활횟수
    [SerializeField] private float  _experience;                // 추가 경험치
    [SerializeField] private float  _luck;                      // 행운

    [Header("===Script===")]
    [SerializeField] private MarkerMovement             _markerMovement;                    // marker 움직임
    [SerializeField] private MarkerShieldController     _markerShieldController;            // 쉴드 컨트롤러
    [SerializeField] private MarkerBulletController     _markerBulletController;            // 총알 컨트롤러
    [SerializeField] private MarkerExplosionConteroller _markerExplosionConteroller;        // 총알 폭발시 컨트롤러

    [Header("===Marker===")]
    [SerializeField] List<Marker> _markers;                         // Marker 클래스 리스트에 저장
    [SerializeField] List<Slider> _markerHpBar;                     // Marker의 hp바 

    [Header("===Layer===")]
    [SerializeField] private LayerMask _markerLayer;                // marker의 layer int 

    [Header("===Transform===")]
    [SerializeField] private Transform _markerHeadTrasform;         // marker head의 transform

    [Header("===Prefab===")]
    [SerializeField] private GameObject _boundaryToScreenObj;       // 스크린 기준 boundary
    [SerializeField] private Transform  _boudaryParent;             // boundary 부모 

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

        // exp 
        _PLAYERLEVEL    = 1;
        _CURREXP        = 0;
        _MAXEXP         = F_EXPAccorLevel(_PLAYERLEVEL);

        // ##TODO : 캐릭터 따라 달라져야함 임시로 초기화
        _revivalCount = 0;
        _experience = 1;
        _luck = 1;

        // 시작 시 ui 업데이트 
        UIManager.Instance.F_UpdateInGameUI(0, _PLAYERLEVEL);
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

            // card Ui On
            UIManager.Instance.F_ReadyToOpenCardUi();
        }

        // ui 업데이트 ( curr - MAX 값으로 해야함)
        UIManager.Instance.F_UpdateInGameUI(_CURREXP / _MAXEXP , _PLAYERLEVEL);

    }

    // marker State 초기화 
    private void F_InitMarkerState() 
    {
        // ## TODO : player에 따라 수정해야함 ( 추후 시작 player 종류가 많아지면 )

        for(int i = 0; i < _markers.Count; i++) 
        {
            _markers[i].markerState.F_SetMarkerState(
                _name       : "귀여운비버" , 
                _hp         : 10f , 
                _maxHp      : 10f ,
                _speed      : 3f , 
                _defence    : 0f , 
                _search     : 5f , 
                _magnet     : 2f, 
                _sCoolTime  : 2f, 
                _bCoolTime  : 2f, 
                _recovery   : 1f, 
                _rCoolTime  : 10f );

            // hp , maxhp , speed , 방어력  , 탐색범위 , 자석 범위 , 쉴드쿨타임 , 총알 쿨타임 , 회복량 , 회복쿨타임
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

    // 기본 state 업데이트 ( Marker 전체의 state 변화 ) 
    public void F_UpdateMarkerState(float MaxHpPercent = 0 , float SpeedPercent = 0 , float DefencePercent = 0 ,
        float SearchRadiousPercent = 0 , float MagnetPercent = 0 , float ShieldCoolTimePercent = 0 , float BulletCoolTimePercent = 0,
        float RecoveryIncrease = 0 , float RecoveryCoolTimeDecrease = 0 ) 
    {
        for(int i = 0; i < _markers.Count; i++) 
        {
            MarkerState state = _markers[i].markerState;

            state.markerMaxHp               += state.markerMaxHp * MaxHpPercent;
            state.markerMoveSpeed           += state.markerMoveSpeed * SpeedPercent;
            state.defence                   += state.defence * DefencePercent;
            state.markerSearchRadious       += state.markerSearchRadious * SearchRadiousPercent;
            state.magnetSearchRadious       += state.magnetSearchRadious * MagnetPercent;
            state.markerShieldCoolTime      -= state.markerShieldCoolTime * ShieldCoolTimePercent;
            state.markerBulletShootCoolTime -= state.markerBulletShootCoolTime * BulletCoolTimePercent;
            state.naturalRecoery            += RecoveryIncrease;
            state.recoveryCoolTime          -= RecoveryCoolTimeDecrease;

        }
    }

    // 추가 state 업데이트 
    public void F_UpdateMarkerSubState(int RevivalCount = 0, float ExperiencePercent = 0, float LuckPercent = 0)
    {
        this._revivalCount  += RevivalCount;
        this._experience    += _experience * ExperiencePercent;
        this._luck          += _luck * LuckPercent;
    }



}
