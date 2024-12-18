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

    // 하늘에서 투사체가 떨어짐 
    public void IS_Attack(Unit _boss)
    {
        Debug.Log("BOSS가 Projectile Attack을 합니다");
    }
}