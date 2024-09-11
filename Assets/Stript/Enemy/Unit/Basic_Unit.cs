using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple.ReplayKit;

public class Basic_Unit :  Unit 
{
    private void Awake()
    {
        // 스탯 초기화 
        F_InitUnitUnitState();
        
        // FSM 세팅 
        F_InitUnitState(this);

        // FSM enter 
        F_CurrStateEnter();

    }

    private void Update()
    {
        // FSM excute 
        F_CurrStateExcute();

        // Ray 그리기 
        F_DrawLine();
    }

    protected override void F_InitUnitUnitState()
    {
        this._unitHp = 10;
        this._unitSpeed = 3f;
        this._unitAttackTime = 2f;
        this._searchRadious = 1.5f;
        this._unitTimeStamp = 0;
    }

    public override void F_UnitAttatk()
    {
        _unitTimeStamp += Time.deltaTime;

        if(_unitTimeStamp < _unitAttackTime)
        {
            // ##TODO : 각 공격동작 추가 
        }
        else if(_unitTimeStamp >= _unitAttackTime )
        {
            // tracking으로 상태변화
            F_ChangeState(UNIT_STATE.Tracking);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, this._searchRadious);
    }



}
