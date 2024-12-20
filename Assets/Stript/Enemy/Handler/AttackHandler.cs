using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class AttackHandler : IAttackHandler
{
    [Header("===Unit===")]
    private Unit _unit;

    [Header("===Attack===")]
    [SerializeField] private Dictionary<UnitAnimationType, AttackStrategy> DICT_AniTypeByAttackSt;
    [SerializeField] private AttackStrategy _nowAttack;
    [SerializeField] private UnitAnimationType _currAnimationType;

    // ������
    public AttackHandler(Unit _unit) 
    { 
        this._unit = _unit;

        // attack�������̽� ����Ʈ �ʱ�ȭ
        DICT_AniTypeByAttackSt = new Dictionary<UnitAnimationType, AttackStrategy>();
    }

    public void AH_AddAttackList(UnitAnimationType _aniType , AttackStrategy _attack) 
    {
        // ����Ʈ�� �����ڿ��� �ʱ�ȭ
        try
        {
            DICT_AniTypeByAttackSt.Add(_aniType, _attack);
        }
        catch (Exception e) 
        {
            Debug.LogError(e.ToString());
        }
    }

    // ���� ���� 
    public void AH_AttackExcutor()
    {
        // type�� return ���� 
        _currAnimationType = _unit.F_returnAttackType();

        // ��ųʸ� �ȿ� ������ 
        if (DICT_AniTypeByAttackSt.ContainsKey(_currAnimationType))
        {
            // �ش� type�� �ش��ϴ� attack ���� 
            DICT_AniTypeByAttackSt[_currAnimationType].IS_Attack(_unit);
        }
        else 
        {
            Debug.LogError(" !!!!!!!!!DIctionary�� AttackStretegy�� ����!!!!!!!!! ");
        }
    }
}
