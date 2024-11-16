using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Timeline;

public class HealingShieldObject : ShieldObject
{
    void Update()
    {
        F_ShieldUpdate();
    }

    protected override void F_EndShiled()
    {
        // 일정 범위 내 marker을 다 검사 후 hp증가 
        Collider[] _coll = F_ReturnUnitCollider(gameObject , _maxsize.x, LayerManager.Instance.markerLayer);

        foreach (Collider _marker in _coll) 
        {
            try 
            {
                // 회복량 : marker의 maxHp의 반 만큼 
                _marker.GetComponent<Marker>().F_UpdateHP( parentMarker.markerState.markerMaxHp / 2 );
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        // 쉴드 pool로 되돌리기
        ShieldManager.Instance.shieldPooling.F_ShieldSet(gameObject, Shield_Effect.Legend_HealingField);
    }

    protected override void F_ExpandingShield()
    { 

    }
}
