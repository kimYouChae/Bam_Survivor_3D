using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BloodShieldObject : ShieldObject
{
    void Update()
    {
        F_ShieldUpdate();

        F_FllowMarker();
    }

    protected override void F_EndShiled()
    {
        // Blood 획득 count 검사
        if (!ShieldManager.Instance.F_IsBloodExution())
            return;

        // 일정횟수 이상 획득
        // 처형효과 추가

        Collider[] _coll = F_ReturnUnitCollider(gameObject, gameObject.transform.localScale.x , LayerManager.Instance.unitLayer);

        // Linq로 일정 hp 하위 unit 검출
        var _excutionUnit = from coll in _coll
                            where coll.GetComponent<Unit>() != null && coll.GetComponent<Unit>().unitHp 
                                <= ShieldManager.Instance.bloodExcutionLimit
                            select coll.GetComponent<Unit>();

        foreach (var unit in _excutionUnit) 
        {
            // ## TODO : Unit Pool로 되돌리기

        }

        // 쉴드 pool로 되돌리기
        ShieldManager.Instance.shieldPooling.F_ShieldSet(gameObject, Shield_Effect.Epic_BloodSiphon);

    }

    protected override void F_ExpandingShield()
    {
        Collider[] _coll = F_ReturnUnitCollider(gameObject, gameObject.transform.localScale.x, LayerManager.Instance.unitLayer);

        // 흠혈 
        // 비울 * 획득 count 만큼 
        float _bloodAmount =
            ShieldManager.Instance.bloodShiponRatio
            * ShieldManager.Instance.F_ReturnCountToDic(Shield_Effect.Epic_BloodSiphon);

        foreach (Collider unit in _coll) 
        {
            try 
            {
                // Unit 피 감소 
                unit.GetComponent<Unit>().F_GetDamage(_bloodAmount);

                // 플레이어 Marker hp 증가
                _parentMarker.F_UpdateHP(HP:_bloodAmount);
            }
            catch(Exception e) 
            {
                Debug.LogError(e.ToString());
            }   
        }


    }
}
