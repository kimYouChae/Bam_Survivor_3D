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

    // 생성자
    public RANGED_Basic_Attack(UnitAnimationType _type)
    {
        this._attackType = _type;
        _bulletType = UnitBulletType.RedApple;
    }

    public void IS_Attack(Unit _unit)
    {
        Debug.Log("RANGED가 Attack을 합니다");

        // Attack ( Trigger )애니메이션 실행
        _unit.F_TriggerAnimation(_attackType);

        // 원거리 공격
        // ##TODO : pool에서 get 해야함 
        GameObject _obj = UnitManager.Instance.UnitBulletPooling.F_UnitBulletGet(_bulletType);
        // 위치조정 
        _obj.transform.position = _unit.hitTransform.position;

        // marker 감지 
        Collider[] _coll = Physics.OverlapSphere(_unit.hitTransform.position, _unit.unitSearchRadious, LayerManager.Instance.markerLayer);

        // 방향 : 플레이어- unit방향벡터
        Vector3 _dir;

        if (_coll.Length > 0)
            _dir = _coll[0].transform.position - _unit.transform.position;

        else
            _dir = PlayerManager.Instance.markerHeadTrasform.position - _unit.transform.position;

        // add force
        _obj.GetComponent<Rigidbody>().AddForce(new Vector3(_dir.x , 0 , _dir.z) * BulletForce, ForceMode.Impulse);
        // 스크립트에 넣기 
        _obj.GetComponent<UnitBullet>().BulletType = _bulletType;
        _obj.GetComponent<UnitBullet>().Damage = _unit.unitDamage;
    }
}
