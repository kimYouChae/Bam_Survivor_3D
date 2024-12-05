using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{
    [Header("===Uint State===")]
    [SerializeField] protected UnitState _unitState;    // ���� Ŭ����
    [SerializeField] protected LifeCycle _lifeCycle;    // ���� ���� üũ

    [Header("===Handler===")]
    [SerializeField] ITrackingHandler       _trackingHandler;
    [SerializeField] IAttackHandler         _attackHandler;
    [SerializeField] FSMHandler             _FSMHandler;
    [SerializeField] UnitAnimationHandler   _animHandler;

    // ������Ƽ
    public UnitState unitState { get => _unitState; set { _unitState = value; } }
    public float unitHp => _unitState.UnitHp;
    public float unitSpeed      => _unitState.UnitSpeed;
    public float unitSearchRadious  => _unitState.SearchRadious;
    public string unitName => _unitState.UnitName;
    public float unitTimeStamp { get => _unitState.UnitTimeStamp; set{ _unitState.UnitTimeStamp = value;} }

#region Init

    // �ʱ� 1ȸ�� �ʱ�ȭ �ؾ��ϴ°�
    public void F_InitHandlerSetting() 
    {
        // �ڵ鷯 setting
        _trackingHandler = new TrackingHanlder(this);
        _FSMHandler = new FSMHandler(this);
        _attackHandler = new AttackHandler(this);
        _animHandler = new UnitAnimationHandler(this);
    }
#endregion

#region FSM HANDLER ->  FSMHandler�� ����

    // Unit State ���� 
    public void F_ChangeState(UNIT_STATE _State)
    {
        _FSMHandler.FH_ChangeState(_State);
    }

    // ���� ���� ���� 
    public void F_StateEnter() 
    {
        _FSMHandler.FH_CurrStateEnter();
    }

    // ���� ���� ���� 
    public void F_StateExcute() 
    {
        _FSMHandler.FH_CurrStateExcute();
    }

    // ���� ���� Setting ->
    public void F_SettingCurrState(UNIT_STATE _State) 
    {
        _FSMHandler.FH_SettingCurrState(_State);
    }

    // ���� ���� Setting 
    public void F_SettingPreState(UNIT_STATE _state) 
    {
        _FSMHandler.FH_SettingPreState(_state);
    }

#endregion

    #region State

    // Unit hp �˻� -> FSMHandler�� ����
    public void F_ChekchUnitHp() 
    {
        // hp�� 0���ϸ� true 
        if (_unitState.UnitHp <= 0)
        {
            // Die�� ���º�ȭ
            F_ChangeState(UNIT_STATE.Die);
        }
    }

    public void F_GetDamage(float v_damage) 
    {
        // damage��ŭ hp ����
        _unitState.UnitHp -= v_damage;
    }

    public void F_ChageSpeed(float v_speed) 
    {
        // �ӵ� ���� 
        _unitState.UnitSpeed = v_speed;
    }
#endregion

#region TRAKING HANDLER

    // Unit �����ð����� Traking
    public void F_UniTracking(Unit v_unit) 
    {
        StartCoroutine(_trackingHandler.IE_TrackinCorutine());
    }

    // Tracking end �� ���� 
    public void F_StopTrackingCoru() 
    {
        StopCoroutine(_trackingHandler.IE_TrackinCorutine());
    }
    public void F_UpdateStateByDistacne() 
    {
        _trackingHandler.TH_EvaluateStateTransition();
    }
#endregion

#region ATTACK HANDLER
    public void F_AddToAttackStrtegy(IAttackStrategy _attack) 
    {
        _attackHandler.AH_AddAttackList(_attack);
    }

    public void F_AttackExcutor() 
    {
        _attackHandler.AH_AttackExcutor();
    }
#endregion

}
