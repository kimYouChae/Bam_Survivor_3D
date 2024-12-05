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

        // Attack ���� �Լ� ����
        _unit.F_AttackExcutor();

        // ���� �ִϸ����� �˻� 
        //_unit.F_UnitAttackAnimationCheck();
    }

    public override void FSM_Excute()
    {
        // Unit hp �˻�, 0 ���� �� Die�� ���º�ȭ
        _unit.F_ChekchUnitHp();
    }

    public override void FSM_Exit()
    {
        Debug.Log("Attack exit");
        _unit.F_SettingPreState(UNIT_STATE.Attack);
    }


}
