using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Rush_Attack : AttackStrategy
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

        // Attack ( Trigger )애니메이션 실행
        _boss.F_TriggerAnimation(_attackType);

        // 쫒아가야하니까 Navmesh On
        _boss.F_NavmeshOnOff(true);

        // destination 지정 
        _boss.F_SetDestinationToHead();

        // 속도 변경 
        _boss.F_BossChangeSpeed();
    }
}
