using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static MELLE;

public class RANGES : Unit
{
    private void Awake()
    {
        // Awake는 초기1회에만 생성된다
        // 핸들러 초기화
        F_InitHandlerSetting();

        // lifeCycle을 exist로 
        _lifeCycle = LifeCycle.InitInstance;

        // Attack Interface 저장 
        F_AddToAttackStrtegy(UnitAnimationType.BasicAttack, new RANGED_Basic_Attack(UnitAnimationType.BasicAttack));
    }

    // 켜졌을 때 enter (pool에서 on 될 때 )
    private void OnEnable()
    {
        switch (_lifeCycle)
        {
            // Init일때만
            case LifeCycle.InitInstance:
                _lifeCycle = LifeCycle.ExistingInstance;
                break;

            // 초기생성x pool에서 꺼낸 후 on 될때만
            case LifeCycle.ExistingInstance:
                // 현재상태 지정 
                F_SettingCurrState(UNIT_STATE.Tracking);
                // FSM enter 
                F_StateEnter();
                break;
        }
    }

    // 꺼졌을 때 (pool에 들어가서 off 될 때)
    private void OnDisable()
    {
        if (_lifeCycle != LifeCycle.ExistingInstance)
            return;

        // 이것저것 초기화  
        F_OnDisable();
    }

    private void Update()
    {
        // FSM excute 
        F_StateExcute();
    }

    public override UnitAnimationType F_returnAttackType()
    {
        return UnitAnimationType.BasicAttack;
    }
}
