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
        _unit._curr_UNITS_TATE = UNIT_STATE.Tracking;
    }   

    public override void FSM_Excute()
    {
        // Unit hp 검사, 0 이하 시 Die로 상태변화
        _unit.F_ChekchUnitHp();

        // Tracking 동작 
        _unit.F_UniTracking(_unit);
    }

    public override void FSM_Exit()
    {
        Debug.Log("Tracking Exit");
        _unit._pre_UNITS_TATE = UNIT_STATE.Tracking;
    }
}
