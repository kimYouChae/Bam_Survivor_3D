using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Unit :  Unit 
{
    private void Awake()
    {
        // Awake는 초기1회에만 생성된다
        // FSM 세팅 
        F_InitUnitState(this);

        // lifeCycle을 exist로 
        _lifeCycle = LifeCycle.ExistingInstance;
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
    }

    private void Update()
    {
        // FSM excute 
        F_CurrStateExcute();
    }

    public override void F_UnitAttackAnimationCheck()
    {
        // attack이 실행되고 있는지
        if (_unitAnimator.GetCurrentAnimatorStateInfo(0).IsName(DICT_unitAniPara[UnitAnimation.Attack]) == true)
        {
            // 플레이중인지
            float _aniPlayTime = _unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            // 애니메이션이 종료되면 
            if (_aniPlayTime >= 1.0f)
            {
                // tracking으로 상태변화
                F_ChangeState(UNIT_STATE.Tracking);
            }
        }
    }


}
