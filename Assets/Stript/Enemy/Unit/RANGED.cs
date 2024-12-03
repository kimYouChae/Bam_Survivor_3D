using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MELLE;

public class RANGES : Unit
{
    private void Awake()
    {
        // Awake는 초기1회에만 생성된다
        // FSM 세팅 
        F_InitUnitState(this);

        // lifeCycle을 exist로 
        _lifeCycle = LifeCycle.InitInstance;

        // Attack Interface 저장 
        _strategyList = new List<IAttackStrategy>();
        _strategyList.Add(new RANGED_Attack());
    }

    // 켜졌을 때 enter (pool에서 on 될 때 )
    private void OnEnable()
    {
        // 초기생성x pool에서 꺼낸 후 on 될때만
        if (_lifeCycle == LifeCycle.ExistingInstance)
        {
            // 현재상태 지정 
            Curr_UNITS_TATE = UNIT_STATE.Tracking;

            // FSM enter 
            F_CurrStateEnter();
        }

        // Init일때만
        if(_lifeCycle == LifeCycle.InitInstance)
            _lifeCycle = LifeCycle.ExistingInstance;
    }

    private void Update()
    {
        // FSM excute 
        F_CurrStateExcute();
    }

    internal class RANGED_Attack : IAttackStrategy
    {
        public void Attack(Unit _unit)
        {
            Debug.Log("RANGED가 Attack을 합니다");
        }
    }

}
