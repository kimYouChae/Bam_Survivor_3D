using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    [Header("===Uint State===")]
    [SerializeField] protected UnitState _unitState;    // ���� Ŭ����
    [SerializeField] protected LifeCycle _lifeCycle;    // ���� ���� üũ

    [Header("===�׺���̼� ��Ÿ��===")]
    [SerializeField] const float _navActionCoolDown = 1f;

    [Header("===FSM===")]
    public HeadMachine _UnitHeadMachine;
    public FSM[] _UnitStateArr;
    [SerializeField] protected UNIT_STATE curr_UNITS_TATE;           // ���� enum
    [SerializeField] protected UNIT_STATE pre_UNITS_TATE;            // ���� enum 

    [Header("===Tracking===")]
    [SerializeField] private Vector3 _destiPosition     = Vector3.zero;
    [SerializeField] private NavMeshAgent _unitAgent    = null;
    [SerializeField] private Vector2 _playerPos2D       = Vector2.zero;
    [SerializeField] private Vector2 _unitPos2D         = Vector2.zero;

    [Header("===Animator===")]
    [SerializeField] protected UnitAnimationType    _currAniState;
    [SerializeField] protected Animator             _unitAnimator;
    [SerializeField] protected bool _animationEndFlag;

    [Header("===Attack===")]
    [SerializeField] protected List<IAttackStrategy>    _strategyList;
    [SerializeField] private IAttackStrategy            _nowAttack;

    // ������Ƽ
    public UnitState unitState { get => _unitState; set { _unitState = value; } }
    public float unitHp => _unitState.UnitHp;
    public float unitSpeed      => _unitState.UnitSpeed;
    public float searchRadious  => _unitState.SearchRadious;
    public float unitTimeStamp { get => _unitState.UnitTimeStamp; set{ _unitState.UnitTimeStamp = value;} }

    public UNIT_STATE Curr_UNITS_TATE { get => curr_UNITS_TATE; set => curr_UNITS_TATE = value; }
    public UNIT_STATE Pre_UNITS_TATE { get => pre_UNITS_TATE; set => pre_UNITS_TATE = value; }

    private void Start()
    {

    }

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

        // ���ϸ����� ��������
        _unitAnimator = gameObject.GetComponent<Animator>();

        // ������� ���� 
        //curr_UNITS_TATE = UNIT_STATE.Tracking;

        // Machine�� ���� �ֱ� 
        //_UnitHeadMachine.HM_SetState(_UnitStateArr[(int)Curr_UNITS_TATE]);
    }

    // ���� ���� ���� ( OnEnable���� ���� )
    protected void F_CurrStateEnter() 
    {
        // ������� ���� 
        curr_UNITS_TATE = UNIT_STATE.Tracking;

        // Machine�� ���� �ֱ� 
        _UnitHeadMachine.HM_SetState(_UnitStateArr[(int)Curr_UNITS_TATE]);

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
        if (_unitState.UnitHp <= 0)
        {
            // Die�� ���º�ȭ
            F_ChangeState(UNIT_STATE.Die);
        }
    }

    // ��� ���� �����ϸ� ���� ���� �ִ� �ڷ�ƾ�� �������
    public void F_StopColoutine() 
    { 
        StopAllCoroutines();
    }

    // Unit �����ð����� Traking
    public void F_UniTracking(Unit v_unit) 
    {
        /*
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
        */

        StartCoroutine(IE_UnitTracking(v_unit));
    }

    private IEnumerator IE_UnitTracking(Unit v_unit) 
    {
        if (v_unit.gameObject.GetComponent<NavMeshAgent>() == null)
        {
            Debug.LogError("Unit�� agent�� null");
            yield return null;
        }
        else 
        {
            _unitAgent = v_unit.gameObject.GetComponent<NavMeshAgent>();
        }

        // update�� ȿ�� 
        while (true) 
        {
            // ��ó�� navMesh�� ��������
            if (F_CheckIsOnNavMesh()) 
            {
                // marker�� ù��° ��ġ�� ��������
                _destiPosition = PlayerManager.Instance.markerHeadTrasform.position;

                Debug.Log(gameObject.name + "�� ������ + " + _destiPosition);

                // agent�� ������ ����ֱ� 
                _unitAgent.SetDestination(_destiPosition);
            }

            yield return new WaitForSeconds(_navActionCoolDown);
        }
    }

    // marker(�÷��̾�)�� �����ȿ� ������ changeState
    public void F_UpdateSateByDistance(Unit v_unit) 
    {
        _playerPos2D.x = PlayerManager.Instance.markers[0].transform.position.x;
        _playerPos2D.y = PlayerManager.Instance.markers[0].transform.position.z;

        _unitPos2D.x = v_unit.gameObject.transform.position.x;
        _unitPos2D.y = v_unit.gameObject.transform.position.z;

        //Debug.Log(Vector2.Distance(_playerPos2D, _unitPos2D));

        // �Ÿ��� searchRadious���� ������ 
        if (Vector2.Distance(_playerPos2D, _unitPos2D) <= v_unit.searchRadious) 
        {
            F_ChangeState(UNIT_STATE.Attack);
        }
    }

    public void F_GetDamage(float v_damage) 
    {
        // damage��ŭ hp ����
        _unitState.UnitHp -= v_damage;
    }

    public void F_ChageSpeed(float v_speed) 
    {
        // �ӵ� ���� 
        _unitState.UnitSpeed = v_speed;
    }

    // ��ó�� navMesh�� �ִ��� üũ
    private bool F_CheckIsOnNavMesh() 
    {
        NavMeshHit hit;

        // ��ó���� 1.0 �ȿ� navmesh�� �ִ��� 
        if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas)) 
        {
            float _distanceToMesh = Vector3.Distance(transform.position, hit.position);

            // �������� : 0.1f
            if (_distanceToMesh < 0.1f) 
            {
                Debug.Log("Agent�� Navmesh���� �ֽ��ϴ�");
                return true;
            }
            else 
            {
                Debug.Log("Agent�� Navmesh �ۿ� �ֽ��ϴ�.");
                return false;
            }
        }

        // ��ó�� �ƾ� ������ 
        Debug.Log("��ó�� Navmesh�� �����ϴ�.");
        return false;

    }

    // �ִϸ����� - �ĸ������� bool
    public void F_SetAnimatorBoolByState(UnitAnimationType _paramaterName, bool _flag)
    {
        try
        {
            //Debug.Log(_paramaterName + " �� ���º�ȭ : " + _flag);

            // setbool�� true
            _unitAnimator.SetBool(_paramaterName.ToString(), _flag);

            // �ϴ� ����� 
            _currAniState = _paramaterName;

        }
        catch (Exception e) 
        {
            Debug.LogError(e.ToString() + " / " + "Animation Bool �� �������� �ʽ��ϴ� ");
        }
    }

    // �ִϸ����� - �Ķ������ trigger
    public void F_SetAnimatorTriggerByState(UnitAnimationType _paramaterName)
    {
        try
        {
            //Debug.Log(_paramaterName + " �� ���º�ȭ : " + _flag);

            // setbool�� true
            _unitAnimator.SetTrigger(_paramaterName.ToString());

            // �ϴ� ����� 
            _currAniState = _paramaterName;

        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString() + " / " + "Animation Trigger�� �������� �ʽ��ϴ� ");
        }

    }

    // ���� animation �����ϴ��� check
    public void F_UnitAttackAnimationCheck()
    {
        // ex) Pig_BasicAttack �̷������� (State �̸�)
        string _nowAnimationString = unitState.UnitName + "_" + _currAniState.ToString();

        // �ִϸ��̼� ������ flag�� true��
        StartCoroutine(IE_AnimationPlaying(_nowAnimationString));
    }

    private IEnumerator IE_AnimationPlaying(string _aniState) 
    {
        //Debug.Log("�ڷ�ƾ����!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

        // ���� animation������� ���
        yield return new WaitUntil(() => _unitAnimator.GetCurrentAnimatorStateInfo(0).IsName(_aniState));

        //Debug.Log("���� �ִϸ��̼� ����" + _unitAnimator.GetCurrentAnimatorStateInfo(0).length);

        // ���� �ִϸ��̼��� �ð����� 
        float _time = _unitAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(_time);

        // animation �����ϰ� ���� tracking���� ���º�ȭ
        F_ChangeState(UNIT_STATE.Tracking);

        // ����
        yield break;

    }


    // Attack Interface ����Ʈ�ȿ��� �������� idx ��� attack ����
    public void F_AttackExcutor( Unit _unit ) 
    {
        if( _strategyList.Count <= 0 ) 
        {
            Debug.LogError("Attack Interface not implemented ");
            return;
        }

        // ����Ʈ �� ���� �ε��� ���ϱ� 
        int _randIdx = Random.Range(0, _strategyList.Count);

        // �ε����� �ش��ϴ� �������̽� �Լ� ����
        _strategyList[_randIdx].Attack(_unit);

        // ##TODO : �������� �ν����� â���� ��������
        _nowAttack = _strategyList[_randIdx];

    }

}
