using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Basic_Attack : IAttackStrategy
{
    [SerializeField]
    private UnitAnimationType _attackType;
    public Boss_Basic_Attack(UnitAnimationType _type)
    {
        this._attackType = _type;
    }

    public void IS_Attack(Unit _boss)
    {
        Debug.Log("RANGED가 BASIC Attack 을 합니다");
    }
}