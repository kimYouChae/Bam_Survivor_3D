using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class BOSS : Unit
{
    private void Awake()
    {
       
    }

    // ������ �� enter (pool���� on �� �� )
    private void OnEnable()
    {
       
    }

    private void Update()
    {
       
    }

    public override UnitAnimationType F_returnAttackType()
    {
        // marker�� unit�� �Ÿ��� 
        float _distance = Vector3.Distance(PlayerManager.Instance.markerHeadTrasform.position, this.transform.position);

        // TODO : �ӽðŸ� 
        if (_distance <= 5f)
        {
            // rush
            return UnitAnimationType.RushAttack;
        }
        else if (_distance <= 3f) 
        {
            // ����ü
            return UnitAnimationType.ProjectileAttack;
        }
        else 
        {
            return UnitAnimationType.BasicAttack;
        }
    }


}
