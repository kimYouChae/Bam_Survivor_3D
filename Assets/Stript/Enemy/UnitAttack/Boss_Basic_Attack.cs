using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Basic_Attack : AttackStrategy
{
    [SerializeField]
    private UnitAnimationType _attackType;
    public Boss_Basic_Attack(UnitAnimationType _type)
    {
        this._attackType = _type;
    }

    public void IS_Attack(Unit _boss)
    {
        Debug.Log("RANGED�� BASIC Attack �� �մϴ�");

        // Attack ( Trigger )�ִϸ��̼� ����
        _boss.F_TriggerAnimation(_attackType);

        // ���� ���� 
        Collider[] _coll = Physics.OverlapSphere(_boss.hitPosition.position, _boss.unitSearchRadious, LayerManager.Instance.markerLayer);

        if (_coll.Length <= 0)
            return;

        // �����Ǹ�
        foreach (Collider marker in _coll)
        {
            //Debug.Log("MELLE_ATTACK�� ����ǰ� �ֽ��ϴ� . Ÿ�� : " + marker.gameObject.name);

            if (marker.GetComponent<Marker>() == null)
                return;

            // ������ �ֱ� 
            marker.GetComponent<Marker>().F_UpdateHP(_boss.unitDamage * (-1f));
        }
    }
}