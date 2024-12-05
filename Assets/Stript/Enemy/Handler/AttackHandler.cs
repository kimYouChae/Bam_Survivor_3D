using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class AttackHandler : IAttackHandler
{
    [Header("===Unit===")]
    private Unit _unit;

    [Header("===Attack===")]
    [SerializeField] protected List<IAttackStrategy> _strategyList;
    [SerializeField] private IAttackStrategy _nowAttack;

    // 생성자
    public AttackHandler(Unit _unit) 
    { 
        this._unit = _unit;

        // attack인터페이스 리스트 초기화
        _strategyList = new List<IAttackStrategy>();
    }

    public void AH_AddAttackList(IAttackStrategy _attack) 
    {
        // 리스트는 생성자에서 초기화
        try
        {
            _strategyList.Add(_attack);
        }
        catch (Exception e) 
        {
            Debug.LogError(e.ToString());
        }
    }

    // Attack Interface 리스트안에서 랜덤으로 idx 골라서 attack 실행
    public void AttackExcutor()
    {
        if (_strategyList.Count <= 0)
        {
            Debug.LogError("Attack Interface not implemented ");
            return;
        }

        // 리스트 내 랜덤 인덱스 구하기 
        int _randIdx = Random.Range(0, _strategyList.Count);

        // 인덱스에 해당하는 인터페이스 함수 실행
        _strategyList[_randIdx].Attack(_unit);

        // ##TODO : 지워도됨 인스펙터 창에서 보기위한
        _nowAttack = _strategyList[_randIdx];
    }

}
