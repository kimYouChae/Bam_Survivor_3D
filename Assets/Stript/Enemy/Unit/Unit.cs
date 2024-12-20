using System;
using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;

public abstract class Unit : MonoBehaviour
{
    [Header("===Uint State===")]
    [SerializeField] protected UnitState _unitState;    // ���� Ŭ����
    [SerializeField] protected LifeCycle _lifeCycle;    // ���� ���� üũ

    [Header("===Component")]
    [SerializeField] protected NavMeshAgent _unitAgent;     
    [SerializeField] protected Animator _unitAnimator;      
    [SerializeField] private Transform _hitPosition;        // ��Ʈ ��ġ

    [Header("===Handler===")]
    [SerializeField] ITrackingHandler               _trackingHandler;   // tracking �ڵ鷯
    [SerializeField] IAttackHandler                 _attackHandler;     // ���� �ڵ鷯 
    [SerializeField] FSMHandler                     _FSMHandler;
    [SerializeField] UnitAnimationHandler           _animHandler;

    // ������Ƽ
    public UnitState unitState { get => _unitState; set { _unitState = value; } }
    public float unitHp { get=> _unitState.UnitHp; set { value = _unitState.UnitHp; } }
    
    public float unitSpeed => _unitState.UnitSpeed;
    public float unitSearchRadious => _unitState.SearchRadious;
    public string unitName => _unitState.UnitName;
    public float unitDamage => _unitState.UnitDamage;
    public float unitTimeStamp { get => _unitState.UnitTimeStamp; set { _unitState.UnitTimeStamp = value; } }

    public NavMeshAgent unitAgent => _unitAgent;
    public Transform hitPosition => _hitPosition;

    public virtual void F_BossChangeSpeed() { }

    #region Init

    // �ʱ� 1ȸ�� �ʱ�ȭ �ؾ��ϴ°�
    public void F_InitHandlerSetting()
    {
        // navmesh Init
        F_NavmeshInit();

        // animator Init
        F_AnimatorInit();

        // �ڵ鷯 setting
        _trackingHandler = new TrackingHanlder(this);
        _FSMHandler = new FSMHandler(this);
        _attackHandler = new AttackHandler(this);
        _animHandler = new UnitAnimationHandler(this);

        // ��Ʈ������ -> ���� ù��° �ڽ����� 
        _hitPosition = gameObject.transform.GetChild(0);
    }

    // ���� animationType�� return 
    public abstract UnitAnimationType F_returnAttackType();

    #endregion

    #region NavMesh
    public void F_NavmeshInit()
    {
        // navmesh ���� 
        if (gameObject.GetComponent<NavMeshAgent>() == null)
            Debug.LogError("Unit�� agent�� null");

        else
            _unitAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    // agent On
    public void F_NavmeshOnOff(bool _flag) 
    {
        if (_unitAgent.enabled != _flag)
        {
            _unitAgent.enabled = _flag;
        }
    }

    // Destination Set
    public void F_SetDestinationToHead() 
    {
        _unitAgent.SetDestination(PlayerManager.Instance.markerHeadTrasform.position);
    }

    #endregion

    #region FSM HANDLER ->  FSMHandler�� ����

    // Unit State ���� 
    public void F_ChangeState(UNIT_STATE _State)
    {
        _FSMHandler.FH_ChangeState(_State);
    }

    // ���� ���� ���� 
    public void F_StateEnter() 
    {
        _FSMHandler.FH_CurrStateEnter();
    }

    // ���� ���� ���� 
    public void F_StateExcute() 
    {
        _FSMHandler.FH_CurrStateExcute();
    }

    // ���� ���� Setting ->
    public void F_SettingCurrState(UNIT_STATE _State) 
    {
        _FSMHandler.FH_SettingCurrState(_State);
    }

    // ���� ���� Setting 
    public void F_SettingPreState(UNIT_STATE _state) 
    {
        _FSMHandler.FH_SettingPreState(_state);
    }

    #endregion

    #region State

    // Unit hp �˻� -> FSMHandler�� ����
    public void F_ChekchUnitHp() 
    {
        // hp�� 0���ϸ� true 
        if (_unitState.UnitHp <= 0)
        {
            // Die�� ���º�ȭ
            F_ChangeState(UNIT_STATE.Die);
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
    #endregion

    #region TRAKING HANDLER

    // Unit �����ð����� Traking
    public void F_UniTracking(Unit v_unit) 
    {
        StartCoroutine(_trackingHandler.IE_TrackinCorutine());
    }

    // Tracking end �� ���� 
    public void F_StopTrackingCoru() 
    {
        StopAllCoroutines();
    }
    public void F_UpdateStateByDistacne() 
    {
        _trackingHandler.TH_EvaluateStateTransition();
    }
    #endregion

    #region ATTACK HANDLER
    public void F_AddToAttackStrtegy(UnitAnimationType _type, AttackStrategy _attack) 
    {
        _attackHandler.AH_AddAttackList(_type , _attack);
    }

    public void F_AttackExcutor() 
    {
        _attackHandler.AH_AttackExcutor();
    }
    #endregion

    #region Animator HANDLER 

    private void F_AnimatorInit() 
    {
        // Animator ���� 
        if (gameObject.GetComponent<Animator>() == null)
            Debug.LogError("Unit�� Animator�� null");

        else
            _unitAnimator = gameObject.GetComponent<Animator>();
    }

    // trigger
    public void F_TriggerAnimation(UnitAnimationType _type) 
    {
        // trgger �Լ� ����
        _animHandler.F_SetAnimatorTriggerByState(_type);
    }

    // bool
    public void F_BoolAnimation(UnitAnimationType _type , bool _flag) 
    {
        // bool �Լ� ���� 
        _animHandler.F_SetAnimatorBoolByState(_type, _flag);
    }

    // attack�� Enter���� ����
    public void F_UnitAttackAnimationCheck()
    {
        // ���� animation �����ϴ��� check
        StartCoroutine(_animHandler.IE_AnimationPlaying());
    }

    // �ִϸ��̼��� �������� -> ���º�ȭ
    public void F_StateByAnimation(UNIT_STATE _state) 
    {
        // �ִϸ��̼��� �������� 
        if (_animHandler.AnimationEndFlag) 
        {
            F_ChangeState(_state);        
        }
    }

    #endregion

    #region DIE
    public void F_OffUnit() 
    {
        // ����ġ ����
        F_DistrubutionExperience();

        // pool�� �ֱ� 
        UnitManager.Instance.UnitPooling.F_SetUnit(this, unitState.AnimalType);
    }

    private void F_DistrubutionExperience() 
    {
        // ����ġ ����
        // StageIndex ~ StageIndex * 2

        int _currState = StageManager.Instance.currStageIndex;
        int _exCnt = Random.Range( Math.Max(1, _currState) , _currState * 2 + 1 );

        Debug.Log(":::::::::::::::" + _exCnt);

        float x = 0;
        float y = 0;

        for (int i = 0; i < _exCnt; i++) 
        {
            GameObject _ex = PoolingManager.Instance.experiencePooling.F_GetExperience();

            _ex.gameObject.name = "�̰Ž�����ġ";

            x = gameObject.transform.position.x + Random.Range(0f , 1f);
            y = gameObject.transform.position.z + Random.Range(0f , 1f);

            _ex.transform.position = new Vector3(x, 0.5f ,y);
        }
    }

    #endregion

    #region OnDisable
    public void F_OnDisable() 
    {
        // HP ����
        _unitState.UnitHp = _unitState.UnitMaxHp;

        // �ִϸ����� ���� > paremeter �ʱ�ȭ 
        _unitAnimator.Rebind();

    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position , unitSearchRadious);
    }

}
