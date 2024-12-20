using System;
using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;

public abstract class Unit : MonoBehaviour
{
    [Header("===Uint State===")]
    [SerializeField] protected UnitState _unitState;    // 유닛 클래스
    [SerializeField] protected LifeCycle _lifeCycle;    // 생성 유무 체크

    [Header("===Component")]
    [SerializeField] protected NavMeshAgent _unitAgent;     
    [SerializeField] protected Animator _unitAnimator;      
    [SerializeField] private Transform _hitPosition;        // 히트 위치

    [Header("===Handler===")]
    [SerializeField] ITrackingHandler               _trackingHandler;   // tracking 핸들러
    [SerializeField] IAttackHandler                 _attackHandler;     // 공격 핸들러 
    [SerializeField] FSMHandler                     _FSMHandler;
    [SerializeField] UnitAnimationHandler           _animHandler;

    // 프로퍼티
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

    // 초기 1회때 초기화 해야하는것
    public void F_InitHandlerSetting()
    {
        // navmesh Init
        F_NavmeshInit();

        // animator Init
        F_AnimatorInit();

        // 핸들러 setting
        _trackingHandler = new TrackingHanlder(this);
        _FSMHandler = new FSMHandler(this);
        _attackHandler = new AttackHandler(this);
        _animHandler = new UnitAnimationHandler(this);

        // 히트포지션 -> 하위 첫번째 자식으로 
        _hitPosition = gameObject.transform.GetChild(0);
    }

    // 공격 animationType을 return 
    public abstract UnitAnimationType F_returnAttackType();

    #endregion

    #region NavMesh
    public void F_NavmeshInit()
    {
        // navmesh 세팅 
        if (gameObject.GetComponent<NavMeshAgent>() == null)
            Debug.LogError("Unit의 agent가 null");

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

    #region FSM HANDLER ->  FSMHandler에 접근

    // Unit State 변경 
    public void F_ChangeState(UNIT_STATE _State)
    {
        _FSMHandler.FH_ChangeState(_State);
    }

    // 현재 상태 진입 
    public void F_StateEnter() 
    {
        _FSMHandler.FH_CurrStateEnter();
    }

    // 현재 상태 실행 
    public void F_StateExcute() 
    {
        _FSMHandler.FH_CurrStateExcute();
    }

    // 현재 상태 Setting ->
    public void F_SettingCurrState(UNIT_STATE _State) 
    {
        _FSMHandler.FH_SettingCurrState(_State);
    }

    // 이전 상태 Setting 
    public void F_SettingPreState(UNIT_STATE _state) 
    {
        _FSMHandler.FH_SettingPreState(_state);
    }

    #endregion

    #region State

    // Unit hp 검사 -> FSMHandler에 접근
    public void F_ChekchUnitHp() 
    {
        // hp가 0이하면 true 
        if (_unitState.UnitHp <= 0)
        {
            // Die로 상태변화
            F_ChangeState(UNIT_STATE.Die);
        }
    }

    public void F_GetDamage(float v_damage) 
    {
        // damage만큼 hp 감소
        _unitState.UnitHp -= v_damage;
    }

    public void F_ChageSpeed(float v_speed) 
    {
        // 속도 변경 
        _unitState.UnitSpeed = v_speed;
    }
    #endregion

    #region TRAKING HANDLER

    // Unit 일정시간동안 Traking
    public void F_UniTracking(Unit v_unit) 
    {
        StartCoroutine(_trackingHandler.IE_TrackinCorutine());
    }

    // Tracking end 시 실행 
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
        // Animator 세팅 
        if (gameObject.GetComponent<Animator>() == null)
            Debug.LogError("Unit의 Animator가 null");

        else
            _unitAnimator = gameObject.GetComponent<Animator>();
    }

    // trigger
    public void F_TriggerAnimation(UnitAnimationType _type) 
    {
        // trgger 함수 실행
        _animHandler.F_SetAnimatorTriggerByState(_type);
    }

    // bool
    public void F_BoolAnimation(UnitAnimationType _type , bool _flag) 
    {
        // bool 함수 실행 
        _animHandler.F_SetAnimatorBoolByState(_type, _flag);
    }

    // attack의 Enter에서 실행
    public void F_UnitAttackAnimationCheck()
    {
        // 현재 animation 실행하는지 check
        StartCoroutine(_animHandler.IE_AnimationPlaying());
    }

    // 애니메이션이 끝났으면 -> 상태변화
    public void F_StateByAnimation(UNIT_STATE _state) 
    {
        // 애니메이션이 끝낫으면 
        if (_animHandler.AnimationEndFlag) 
        {
            F_ChangeState(_state);        
        }
    }

    #endregion

    #region DIE
    public void F_OffUnit() 
    {
        // 경험치 생성
        F_DistrubutionExperience();

        // pool에 넣기 
        UnitManager.Instance.UnitPooling.F_SetUnit(this, unitState.AnimalType);
    }

    private void F_DistrubutionExperience() 
    {
        // 경험치 갯수
        // StageIndex ~ StageIndex * 2

        int _currState = StageManager.Instance.currStageIndex;
        int _exCnt = Random.Range( Math.Max(1, _currState) , _currState * 2 + 1 );

        Debug.Log(":::::::::::::::" + _exCnt);

        float x = 0;
        float y = 0;

        for (int i = 0; i < _exCnt; i++) 
        {
            GameObject _ex = PoolingManager.Instance.experiencePooling.F_GetExperience();

            _ex.gameObject.name = "이거슨경험치";

            x = gameObject.transform.position.x + Random.Range(0f , 1f);
            y = gameObject.transform.position.z + Random.Range(0f , 1f);

            _ex.transform.position = new Vector3(x, 0.5f ,y);
        }
    }

    #endregion

    #region OnDisable
    public void F_OnDisable() 
    {
        // HP 세팅
        _unitState.UnitHp = _unitState.UnitMaxHp;

        // 애니메이터 세팅 > paremeter 초기화 
        _unitAnimator.Rebind();

    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position , unitSearchRadious);
    }

}
