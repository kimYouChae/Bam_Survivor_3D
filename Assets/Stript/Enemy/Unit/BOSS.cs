using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS : Unit
{
    // ##TODO : 상위로 올려야함
    private List<IAttackStrategy> _strategyList;

    private void Awake()
    {
        // Awake는 초기1회에만 생성된다
        // FSM 세팅 
        F_InitUnitState(this);

        // lifeCycle을 exist로 
        _lifeCycle = LifeCycle.ExistingInstance;

        // ##TODO : awake, onEnable, disEnable < 이거는 unit상속받는 모든 클래스가 동일하니까 
        // unit에서 작성후 모시깽저시깽 하면될듯 ?
        _strategyList = new List<IAttackStrategy>();
        _strategyList.Add(new Boss_Rush_Attack());
        _strategyList.Add(new Boss_Projectile_Attack());
        _strategyList.Add(new Boss_Basic_Attack());

        // EX) 사용할 때는
        _strategyList[0].Attack(this);
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

    internal class Boss_Rush_Attack : IAttackStrategy
    {
        // 보스 돌진 
        public void Attack(Unit _boss)
        {
            
        }
    }


    internal class Boss_Projectile_Attack : IAttackStrategy
    {
        // 하늘에서 투사체가 떨어짐 

        public void Attack(Unit _boss)
        {
            
        }
    }

    internal class Boss_Basic_Attack : IAttackStrategy
    {
        public void Attack(Unit _boss)
        {
            
        }
    }
}
