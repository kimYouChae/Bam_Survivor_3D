using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MELLE : Unit
{
    private void Awake()
    {
        // Awake는 초기1회에만 생성된다
        // 핸들러 초기화
        F_InitHandlerSetting();

        // lifeCycle을 exist로 
        _lifeCycle = LifeCycle.InitInstance;

        // Attack Interface 저장 
        F_AddToAttackStrtegy(new MELLE_Attack());
    }

    // 켜졌을 때 enter (pool에서 on 될 때 )
    private void OnEnable()
    {
        // 초기생성x pool에서 꺼낸 후 on 될때만
        if (_lifeCycle == LifeCycle.ExistingInstance)
        {
            // 현재상태 지정 
            F_SettingCurrState(UNIT_STATE.Tracking);

            // FSM enter 
            F_StateEnter();
        }

        // Init일때만
        if (_lifeCycle == LifeCycle.InitInstance)
            _lifeCycle = LifeCycle.ExistingInstance;
    }

    private void Update()
    {
        // FSM excute 
        F_StateExcute();
    }

    internal class MELLE_Attack : IAttackStrategy
    {
        public void Attack(Unit _unit)
        {
            // Attack 애니메이션 실행
            // _unit.F_SetAnimatorTriggerByState(UnitAnimationType.BasicAttack);
            //_unit.F_ChangeAniParemeter(UnitAnimationType.Tracking, false);

            Debug.Log("Melle가 Attack을 합니다");
        }
    }

}
