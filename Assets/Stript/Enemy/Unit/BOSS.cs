using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS : Unit
{
    private void Awake()
    {
       
    }

    // ������ �� enter (pool���� on �� �� )
    private void OnEnable()
    {
       
    }

    private void Update()
    {
       
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
