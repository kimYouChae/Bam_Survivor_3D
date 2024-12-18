using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class BOSS : Unit
{
    private void Awake()
    {
       
    }

    // 켜졌을 때 enter (pool에서 on 될 때 )
    private void OnEnable()
    {
       
    }

    private void Update()
    {
       
    }

    public override UnitAnimationType F_returnAttackType()
    {
        // marker와 unit의 거리가 
        float _distance = Vector3.Distance(PlayerManager.Instance.markerHeadTrasform.position, this.transform.position);

        // TODO : 임시거리 
        if (_distance <= 5f)
        {
            // rush
            return UnitAnimationType.RushAttack;
        }
        else if (_distance <= 3f) 
        {
            // 투사체
            return UnitAnimationType.ProjectileAttack;
        }
        else 
        {
            return UnitAnimationType.BasicAttack;
        }
    }


}
