using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    /// <summary>
    /// Unit들의 부모 
    /// 
    /// 1. hp
    /// 2. 이동속도
    /// 3. abstract 공격
    /// 
    /// </summary>

    [Header("===Uint State===")]
    [SerializeField] protected float   _unitHp;           // hp
    [SerializeField] protected float _unitSpeed;        // speed 
    [SerializeField] protected float _unitAttackTime;   // 공격 지속시간
    [SerializeField] protected float _unitTimeStamp;   // 공격 시 0으로 초기화                                                 
    [SerializeField] protected float _searchRadious;       // 플레이어 감지 범위 

    [SerializeField] const float _navActionCoolDown = 1f;

    [Header("===FSM===")]
    public HeadMachine _UnitHeadMachine;
    public FSM[] _UnitStateArr;
    [SerializeField] public UNIT_STATE _curr_UNITS_TATE;           // 현재 enum
    [SerializeField] public UNIT_STATE _pre_UNITS_TATE;           // 이전 enum 


    [Header("===LayerMask===")]
    public LayerMask _hitWallLayerMask;

    // 프로퍼티
    public float unitHp => _unitHp;
    public float unitSpeed      => _unitSpeed;
    public float searchRadious  => _searchRadious;
    public float unitTimeStamp { get => _unitTimeStamp; set{ _unitTimeStamp = value;} }

    private void Start()
    {
        _hitWallLayerMask = LayerMask.GetMask("Wall");
    }

    // 상태 초기화
    // ##TODO CVS로 데이터 관리 시 수정필요함
    protected virtual void F_InitUnitUnitState() { }

    // attack 동작 재정의
    public virtual void F_UnitAttatk() { }

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

        // 현재상태 지정 
        _curr_UNITS_TATE = UNIT_STATE.Tracking;

        // Machine에 상태 넣기 
        _UnitHeadMachine.HM_SetState(_UnitStateArr[(int)_curr_UNITS_TATE]);
    }

    // 현재 상태 진입 ( 1회 , Start에서 실행 )
    protected void F_CurrStateEnter() 
    {
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
        if (_unitHp <= 0)
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
        Vector3 _destiPosition;
        NavMeshAgent _unitAgent = null;

        if (v_unit.gameObject.GetComponent<NavMeshAgent>() == null)
        {
            Debug.LogError("Unit의 agane가 null");
            yield return null;
        }
        else 
        {
            _unitAgent = v_unit.gameObject.GetComponent<NavMeshAgent>();
        }

        // update문 효과 
        while (true) 
        {
            // marker의 첫번째 위치를 목적지고
            _destiPosition = PlayerManager.instance.markers[0].transform.position;

            // agent의 도착지 잡아주기 
            _unitAgent.SetDestination( _destiPosition );

            yield return new WaitForSeconds(_navActionCoolDown);
        }
    }

    // marker(플레이어)가 범위안에 들어오면 changeState
    public void F_UpdateSateByDistance(Unit v_unit) 
    {
        Vector2 _playerPos2D = new Vector2(PlayerManager.instance.markers[0].transform.position.x, PlayerManager.instance.markers[0].transform.position.z);
        Vector2 _unitPos2D = new Vector2(v_unit.gameObject.transform.position.x, v_unit.gameObject.transform.position.z);

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
        _unitHp -= v_damage;
    }

    public void F_ChageSpeed(float v_speed) 
    {
        // 속도 변경 
        _unitSpeed = v_speed;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Unit"))
        {
            Debug.Log("총알이랑 충돌");
        }                   
    }
}
