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
        // ���� ���� �� marker�� �� �˻� �� hp���� 
        Collider[] _coll = F_ReturnUnitCollider(gameObject , _maxsize.x, LayerManager.Instance.markerLayer);

        foreach (Collider _marker in _coll) 
        {
            try 
            {
                // ȸ���� : marker�� maxHp�� �� ��ŭ 
                _marker.GetComponent<Marker>().F_UpdateHP( parentMarker.markerState.markerMaxHp / 2 );
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        // ���� pool�� �ǵ�����
        ShieldManager.Instance.shieldPooling.F_ShieldSet(gameObject, Shield_Effect.Legend_HealingField);
    }

    protected override void F_ExpandingShield()
    { 

    }
}
