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
        _unit.F_SettingCurrState(UNIT_STATE.Tracking);

        // Tracking ���� 
        _unit.F_UniTracking(_unit);

        // Tracking �ִϸ��̼� ����
        //_unit.F_SetAnimatorBoolByState(UnitAnimationType.Tracking, true);
    }   

    public override void FSM_Excute()
    {
        // Unit hp �˻�, 0 ���� �� Die�� ���º�ȭ
        _unit.F_ChekchUnitHp();

        // marker(player)�� ��������� ���º�ȭ
        _unit.F_UpdateStateByDistacne();
    }

    public override void FSM_Exit()
    {
        Debug.Log("Tracking Exit");
        _unit.F_SettingPreState(UNIT_STATE.Tracking);

        // tracking ���� �ڷ�ƾ ����
        _unit.F_StopTrackingCoru();

        // Tracking �ִϸ��̼� ����
        //_unit.F_SetAnimatorBoolByState(UnitAnimationType.Tracking, false);
    }
}
