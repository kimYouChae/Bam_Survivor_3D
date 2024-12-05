using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{
    [Header("===Uint State===")]
    [SerializeField] protected UnitState _unitState;    // 유닛 클래스
    [SerializeField] protected LifeCycle _lifeCycle;    // 생성 유무 체크

    [Header("===Handler===")]
    [SerializeField] ITrackingHandler       _trackingHandler;
    [SerializeField] IAttackHandler         _attackHandler;
    [SerializeField] FSMHandler             _FSMHandler;
    [SerializeField] UnitAnimationHandler   _animHandler;

    // 프로퍼티
    public UnitState unitState { get => _unitState; set { _unitState = value; } }
    public float unitHp => _unitState.UnitHp;
    public float unitSpeed      => _unitState.UnitSpeed;
    public float unitSearchRadious  => _unitState.SearchRadious;
    public string unitName => _unitState.UnitName;
    public float unitTimeStamp { get => _unitState.UnitTimeStamp; set{ _unitState.UnitTimeStamp = value;} }

#region Init

    // 초기 1회때 초기화 해야하는것
    public void F_InitHandlerSetting() 
    {
        // 핸들러 setting
        _trackingHandler = new TrackingHanlder(this);
        _FSMHandler = new FSMHandler(this);
        _attackHandler = new AttackHandler(this);
        _animHandler = new UnitAnimationHandler(this);
    }
#endregion

#region FSM HANDLER ->  FSMHandler에 접근

    // Unit State 변경 
    public void F_ChangeState(UNIT_STATE _State)
    {
        _FSMHandler.FH_ChangeState(_State);
    }

    // 현재 상태 진입 
    public void F_StateEnter() 
    {
        _FSMHandler.FH_CurrStateEnter();
    }

    // 현재 상태 실행 
    public void F_StateExcute() 
    {
        _FSMHandler.FH_CurrStateExcute();
    }

    // 현재 상태 Setting ->
    public void F_SettingCurrState(UNIT_STATE _State) 
    {
        _FSMHandler.FH_SettingCurrState(_State);
    }

    // 이전 상태 Setting 
    public void F_SettingPreState(UNIT_STATE _state) 
    {
        _FSMHandler.FH_SettingPreState(_state);
    }

#endregion

    #region State

    // Unit hp 검사 -> FSMHandler에 접근
    public void F_ChekchUnitHp() 
    {
        // hp가 0이하면 true 
        if (_unitState.UnitHp <= 0)
        {
            // Die로 상태변화
            F_ChangeState(UNIT_STATE.Die);
        }
    }

    public void F_GetDamage(float v_damage) 
    {
        // damage만큼 hp 감소
        _unitState.UnitHp -= v_damage;
    }

    public void F_ChageSpeed(float v_speed) 
    {
        // 속도 변경 
        _unitState.UnitSpeed = v_speed;
    }
#endregion

#region TRAKING HANDLER

    // Unit 일정시간동안 Traking
    public void F_UniTracking(Unit v_unit) 
    {
        StartCoroutine(_trackingHandler.IE_TrackinCorutine());
    }

    // Tracking end 시 실행 
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
