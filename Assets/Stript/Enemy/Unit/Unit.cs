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
    [SerializeField] protected UnitState _unitState;    // 유닛 클래스
    [SerializeField] protected LifeCycle _lifeCycle;    // 생성 유무 체크

    [Header("===네비게이션 쿨타임===")]
    [SerializeField] const float _navActionCoolDown = 1f;

    [Header("===FSM===")]
    public HeadMachine _UnitHeadMachine;
    public FSM[] _UnitStateArr;
    [SerializeField] protected UNIT_STATE curr_UNITS_TATE;           // 현재 enum
    [SerializeField] protected UNIT_STATE pre_UNITS_TATE;            // 이전 enum 

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

    // 프로퍼티
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

    // FSM 세팅 
    protected void F_InitUnitState( Unit v_standard ) 
    {
        // 헤드머신 생성 ( 자식에서 함수실행 , 자식 본인이 headmachine에 들어감 )
        _UnitHeadMachine = new HeadMachine(v_standard);

        // FSM array 생성
        _UnitStateArr = new FSM[System.Enum.GetValues(typeof(UNIT_STATE)).Length];

        _UnitStateArr[(int)UNIT_STATE.Idle]         = new Unit_Idle(v_standard);
        _UnitStateArr[(int)UNIT_STATE.Tracking]     = new Unit_Tracking(v_standard);
        _UnitStateArr[(int)UNIT_STATE.Attack]       = new Unit_Attack(v_standard);
        _UnitStateArr[(int)UNIT_STATE.Die]          = new Unit_Die(v_standard);

        // 에니메이터 가져오기
        _unitAnimator = gameObject.GetComponent<Animator>();

        // 현재상태 지정 
        //curr_UNITS_TATE = UNIT_STATE.Tracking;

        // Machine에 상태 넣기 
        //_UnitHeadMachine.HM_SetState(_UnitStateArr[(int)Curr_UNITS_TATE]);
    }

    // 현재 상태 진입 ( OnEnable에서 실행 )
    protected void F_CurrStateEnter() 
    {
        // 현재상태 지정 
        curr_UNITS_TATE = UNIT_STATE.Tracking;

        // Machine에 상태 넣기 
        _UnitHeadMachine.HM_SetState(_UnitStateArr[(int)Curr_UNITS_TATE]);

        // head Machine의 enter
        _UnitHeadMachine.HM_StateEnter();
    }
 
    // 현재 상태 실행 ( update에서 실행 )
    protected void F_CurrStateExcute() 
    {
        // head Machine의 excute 
        _UnitHeadMachine.HM_StateExcute();
    }

    public void F_ChangeState( UNIT_STATE v_state ) 
    {
        // UNIT_STATE에 맞는 FSM으로 상태변화 
        // head Machine의 Change 

        _UnitHeadMachine.HM_ChangeState(_UnitStateArr[(int)v_state]);
    }

    // Unit hp 검사
    public void F_ChekchUnitHp() 
    {
        // hp가 0이하면 true 
        if (_unitState.UnitHp <= 0)
        {
            // Die로 상태변화
            F_ChangeState(UNIT_STATE.Die);
        }
    }

    // 모든 상태 시작하면 현재 돌고 있는 코루틴은 멈춰야함
    public void F_StopColoutine() 
    { 
        StopAllCoroutines();
    }

    // Unit 일정시간동안 Traking
    public void F_UniTracking(Unit v_unit) 
    {
        /*
        // 1. 플레이어 추적
        v_unit.gameObject.transform.position
            = Vector3.MoveTowards(v_unit.gameObject.transform.position,
                PlayerManager.instance.markerHeadTrasform.position, v_unit.unitSpeed * Time.deltaTime);

        // 2. 감지범위에 marker 가 검출되면
        Collider[] _coll = Physics.OverlapSphere
            (v_unit.gameObject.transform.position, v_unit.searchRadious, PlayerManager.instance.markerLayer);

        if (_coll.Length > 0)
        {
            // 상태전이
            v_unit.F_ChangeState( UNIT_STATE.Attack );
        }
        */

        StartCoroutine(IE_UnitTracking(v_unit));
    }

    private IEnumerator IE_UnitTracking(Unit v_unit) 
    {
        if (v_unit.gameObject.GetComponent<NavMeshAgent>() == null)
        {
            Debug.LogError("Unit의 agent가 null");
            yield return null;
        }
        else 
        {
            _unitAgent = v_unit.gameObject.GetComponent<NavMeshAgent>();
        }

        // update문 효과 
        while (true) 
        {
            // 근처에 navMesh가 있을때만
            if (F_CheckIsOnNavMesh()) 
            {
                // marker의 첫번째 위치를 목적지고
                _destiPosition = PlayerManager.Instance.markerHeadTrasform.position;

                Debug.Log(gameObject.name + "의 도착지 + " + _destiPosition);

                // agent의 도착지 잡아주기 
                _unitAgent.SetDestination(_destiPosition);
            }

            yield return new WaitForSeconds(_navActionCoolDown);
        }
    }

    // marker(플레이어)가 범위안에 들어오면 changeState
    public void F_UpdateSateByDistance(Unit v_unit) 
    {
        _playerPos2D.x = PlayerManager.Instance.markers[0].transform.position.x;
        _playerPos2D.y = PlayerManager.Instance.markers[0].transform.position.z;

        _unitPos2D.x = v_unit.gameObject.transform.position.x;
        _unitPos2D.y = v_unit.gameObject.transform.position.z;

        //Debug.Log(Vector2.Distance(_playerPos2D, _unitPos2D));

        // 거리가 searchRadious보다 작으면 
        if (Vector2.Distance(_playerPos2D, _unitPos2D) <= v_unit.searchRadious) 
        {
            F_ChangeState(UNIT_STATE.Attack);
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

    // 근처에 navMesh가 있는지 체크
    private bool F_CheckIsOnNavMesh() 
    {
        NavMeshHit hit;

        // 근처에서 1.0 안에 navmesh가 있는지 
        if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas)) 
        {
            float _distanceToMesh = Vector3.Distance(transform.position, hit.position);

            // 범위오차 : 0.1f
            if (_distanceToMesh < 0.1f) 
            {
                Debug.Log("Agent가 Navmesh위에 있습니다");
                return true;
            }
            else 
            {
                Debug.Log("Agent가 Navmesh 밖에 있습니다.");
                return false;
            }
        }

        // 근처에 아얘 없으면 
        Debug.Log("근처에 Navmesh가 없습니다.");
        return false;

    }

    // 애니메이터 - 파리미터의 bool
    public void F_SetAnimatorBoolByState(UnitAnimationType _paramaterName, bool _flag)
    {
        try
        {
            //Debug.Log(_paramaterName + " 의 상태변화 : " + _flag);

            // setbool을 true
            _unitAnimator.SetBool(_paramaterName.ToString(), _flag);

            // 일단 보기용 
            _currAniState = _paramaterName;

        }
        catch (Exception e) 
        {
            Debug.LogError(e.ToString() + " / " + "Animation Bool 이 존재하지 않습니다 ");
        }
    }

    // 애니메이터 - 파라미터의 trigger
    public void F_SetAnimatorTriggerByState(UnitAnimationType _paramaterName)
    {
        try
        {
            //Debug.Log(_paramaterName + " 의 상태변화 : " + _flag);

            // setbool을 true
            _unitAnimator.SetTrigger(_paramaterName.ToString());

            // 일단 보기용 
            _currAniState = _paramaterName;

        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString() + " / " + "Animation Trigger가 존재하지 않습니다 ");
        }

    }

    // 현재 animation 실행하는지 check
    public void F_UnitAttackAnimationCheck()
    {
        // ex) Pig_BasicAttack 이런식으로 (State 이름)
        string _nowAnimationString = unitState.UnitName + "_" + _currAniState.ToString();

        // 애니메이션 끝나면 flag를 true로
        StartCoroutine(IE_AnimationPlaying(_nowAnimationString));
    }

    private IEnumerator IE_AnimationPlaying(string _aniState) 
    {
        //Debug.Log("코루틴실행!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

        // 현재 animation실행까지 대기
        yield return new WaitUntil(() => _unitAnimator.GetCurrentAnimatorStateInfo(0).IsName(_aniState));

        //Debug.Log("현재 애니메이션 길이" + _unitAnimator.GetCurrentAnimatorStateInfo(0).length);

        // 현재 애니메이션의 시간까지 
        float _time = _unitAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(_time);

        // animation 실행하고 나서 tracking으로 상태변화
        F_ChangeState(UNIT_STATE.Tracking);

        // 종료
        yield break;

    }


    // Attack Interface 리스트안에서 랜덤으로 idx 골라서 attack 실행
    public void F_AttackExcutor( Unit _unit ) 
    {
        if( _strategyList.Count <= 0 ) 
        {
            Debug.LogError("Attack Interface not implemented ");
            return;
        }

        // 리스트 내 랜덤 인덱스 구하기 
        int _randIdx = Random.Range(0, _strategyList.Count);

        // 인덱스에 해당하는 인터페이스 함수 실행
        _strategyList[_randIdx].Attack(_unit);

        // ##TODO : 지워도됨 인스펙터 창에서 보기위한
        _nowAttack = _strategyList[_randIdx];

    }

}
