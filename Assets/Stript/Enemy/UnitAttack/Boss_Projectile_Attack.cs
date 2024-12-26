using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Boss_Projectile_Attack : AttackStrategy
{
    [SerializeField]
    private UnitAnimationType _attackType;

    [SerializeField]
    private const float BulletForce = 2f;

    public Boss_Projectile_Attack(UnitAnimationType _type)
    {
        this._attackType = _type;
    }

    // ##TODO : RANGED�� �ڵ尡 ����ؼ� ��ġ�ų� ��� �۾� �ʿ� 

    // �ϴÿ��� ����ü�� ������ 
    public void IS_Attack(Unit _boss)
    {
        Debug.Log("BOSS�� Projectile Attack�� �մϴ�"); 

        // Attack ( Trigger )�ִϸ��̼� ����
        _boss.F_TriggerAnimation(_attackType);

        // ���Ÿ� ����
        // ##TODO : pool���� get �ؾ��� 
        GameObject _obj = UnitManager.Instance.UnitBulletPooling.F_UnitBulletGet(UnitBullet.RedApple);

        // marker ���� 
        Collider[] _coll = Physics.OverlapSphere(_boss.hitTransform.position, _boss.unitSearchRadious, LayerManager.Instance.markerLayer);

        // ���� : �÷��̾�- unit���⺤��
        Vector3 _dir;

        if (_coll.Length > 0)
            _dir = _coll[0].transform.position - _boss.transform.position;

        else
            _dir = PlayerManager.Instance.markerHeadTrasform.position - _boss.transform.position;

        _obj.GetComponent<Rigidbody>().AddForce(_dir * BulletForce, ForceMode.Impulse);
    }
}