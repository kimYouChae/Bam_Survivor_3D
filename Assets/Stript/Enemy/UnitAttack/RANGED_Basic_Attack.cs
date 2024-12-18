using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RANGED_Basic_Attack : IAttackStrategy
{
    [SerializeField]
    private UnitAnimationType _attackType;

    [SerializeField]
    private const float BulletForce = 2f;

    // ������
    public RANGED_Basic_Attack(UnitAnimationType _type)
    {
        this._attackType = _type;
    }

    public void IS_Attack(Unit _unit)
    {
        Debug.Log("RANGED�� Attack�� �մϴ�");

        // Attack ( Trigger )�ִϸ��̼� ����
        _unit.F_TriggerAnimation(_attackType);

        // ���Ÿ� ����
        // ##TODO : pool���� get �ؾ��� 
        //GameObject _obj = Instantiate(UnitManager.Instance.UnitBullet, _unit.hitPosition.position, Quaternion.identity);

        // marker ���� 
        Collider[] _coll = Physics.OverlapSphere(_unit.hitPosition.position, _unit.unitSearchRadious, LayerManager.Instance.markerLayer);

        // ���� : �÷��̾�- unit���⺤��
        Vector3 _dir;

        if (_coll.Length > 0)
            _dir = _coll[0].transform.position - _unit.transform.position;

        else
            _dir = PlayerManager.Instance.markerHeadTrasform.position - _unit.transform.position;

        //_obj.GetComponent<Rigidbody>().AddForce(_dir * BulletForce, ForceMode.Impulse);

    }
}
