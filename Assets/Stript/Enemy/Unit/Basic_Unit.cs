using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Unit :  Unit 
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

    public override void F_UnitAttackAnimationCheck()
    {
        // attack�� ����ǰ� �ִ���
        if (_unitAnimator.GetCurrentAnimatorStateInfo(0).IsName(DICT_unitAniPara[UnitAnimation.Attack]) == true)
        {
            // �÷���������
            float _aniPlayTime = _unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            // �ִϸ��̼��� ����Ǹ� 
            if (_aniPlayTime >= 1.0f)
            {
                // tracking���� ���º�ȭ
                F_ChangeState(UNIT_STATE.Tracking);
            }
        }
    }


}
