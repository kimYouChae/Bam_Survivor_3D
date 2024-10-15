using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("===Player Level===")]
    [SerializeField] private int _PLAYERLEVEL;        // ���� ����
    [SerializeField] private float _CURREXP;        // ���� ����ġ
    [SerializeField] private float _MAXEXP;        // max ����ġ

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
    }

    private void Start()
    {
        // state �ʱ�ȭ 
        F_InitMarkerState();

        _markerLayer = LayerMask.GetMask("Marker");

        _markerHeadTrasform = _markers[0].transform;

        // exp 
        _PLAYERLEVEL    = 1;
        _CURREXP        = 0;
        _MAXEXP         = F_EXPAccorLevel(_PLAYERLEVEL);

        // ���� �� ui ������Ʈ 
        UIManager.Instance.F_UpdateInGameUI(0, _PLAYERLEVEL);
    }

    // level�� ���� ����ġ return
    private float F_EXPAccorLevel(int v_level) 
    {
        float a = Mathf.Floor((0.5f * Mathf.Pow(v_level, 2)) * 10f) / 10f;

        return a + (float)v_level + 1.0f;
    }

    // HP ȹ��
    public void F_AddEXP(float v_exp) 
    {
        _CURREXP += v_exp;

        // ���� �ִ� exp ������
        if (_CURREXP >= _MAXEXP)
        {
            Debug.Log( "���� level : " + _PLAYERLEVEL + " / ���� MAX" + _MAXEXP + " / ���� Curr" + _CURREXP );

            // ���� exp �ʱ�ȭ
            _CURREXP = _CURREXP - _MAXEXP;

            // �÷��̾� ���� ++
            _PLAYERLEVEL++;

            // max �ٽð�� 
            _MAXEXP = F_EXPAccorLevel(_PLAYERLEVEL);

            // card Ui On
            UIManager.Instance.F_ReadyToOpenCardUi();
        }

        // ui ������Ʈ ( curr - MAX ������ �ؾ���)
        UIManager.Instance.F_UpdateInGameUI(_CURREXP / _MAXEXP , _PLAYERLEVEL);

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


}
