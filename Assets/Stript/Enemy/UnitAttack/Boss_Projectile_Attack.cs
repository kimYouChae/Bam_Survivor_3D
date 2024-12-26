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

    // ##TODO : RANGED랑 코드가 비슷해서 합치거나 등등 작업 필요 

    // 하늘에서 투사체가 떨어짐 
    public void IS_Attack(Unit _boss)
    {
        Debug.Log("BOSS가 Projectile Attack을 합니다"); 

        // Attack ( Trigger )애니메이션 실행
        _boss.F_TriggerAnimation(_attackType);

        // 원거리 공격
        // ##TODO : pool에서 get 해야함 
        GameObject _obj = UnitManager.Instance.UnitBulletPooling.F_UnitBulletGet(UnitBullet.RedApple);

        // marker 감지 
        Collider[] _coll = Physics.OverlapSphere(_boss.hitTransform.position, _boss.unitSearchRadious, LayerManager.Instance.markerLayer);

        // 방향 : 플레이어- unit방향벡터
        Vector3 _dir;

        if (_coll.Length > 0)
            _dir = _coll[0].transform.position - _boss.transform.position;

        else
            _dir = PlayerManager.Instance.markerHeadTrasform.position - _boss.transform.position;

        _obj.GetComponent<Rigidbody>().AddForce(_dir * BulletForce, ForceMode.Impulse);
    }
}