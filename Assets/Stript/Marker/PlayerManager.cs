using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MarkerState
{
    [SerializeField] private float _markerHp;                     // marker Hp
    [SerializeField] private float _markerMaxHp;                  // marker max hp
    [SerializeField] private float _markerMoveSpeed;            // marker speed
    [SerializeField] private float _markerShieldCoolTime;       // marker 쉴드 쿨타임 
    [SerializeField] private float _markerShootCoolTime;        // 총알 발사 쿨타임 
    [SerializeField] private float _markerSearchRadious;        // unit 탐색 범위

    // 프로퍼티
    public float markerHp => _markerHp;
    public float markerMaxHp { get => _markerMaxHp; set { _markerMaxHp = value; } }
    public float markerMoveSpeed { get => _markerMoveSpeed; set { _markerMoveSpeed = value;  } }
    public float markerShieldCoolTime { get => _markerShieldCoolTime; set { _markerShieldCoolTime = value; } }
    public float markerShootCoolTime { get => _markerShootCoolTime; set { _markerShootCoolTime = value; } }
    public float markerSearchRadious { get => _markerSearchRadious;  set { _markerSearchRadious = value; } }


    // 생성자 
    public void F_SetMarkerState(float v_hp, float v_maxHp , float v_speed, float v_sCoolTime, float v_bCoolTime, float v_search)
    {
        this._markerHp = v_hp;
        this._markerMaxHp = v_maxHp;
        this._markerMoveSpeed = v_speed;
        this._markerShieldCoolTime = v_sCoolTime;
        this._markerShootCoolTime = v_bCoolTime;
        this._markerSearchRadious = v_search;
    }
}

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

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

        // Boundary 생성 
        F_CreateBoundaryByScreen();
    }

    private void Start()
    {
        // state 초기화 
        F_InitMarkerState();

        _markerLayer = LayerMask.GetMask("Marker");

        _markerHeadTrasform = _markers[0].transform;

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

    // 해상도 따라서 bullet이 bounce할 경계선 생성 
    public void F_CreateBoundaryByScreen() 
    { 
        // 해상도는 16 : 9로 고정 (가로)


    }
}
