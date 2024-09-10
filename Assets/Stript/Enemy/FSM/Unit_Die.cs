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
        _unit._curr_UNITS_TATE = UNIT_STATE.Die;

        // ²ô±â 
        _unit.gameObject.SetActive(false);
    }

    public override void FSM_Excute()
    {
        
    }

    public override void FSM_Exit()
    {
        Debug.Log("Die Exit");
        _unit._curr_UNITS_TATE = UNIT_STATE.Die;
    }
}
