using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RANGED_Basic_Attack : AttackStrategy
{
    [SerializeField]
    private UnitAnimationType _attackType;
    [SerializeField]
    private UnitBulletType _bulletType;

    [SerializeField]
    private const float BulletForce = 2f;

    // ������
    public RANGED_Basic_Attack(UnitAnimationType _type)
    {
        this._attackType = _type;
        _bulletType = UnitBulletType.RedApple;
    }

    public void IS_Attack(Unit _unit)
    {
        Debug.Log("RANGED�� Attack�� �մϴ�");

        // Attack ( Trigger )�ִϸ��̼� ����
        _unit.F_TriggerAnimation(_attackType);

        // ���Ÿ� ����
        // ##TODO : pool���� get �ؾ��� 
        GameObject _obj = UnitManager.Instance.UnitBulletPooling.F_UnitBulletGet(_bulletType);
        // ��ġ���� 
        _obj.transform.position = _unit.hitTransform.position;

        // marker ���� 
        Collider[] _coll = Physics.OverlapSphere(_unit.hitTransform.position, _unit.unitSearchRadious, LayerManager.Instance.markerLayer);

        // ���� : �÷��̾�- unit���⺤��
        Vector3 _dir;

        if (_coll.Length > 0)
            _dir = _coll[0].transform.position - _unit.transform.position;

        else
            _dir = PlayerManager.Instance.markerHeadTrasform.position - _unit.transform.position;

        // add force
        _obj.GetComponent<Rigidbody>().AddForce(new Vector3(_dir.x , 0 , _dir.z) * BulletForce, ForceMode.Impulse);
        // ��ũ��Ʈ�� �ֱ� 
        _obj.GetComponent<UnitBullet>().BulletType = _bulletType;
        _obj.GetComponent<UnitBullet>().Damage = _unit.unitDamage;
    }
}
