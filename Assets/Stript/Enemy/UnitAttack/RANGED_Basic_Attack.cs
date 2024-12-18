using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RANGED_Basic_Attack : IAttackStrategy
{
    [SerializeField]
    private UnitAnimationType _attackType;

    [SerializeField]
    private const float BulletForce = 2f;

    // 생성자
    public RANGED_Basic_Attack(UnitAnimationType _type)
    {
        this._attackType = _type;
    }

    public void IS_Attack(Unit _unit)
    {
        Debug.Log("RANGED가 Attack을 합니다");

        // Attack ( Trigger )애니메이션 실행
        _unit.F_TriggerAnimation(_attackType);

        // 원거리 공격
        // ##TODO : pool에서 get 해야함 
        //GameObject _obj = Instantiate(UnitManager.Instance.UnitBullet, _unit.hitPosition.position, Quaternion.identity);

        // marker 감지 
        Collider[] _coll = Physics.OverlapSphere(_unit.hitPosition.position, _unit.unitSearchRadious, LayerManager.Instance.markerLayer);

        // 방향 : 플레이어- unit방향벡터
        Vector3 _dir;

        if (_coll.Length > 0)
            _dir = _coll[0].transform.position - _unit.transform.position;

        else
            _dir = PlayerManager.Instance.markerHeadTrasform.position - _unit.transform.position;

        //_obj.GetComponent<Rigidbody>().AddForce(_dir * BulletForce, ForceMode.Impulse);

    }
}
