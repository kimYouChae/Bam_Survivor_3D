using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMHandler 
{
    [Header("===FSM===")]
    public HeadMachine _UnitHeadMachine;
    public FSM[] _UnitStateArr;
    [SerializeField] protected UNIT_STATE curr_UNITS_TATE;           // ���� enum
    [SerializeField] protected UNIT_STATE pre_UNITS_TATE;            // ���� enum 

    public UNIT_STATE Curr_UNITS_TATE { get => curr_UNITS_TATE; set => curr_UNITS_TATE = value; }
    public UNIT_STATE Pre_UNITS_TATE { get => pre_UNITS_TATE; set => pre_UNITS_TATE = value; }

    // ������
    public FSMHandler(Unit _unit) 
    {
        FH_InitUnitState(_unit);
    }

    // FSM ���� 
    protected void FH_InitUnitState(Unit v_standard)
    {
        // ���ӽ� ���� ( �ڽĿ��� �Լ����� , �ڽ� ������ headmachine�� �� )
        _UnitHeadMachine = new HeadMachine(v_standard);

        // FSM array ����
        _UnitStateArr = new FSM[System.Enum.GetValues(typeof(UNIT_STATE)).Length];

        _UnitStateArr[(int)UNIT_STATE.Idle] = new Unit_Idle(v_standard);
        _UnitStateArr[(int)UNIT_STATE.Tracking] = new Unit_Tracking(v_standard);
        _UnitStateArr[(int)UNIT_STATE.Attack] = new Unit_Attack(v_standard);
        _UnitStateArr[(int)UNIT_STATE.Die] = new Unit_Die(v_standard);

        // ������� ���� 
        //curr_UNITS_TATE = UNIT_STATE.Tracking;

        // Machine�� ���� �ֱ� 
        //_UnitHeadMachine.HM_SetState(_UnitStateArr[(int)Curr_UNITS_TATE]);
    }

    // ���� ���� Setting
    public void FH_SettingState(UNIT_STATE _state) 
    {
        // ������� ���� 
        curr_UNITS_TATE = _state;
    }

    // ���� ���� ���� ( OnEnable���� ���� )
    public void FH_CurrStateEnter()
    {
        // Machine�� ���� �ֱ� 
        _UnitHeadMachine.HM_SetState(_UnitStateArr[(int)Curr_UNITS_TATE]);

        // head Machine�� enter
        _UnitHeadMachine.HM_StateEnter();
    }

    // ���� ���� ���� ( update���� ���� )
    public void FH_CurrStateExcute()
    {
        // head Machine�� excute 
        _UnitHeadMachine.HM_StateExcute();
    }

    public void FH_ChangeState(UNIT_STATE v_state)
    {
        // UNIT_STATE�� �´� FSM���� ���º�ȭ 
        // head Machine�� Change 

        _UnitHeadMachine.HM_ChangeState(_UnitStateArr[(int)v_state]);
    }
}
