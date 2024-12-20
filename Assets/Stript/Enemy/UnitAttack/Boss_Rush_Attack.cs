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

    // ���� ���� 
    public void IS_Attack(Unit _boss)
    {
        Debug.Log("BOSS�� RUSH Attack�� �մϴ�");

        // Attack ( Trigger )�ִϸ��̼� ����
        _boss.F_TriggerAnimation(_attackType);

        // �i�ư����ϴϱ� Navmesh On
        _boss.F_NavmeshOnOff(true);

        // destination ���� 
        _boss.F_SetDestinationToHead();

        // �ӵ� ���� 
        _boss.F_BossChangeSpeed();
    }
}
