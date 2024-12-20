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
        Debug.Log("RANGED가 BASIC Attack 을 합니다");

        // Attack ( Trigger )애니메이션 실행
        _boss.F_TriggerAnimation(_attackType);

        // 근접 공격 
        Collider[] _coll = Physics.OverlapSphere(_boss.hitPosition.position, _boss.unitSearchRadious, LayerManager.Instance.markerLayer);

        if (_coll.Length <= 0)
            return;

        // 감지되면
        foreach (Collider marker in _coll)
        {
            //Debug.Log("MELLE_ATTACK이 실행되고 있습니다 . 타겟 : " + marker.gameObject.name);

            if (marker.GetComponent<Marker>() == null)
                return;

            // 데미지 넣기 
            marker.GetComponent<Marker>().F_UpdateHP(_boss.unitDamage * (-1f));
        }
    }
}