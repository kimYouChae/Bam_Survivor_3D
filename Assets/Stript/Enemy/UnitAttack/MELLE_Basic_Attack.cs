using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MELLE_Basic_Attack : AttackStrategy
{
    [SerializeField]
    private UnitAnimationType _attackType;

    // ������
    public MELLE_Basic_Attack(UnitAnimationType _type) 
    {
        this._attackType = _type;
    }

    public void IS_Attack(Unit _unit)
    {
        Debug.Log("Melle�� Attack�� �մϴ�");

        // Attack ( Trigger )�ִϸ��̼� ����
        _unit.F_TriggerAnimation(_attackType);

        // ���� ���� 
        Collider[] _coll = Physics.OverlapSphere(_unit.hitTransform.position, _unit.unitSearchRadious, LayerManager.Instance.markerLayer);

        if (_coll.Length <= 0)
            return;

        // �����Ǹ�
        foreach (Collider marker in _coll)
        {
            //Debug.Log("MELLE_ATTACK�� ����ǰ� �ֽ��ϴ� . Ÿ�� : " + marker.gameObject.name);

            if (marker.GetComponent<Marker>() == null)
                return;

            // ������ �ֱ� 
            marker.GetComponent<Marker>().F_UpdateHP(_unit.unitDamage * (-1f));
        }
    }
}

