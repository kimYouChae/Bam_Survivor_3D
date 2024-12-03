using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Tracking : FSM
{
    private Unit _unit;

    public Unit_Tracking(Unit v_unit)
    {
        this._unit = v_unit;
    }

    public override void FSM_Enter()
    {
        Debug.Log("Tracking Enter");
        _unit.Curr_UNITS_TATE = UNIT_STATE.Tracking;

        // Tracking 동작 
        _unit.F_UniTracking(_unit);

        // Tracking 애니메이션 실행
        _unit.F_SetAnimatorBoolByState(UnitAnimationType.Tracking, true);
    }   

    public override void FSM_Excute()
    {
        // Unit hp 검사, 0 이하 시 Die로 상태변화
        _unit.F_ChekchUnitHp();

        // marker(player)와 가까워지면 상태변화
        _unit.F_UpdateSateByDistance(_unit);
    }

    public override void FSM_Exit()
    {
        Debug.Log("Tracking Exit");
        _unit.Pre_UNITS_TATE = UNIT_STATE.Tracking;

        // Tracking 애니메이션 종료
        _unit.F_SetAnimatorBoolByState(UnitAnimationType.Tracking, false);
    }
}
