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
    }

    protected override void F_EndShiled()
    {
        // Blood ȹ�� count �˻�
        if (!PlayerManager.instance.markerShieldController.F_IsBloodExution())
            return;

        // ����Ƚ�� �̻� ȹ��
        // ó��ȿ�� �߰�

        Collider[] _coll = F_ReturnUnitCollider(gameObject, gameObject.transform.localScale.x);

        // Linq�� ���� hp ���� unit ����
        var _excutionUnit = from coll in _coll
                            where coll.GetComponent<Unit>() != null && coll.GetComponent<Unit>().unitHp 
                                <= PlayerManager.instance.markerShieldController.bloodExcutionLimit
                            select coll.GetComponent<Unit>();

        foreach (var unit in _excutionUnit) 
        {
            // ## TODO : Unit Pool�� �ǵ�����

        }

    }

    protected override void F_ExpandingShield()
    {
        Collider[] _coll = F_ReturnUnitCollider(gameObject, gameObject.transform.localScale.x);

        // ���� 
        // ��� * ȹ�� count ��ŭ 
        float _bloodAmount =
            PlayerManager.instance.markerShieldController.bloodShiponRatio
            * PlayerManager.instance.markerShieldController.F_ReturnCountToDic(Shield_Effect.Epic_BloodSiphon);

        foreach (Collider unit in _coll) 
        {
            try 
            {
                // Unit �� ���� 
                unit.GetComponent<Unit>().F_GetDamage(_bloodAmount);

                // �÷��̾� Marker hp ����
                PlayerManager.instance.F_UpdateHP(HP : _bloodAmount);
            }
            catch(Exception e) 
            {
                Debug.LogError(e.ToString());
            }   
        }


    }
}
