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

    // 생성자
    public AttackHandler(Unit _unit) 
    { 
        this._unit = _unit;

        // attack인터페이스 리스트 초기화
        DICT_AniTypeByAttackSt = new Dictionary<UnitAnimationType, AttackStrategy>();
    }

    public void AH_AddAttackList(UnitAnimationType _aniType , AttackStrategy _attack) 
    {
        // 리스트는 생성자에서 초기화
        try
        {
            DICT_AniTypeByAttackSt.Add(_aniType, _attack);
        }
        catch (Exception e) 
        {
            Debug.LogError(e.ToString());
        }
    }

    // 공격 실행 
    public void AH_AttackExcutor()
    {
        // type을 return 받음 
        _currAnimationType = _unit.F_returnAttackType();

        // 딕셔너리 안에 있으면 
        if (DICT_AniTypeByAttackSt.ContainsKey(_currAnimationType))
        {
            // 해당 type에 해당하는 attack 실행 
            DICT_AniTypeByAttackSt[_currAnimationType].IS_Attack(_unit);
        }
        else 
        {
            Debug.LogError(" !!!!!!!!!DIctionary에 AttackStretegy가 없음!!!!!!!!! ");
        }
    }
}
