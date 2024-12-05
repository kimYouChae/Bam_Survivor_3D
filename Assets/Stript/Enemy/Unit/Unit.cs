using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{
    [Header("===Uint State===")]
    [SerializeField] protected UnitState _unitState;    // ���� Ŭ����
    [SerializeField] protected LifeCycle _lifeCycle;    // ���� ���� üũ

    ITrackingHandler _trackingHandler;
    IAttackHandler _attackHandler;
    FSMHandler _FSMHandler;
    UnitAnimationHandler _animHandler;

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

#region FSM HANDLER

    // Unit State ���� -> FSMHandler�� ����
    public void F_ChangeState(UNIT_STATE _State)
    {
        _FSMHandler.FH_ChangeState(_State);
    }

    // ���� ���� ���� -> FSMHandler�� ����
    public void F_StateEnter() 
    {
        _FSMHandler.FH_CurrStateEnter();
    }

    // ���� ���� ���� -> FSMHandler�� ����
    public void F_StateExcute() 
    {
        _FSMHandler.FH_CurrStateExcute();
    }

    // ���� ���� Setting -> FSMHandler�� ����
    public void F_SettingState(UNIT_STATE _State) 
    {
        _FSMHandler.FH_SettingState(_State);
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
        // TODO : ���ư����� �׽�Ʈ�ؾ��� 
        StartCoroutine(_trackingHandler.IE_TrackinCorutine());
    }
#endregion

#region ATTACK HANDLER
    public void F_AddToAttackStrtegy(IAttackStrategy _attack) 
    {
        _attackHandler.AH_AddAttackList(_attack);
    }
#endregion

}
