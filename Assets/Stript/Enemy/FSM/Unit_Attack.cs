using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Attack : FSM
{
    private Unit _unit;

    public Unit_Attack(Unit v_unit)
    {
        this._unit = v_unit;
    }

    public override void FSM_Enter()
    {
        Debug.Log("Attack Enter");
        _unit.F_SettingCurrState(UNIT_STATE.Attack);

        // Attack 공격 함수 실행
        _unit.F_AttackExcutor();

        // 공격 애니메이터 검사 
        //_unit.F_UnitAttackAnimationCheck();
    }

    public override void FSM_Excute()
    {
        // Unit hp 검사, 0 이하 시 Die로 상태변화
        _unit.F_ChekchUnitHp();
    }

    public override void FSM_Exit()
    {
        Debug.Log("Attack exit");
        _unit.F_SettingPreState(UNIT_STATE.Attack);
    }


}
