using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Unit :  Unit 
{
    private void Awake()
    {  
        // FSM ���� 
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
            // ##TODO : �� ���ݵ��� �߰� 
        }
        else if(unitState.UnitTimeStamp >= unitState.UnitAttackTime)
        {
            // tracking���� ���º�ȭ
            F_ChangeState(UNIT_STATE.Tracking);
        }
    }


}
