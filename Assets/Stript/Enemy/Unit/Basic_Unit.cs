using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Unit :  Unit 
{
    private void Awake()
    {  
        // FSM 세팅 
        F_InitUnitState(this);

        // FSM enter 
        F_CurrStateEnter();

    }

    private void Update()
    {
        // FSM excute 
        F_CurrStateExcute();
    }

    public override void F_UnitAttatk()
    {
        unitState.UnitTimeStamp += Time.deltaTime;

        if(unitState.UnitTimeStamp < unitState.UnitAttackTime)
        {
            // ##TODO : 각 공격동작 추가 
        }
        else if(unitState.UnitTimeStamp >= unitState.UnitAttackTime)
        {
            // tracking으로 상태변화
            F_ChangeState(UNIT_STATE.Tracking);
        }
    }


}
