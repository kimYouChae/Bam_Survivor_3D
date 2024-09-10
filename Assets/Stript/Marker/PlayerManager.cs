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
    [SerializeField] private float _markerShieldCoolTime;       // marker ���� ��Ÿ�� 
    [SerializeField] private float _markerShootCoolTime;        // �Ѿ� �߻� ��Ÿ�� 
    [SerializeField] private float _markerSearchRadious;        // unit Ž�� ����

    // ������Ƽ
    public float markerHp => _markerHp;
    public float markerMaxHp { get => _markerMaxHp; set { _markerMaxHp = value; } }
    public float markerMoveSpeed { get => _markerMoveSpeed; set { _markerMoveSpeed = value;  } }
    public float markerShieldCoolTime { get => _markerShieldCoolTime; set { _markerShieldCoolTime = value; } }
    public float markerShootCoolTime { get => _markerShootCoolTime; set { _markerShootCoolTime = value; } }
    public float markerSearchRadious { get => _markerSearchRadious;  set { _markerSearchRadious = value; } }


    // ������ 
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
    [SerializeField] private MarkerMovement _markerMovement;                    // marker ������
    [SerializeField] private MarkerShieldController _markerShieldController;    // ���� ��Ʈ�ѷ�
    [SerializeField] private MarkerBulletController _markerBulletController;    // �Ѿ� ��Ʈ�ѷ�
    [SerializeField] private MarkerExplosionConteroller _markerExplosionConteroller;    // �Ѿ� ���߽� ��Ʈ�ѷ�

    [Header("===Marker===")]
    [SerializeField] List<Marker> _markers;             // Marker Ŭ���� ����Ʈ�� ����
    [SerializeField] List<Slider> _markerHpBar;         // Marker�� hp�� 

    [Header("===Layer===")]
    [SerializeField] private LayerMask _markerLayer;             // marker�� layer int 

    [Header("===Transform===")]
    [SerializeField] private Transform _markerHeadTrasform;      // marker head�� transform

    [Header("===Prefab===")]
    [SerializeField] private GameObject _boundaryToScreenObj;      // ��ũ�� ���� boundary
    [SerializeField] private Transform _boudaryParent;            // boundary �θ� 

    // ������Ƽ
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

        // Boundary ���� 
        F_CreateBoundaryByScreen();
    }

    private void Start()
    {
        // state �ʱ�ȭ 
        F_InitMarkerState();

        _markerLayer = LayerMask.GetMask("Marker");

        _markerHeadTrasform = _markers[0].transform;

    }

    // marker State �ʱ�ȭ 
    private void F_InitMarkerState() 
    {
        // ## TODO : player�� ���� �����ؾ��� ( ���� ���� player ������ �������� )

        for(int i = 0; i < _markers.Count; i++) 
        {
            _markers[i].markerState.F_SetMarkerState(10f, 10f, 3f, 2f, 2f, 7f);

            // hp , maxHp, speed , ���� ��Ÿ��, �� ��Ÿ��, unit ���� ���� 
        }
    }

    // skillcard�� ȿ�� ����
    public void F_ApplyCardEffect(SkillCard v_Card ) 
    {
        // skillcard �� effect �߰� 
        v_Card.F_SkillcardEffect();
    
        // marker ui ������Ʈ 
        UIManager.Instance.F_UpdateMarkerStateText();

    }

    // �ػ� ���� bullet�� bounce�� ��輱 ���� 
    public void F_CreateBoundaryByScreen() 
    { 
        // �ػ󵵴� 16 : 9�� ���� (����)


    }
}
