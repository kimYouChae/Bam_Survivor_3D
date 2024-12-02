using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS : Unit
{
    private void Awake()
    {
        // Awake�� �ʱ�1ȸ���� �����ȴ�
        // FSM ���� 
        F_InitUnitState(this);

        // lifeCycle�� exist�� 
        _lifeCycle = LifeCycle.InitInstance;

        // ##TODO : awake, onEnable, disEnable < �̰Ŵ� unit��ӹ޴� ��� Ŭ������ �����ϴϱ� 
        // unit���� �ۼ��� ��ò����ò� �ϸ�ɵ� ?
        _strategyList = new List<IAttackStrategy>();
        _strategyList.Add(new Boss_Rush_Attack());
        _strategyList.Add(new Boss_Projectile_Attack());
        _strategyList.Add(new Boss_Basic_Attack());

        // EX) ����� ����
        //_strategyList[0].Attack(this);
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

    internal class Boss_Rush_Attack : IAttackStrategy
    {
        // ���� ���� 
        public void Attack(Unit _boss)
        {
            Debug.Log("BOSS�� RUSH Attack�� �մϴ�");
        }
    }


    internal class Boss_Projectile_Attack : IAttackStrategy
    {
        // �ϴÿ��� ����ü�� ������ 

        public void Attack(Unit _boss)
        {
            Debug.Log("BOSS�� Projectile Attack�� �մϴ�");
        }
    }

    internal class Boss_Basic_Attack : IAttackStrategy
    {
        public void Attack(Unit _boss)
        {
            Debug.Log("RANGED�� BASIC Attack �� �մϴ�");
        }
    }
}
