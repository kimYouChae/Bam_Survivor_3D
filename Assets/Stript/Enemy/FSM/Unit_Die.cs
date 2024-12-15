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
        _unit.F_SettingCurrState(UNIT_STATE.Die);

        // Die 애니메이션 실행 
        _unit.F_BoolAnimation(UnitAnimationType.Die , true);

        // 애니메이션 체크
        _unit.F_UnitAttackAnimationCheck();
    }

    public override void FSM_Excute()
    {
        // 애니메이션 끝나면 idle로 변경 
        _unit.F_StateByAnimation(UNIT_STATE.Idle);
    }

    public override void FSM_Exit()
    {
        Debug.Log("Die Exit");
        _unit.F_SettingPreState(UNIT_STATE.Die);

        // 유닛 off 
        _unit.F_OffUnit();
    }
}
