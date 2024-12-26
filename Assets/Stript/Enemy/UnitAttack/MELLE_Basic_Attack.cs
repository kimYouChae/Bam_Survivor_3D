using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MELLE_Basic_Attack : AttackStrategy
{
    [SerializeField]
    private UnitAnimationType _attackType;

    // 생성자
    public MELLE_Basic_Attack(UnitAnimationType _type) 
    {
        this._attackType = _type;
    }

    public void IS_Attack(Unit _unit)
    {
        Debug.Log("Melle가 Attack을 합니다");

        // Attack ( Trigger )애니메이션 실행
        _unit.F_TriggerAnimation(_attackType);

        // 근접 공격 
        Collider[] _coll = Physics.OverlapSphere(_unit.hitTransform.position, _unit.unitSearchRadious, LayerManager.Instance.markerLayer);

        if (_coll.Length <= 0)
            return;

        // 감지되면
        foreach (Collider marker in _coll)
        {
            //Debug.Log("MELLE_ATTACK이 실행되고 있습니다 . 타겟 : " + marker.gameObject.name);

            if (marker.GetComponent<Marker>() == null)
                return;

            // 데미지 넣기 
            marker.GetComponent<Marker>().F_UpdateHP(_unit.unitDamage * (-1f));
        }
    }
}

