using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MELLE : Unit
{
    private void Awake()
    {
        // Awake�� �ʱ�1ȸ���� �����ȴ�
        // FSM ���� 
        F_InitUnitState(this);

        // lifeCycle�� exist�� 
        _lifeCycle = LifeCycle.ExistingInstance;
    }

    // ������ �� enter (pool���� on �� �� )
    private void OnEnable()
    {
        // �ʱ����x pool���� ���� �� on �ɶ���
        if (_lifeCycle == LifeCycle.ExistingInstance)
        {
            // ������� ���� 
            Curr_UNITS_TATE = UNIT_STATE.Tracking;

            // FSM enter 
            F_CurrStateEnter();
        }
    }

    private void Update()
    {
        // FSM excute 
        F_CurrStateExcute();
    }

}
