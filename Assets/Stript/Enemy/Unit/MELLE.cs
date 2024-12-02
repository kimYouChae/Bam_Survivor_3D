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
        _lifeCycle = LifeCycle.InitInstance;

        // Attack Interface ���� 
        _strategyList = new List<IAttackStrategy>();
        _strategyList.Add(new MELLE_Attack());
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

        // Init�϶���
        if (_lifeCycle == LifeCycle.InitInstance)
            _lifeCycle = LifeCycle.ExistingInstance;
    }

    private void Update()
    {
        // FSM excute 
        F_CurrStateExcute();
    }

    internal class MELLE_Attack : IAttackStrategy
    {
        public void Attack(Unit _unit)
        {
            // Attack �ִϸ��̼� ����
            _unit.F_ChangeAniParemeter(UnitAnimationType.BasicAttack, true);
            //_unit.F_ChangeAniParemeter(UnitAnimationType.Tracking, false);

            Debug.Log("Melle�� Attack�� �մϴ�");
        }
    }

}
