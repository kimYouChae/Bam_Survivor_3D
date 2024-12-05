using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMHandler 
{
    [Header("===FSM===")]
    public HeadMachine _UnitHeadMachine;
    public FSM[] _UnitStateArr;
    [SerializeField] protected UNIT_STATE curr_UNITS_TATE;           // 현재 enum
    [SerializeField] protected UNIT_STATE pre_UNITS_TATE;            // 이전 enum 

    public UNIT_STATE Curr_UNITS_TATE { get => curr_UNITS_TATE; set => curr_UNITS_TATE = value; }
    public UNIT_STATE Pre_UNITS_TATE { get => pre_UNITS_TATE; set => pre_UNITS_TATE = value; }

    // 생성자
    public FSMHandler(Unit _unit) 
    {
        FH_InitUnitState(_unit);
    }

    // FSM 세팅 
    protected void FH_InitUnitState(Unit v_standard)
    {
        // 헤드머신 생성 ( 자식에서 함수실행 , 자식 본인이 headmachine에 들어감 )
        _UnitHeadMachine = new HeadMachine(v_standard);

        // FSM array 생성
        _UnitStateArr = new FSM[System.Enum.GetValues(typeof(UNIT_STATE)).Length];

        _UnitStateArr[(int)UNIT_STATE.Idle] = new Unit_Idle(v_standard);
        _UnitStateArr[(int)UNIT_STATE.Tracking] = new Unit_Tracking(v_standard);
        _UnitStateArr[(int)UNIT_STATE.Attack] = new Unit_Attack(v_standard);
        _UnitStateArr[(int)UNIT_STATE.Die] = new Unit_Die(v_standard);

        // 현재상태 지정 
        //curr_UNITS_TATE = UNIT_STATE.Tracking;

        // Machine에 상태 넣기 
        //_UnitHeadMachine.HM_SetState(_UnitStateArr[(int)Curr_UNITS_TATE]);
    }

    // 현재 상태 Setting
    public void FH_SettingState(UNIT_STATE _state) 
    {
        // 현재상태 지정 
        curr_UNITS_TATE = _state;
    }

    // 현재 상태 진입 ( OnEnable에서 실행 )
    public void FH_CurrStateEnter()
    {
        // Machine에 상태 넣기 
        _UnitHeadMachine.HM_SetState(_UnitStateArr[(int)Curr_UNITS_TATE]);

        // head Machine의 enter
        _UnitHeadMachine.HM_StateEnter();
    }

    // 현재 상태 실행 ( update에서 실행 )
    public void FH_CurrStateExcute()
    {
        // head Machine의 excute 
        _UnitHeadMachine.HM_StateExcute();
    }

    public void FH_ChangeState(UNIT_STATE v_state)
    {
        // UNIT_STATE에 맞는 FSM으로 상태변화 
        // head Machine의 Change 

        _UnitHeadMachine.HM_ChangeState(_UnitStateArr[(int)v_state]);
    }
}
