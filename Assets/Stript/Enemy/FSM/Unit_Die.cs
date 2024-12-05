using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Die : FSM
{
    private Unit _unit;

    public Unit_Die(Unit v_unit)
    {
        this._unit = v_unit;
    }

    public override void FSM_Enter()
    {
        Debug.Log("Die Enter");
        //_unit.Curr_UNITS_TATE = UNIT_STATE.Die;

        // Die 애니메이션 끝나고
        
        // pool에 넣기 
    

    }

    public override void FSM_Excute()
    {
        
    }

    public override void FSM_Exit()
    {
        Debug.Log("Die Exit");
        //_unit.Curr_UNITS_TATE = UNIT_STATE.Die;
    }
}
