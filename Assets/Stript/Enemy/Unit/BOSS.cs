using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS : Unit
{
    private void Awake()
    {
       
    }

    // 켜졌을 때 enter (pool에서 on 될 때 )
    private void OnEnable()
    {
       
    }

    private void Update()
    {
       
    }

    internal class Boss_Rush_Attack : IAttackStrategy
    {
        // 보스 돌진 
        public void Attack(Unit _boss)
        {
            Debug.Log("BOSS가 RUSH Attack을 합니다");
        }
    }


    internal class Boss_Projectile_Attack : IAttackStrategy
    {
        // 하늘에서 투사체가 떨어짐 

        public void Attack(Unit _boss)
        {
            Debug.Log("BOSS가 Projectile Attack을 합니다");
        }
    }

    internal class Boss_Basic_Attack : IAttackStrategy
    {
        public void Attack(Unit _boss)
        {
            Debug.Log("RANGED가 BASIC Attack 을 합니다");
        }
    }
}
