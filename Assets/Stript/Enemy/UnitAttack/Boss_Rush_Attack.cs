using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Rush_Attack : IAttackStrategy
{
    [SerializeField]
    private UnitAnimationType _attackType;
    public Boss_Rush_Attack(UnitAnimationType _type)
    {
        this._attackType = _type;
    }

    // 보스 돌진 
    public void IS_Attack(Unit _boss)
    {
        Debug.Log("BOSS가 RUSH Attack을 합니다");
    }
}
