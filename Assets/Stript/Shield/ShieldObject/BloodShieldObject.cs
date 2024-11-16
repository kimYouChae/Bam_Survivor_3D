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
        // Blood ȹ�� count �˻�
        if (!ShieldManager.Instance.F_IsBloodExution())
            return;

        // ����Ƚ�� �̻� ȹ��
        // ó��ȿ�� �߰�

        Collider[] _coll = F_ReturnUnitCollider(gameObject, gameObject.transform.localScale.x , LayerManager.Instance.unitLayer);

        // Linq�� ���� hp ���� unit ����
        var _excutionUnit = from coll in _coll
                            where coll.GetComponent<Unit>() != null && coll.GetComponent<Unit>().unitHp 
                                <= ShieldManager.Instance.bloodExcutionLimit
                            select coll.GetComponent<Unit>();

        foreach (var unit in _excutionUnit) 
        {
            // ## TODO : Unit Pool�� �ǵ�����

        }

        // ���� pool�� �ǵ�����
        ShieldManager.Instance.shieldPooling.F_ShieldSet(gameObject, Shield_Effect.Epic_BloodSiphon);

    }

    protected override void F_ExpandingShield()
    {
        Collider[] _coll = F_ReturnUnitCollider(gameObject, gameObject.transform.localScale.x, LayerManager.Instance.unitLayer);

        // ���� 
        // ��� * ȹ�� count ��ŭ 
        float _bloodAmount =
            ShieldManager.Instance.bloodShiponRatio
            * ShieldManager.Instance.F_ReturnCountToDic(Shield_Effect.Epic_BloodSiphon);

        foreach (Collider unit in _coll) 
        {
            try 
            {
                // Unit �� ���� 
                unit.GetComponent<Unit>().F_GetDamage(_bloodAmount);

                // �÷��̾� Marker hp ����
                _parentMarker.F_UpdateHP(HP:_bloodAmount);
            }
            catch(Exception e) 
            {
                Debug.LogError(e.ToString());
            }   
        }


    }
}
