using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Boss_Projectile_Attack : IAttackStrategy
{
    [SerializeField]
    private UnitAnimationType _attackType;

    public Boss_Projectile_Attack(UnitAnimationType _type)
    {
        this._attackType = _type;
    }

    // �ϴÿ��� ����ü�� ������ 
    public void IS_Attack(Unit _boss)
    {
        Debug.Log("BOSS�� Projectile Attack�� �մϴ�");
    }
}