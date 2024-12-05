using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MELLE : Unit
{
    private void Awake()
    {
        // Awake�� �ʱ�1ȸ���� �����ȴ�
        // �ڵ鷯 �ʱ�ȭ
        F_InitHandlerSetting();

        // lifeCycle�� exist�� 
        _lifeCycle = LifeCycle.InitInstance;

        // Attack Interface ���� 
        F_AddToAttackStrtegy(new MELLE_Attack());
    }

    // ������ �� enter (pool���� on �� �� )
    private void OnEnable()
    {
        // �ʱ����x pool���� ���� �� on �ɶ���
        if (_lifeCycle == LifeCycle.ExistingInstance)
        {
            // ������� ���� 
            F_SettingCurrState(UNIT_STATE.Tracking);

            // FSM enter 
            F_StateEnter();
        }

        // Init�϶���
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
            // Attack �ִϸ��̼� ����
            // _unit.F_SetAnimatorTriggerByState(UnitAnimationType.BasicAttack);
            //_unit.F_ChangeAniParemeter(UnitAnimationType.Tracking, false);

            Debug.Log("Melle�� Attack�� �մϴ�");
        }
    }

}
