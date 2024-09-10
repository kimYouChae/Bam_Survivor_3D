using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum UNIT_STATE 
{ 
    Idle,
    Tracking,
    Attack,
    Die
}

public class Unit : MonoBehaviour
{
    /// <summary>
    /// Unit���� �θ� 
    /// 
    /// 1. hp
    /// 2. �̵��ӵ�
    /// 3. abstract ����
    /// 
    /// </summary>

    [Header("===Uint State===")]
    [SerializeField] protected float   _unitHp;           // hp
    [SerializeField] protected float _unitSpeed;        // speed 
    [SerializeField] protected float _unitAttackTime;   // ���� ���ӽð�
    [SerializeField] protected float _unitTimeStamp;   // ���� �� 0���� �ʱ�ȭ                                                 
    [SerializeField] protected float _searchRadious;       // �÷��̾� ���� ���� 

    [Header("===FSM===")]
    public HeadMachine _UnitHeadMachine;
    public FSM[] _UnitStateArr;
    [SerializeField] public UNIT_STATE _curr_UNITS_TATE;           // ���� enum
    [SerializeField] public UNIT_STATE _pre_UNITS_TATE;           // ���� enum 


    [Header("===LayerMask===")]
    public LayerMask _hitWallLayerMask;

    // ##TODO ��������
    [Header("===Ect Object===")]
    [SerializeField] protected GameObject _dangerLine;
    [SerializeField] protected LineRenderer _dangerBounceLine;

    // ������Ƽ
    public float unitSpeed      => _unitSpeed;
    public float searchRadious  => _searchRadious;
    public float unitTimeStamp { get => _unitTimeStamp; set{ _unitTimeStamp = value;} }

    private void Start()
    {
        _hitWallLayerMask = LayerMask.GetMask("Wall");
    }

    // ���� �ʱ�ȭ
    // ##TODO CVS�� ������ ���� �� �����ʿ���
    protected virtual void F_InitUnitUnitState() { }

    // attack ���� ������
    public virtual void F_UnitAttatk() { }

    // FSM ���� 
    protected void F_InitUnitState( Unit v_standard ) 
    {
        // ���ӽ� ���� ( �ڽĿ��� �Լ����� , �ڽ� ������ headmachine�� �� )
        _UnitHeadMachine = new HeadMachine(v_standard);

        // FSM array ����
        _UnitStateArr = new FSM[System.Enum.GetValues(typeof(UNIT_STATE)).Length];

        _UnitStateArr[(int)UNIT_STATE.Idle]         = new Unit_Idle(v_standard);
        _UnitStateArr[(int)UNIT_STATE.Tracking]     = new Unit_Tracking(v_standard);
        _UnitStateArr[(int)UNIT_STATE.Attack]       = new Unit_Attack(v_standard);
        _UnitStateArr[(int)UNIT_STATE.Die]          = new Unit_Die(v_standard);

        // ������� ���� 
        _curr_UNITS_TATE = UNIT_STATE.Tracking;

        // Machine�� ���� �ֱ� 
        _UnitHeadMachine.HM_SetState(_UnitStateArr[(int)_curr_UNITS_TATE]);
    }

    // ���� ���� ���� ( 1ȸ , Start���� ���� )
    protected void F_CurrStateEnter() 
    {
        // head Machine�� enter
        _UnitHeadMachine.HM_StateEnter();
    }
 
    // ���� ���� ���� ( update���� ���� )
    protected void F_CurrStateExcute() 
    {
        // head Machine�� excute 
        _UnitHeadMachine.HM_StateExcute();
    }

    public void F_ChangeState( UNIT_STATE v_state ) 
    {
        // UNIT_STATE�� �´� FSM���� ���º�ȭ 
        // head Machine�� Change 

        _UnitHeadMachine.HM_ChangeState(_UnitStateArr[(int)v_state]);
    }

    // Unit hp �˻�
    public void F_ChekchUnitHp() 
    {
        // hp�� 0���ϸ� true 
        if (_unitHp <= 0)
        {
            // Die�� ���º�ȭ
            F_ChangeState(UNIT_STATE.Die);
        }
    }

    public void F_UniTracking(Unit v_unit) 
    {
        // 1. �÷��̾� ����
        v_unit.gameObject.transform.position
            = Vector3.MoveTowards(v_unit.gameObject.transform.position,
                PlayerManager.instance.markerHeadTrasform.position, v_unit.unitSpeed * Time.deltaTime);

        // 2. ���������� marker �� ����Ǹ�
        Collider[] _coll = Physics.OverlapSphere
            (v_unit.gameObject.transform.position, v_unit.searchRadious, PlayerManager.instance.markerLayer);

        if (_coll.Length > 0)
        {
            // ��������
            v_unit.F_ChangeState( UNIT_STATE.Attack );
        }
       
    }

    public void F_DrawLine() 
    {
        Debug.DrawRay(gameObject.transform.position , transform.up * 30f , Color.red);
    }

    public void F_GetDamage(float v_damage) 
    {
        // damage��ŭ hp ����
        _unitHp -= v_damage;
    }

    public void F_ChageSpeed(float v_speed) 
    {
        // �ӵ� ���� 
        _unitSpeed = v_speed;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Unit"))
        {
            Debug.Log("�Ѿ��̶� �浹");
        }                   

    }
}
