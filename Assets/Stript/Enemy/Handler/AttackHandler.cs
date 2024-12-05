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

    // ������
    public AttackHandler(Unit _unit) 
    { 
        this._unit = _unit;

        // attack�������̽� ����Ʈ �ʱ�ȭ
        _strategyList = new List<IAttackStrategy>();
    }

    public void AH_AddAttackList(IAttackStrategy _attack) 
    {
        // ����Ʈ�� �����ڿ��� �ʱ�ȭ
        try
        {
            _strategyList.Add(_attack);
        }
        catch (Exception e) 
        {
            Debug.LogError(e.ToString());
        }
    }

    // Attack Interface ����Ʈ�ȿ��� �������� idx ��� attack ����
    public void AttackExcutor()
    {
        if (_strategyList.Count <= 0)
        {
            Debug.LogError("Attack Interface not implemented ");
            return;
        }

        // ����Ʈ �� ���� �ε��� ���ϱ� 
        int _randIdx = Random.Range(0, _strategyList.Count);

        // �ε����� �ش��ϴ� �������̽� �Լ� ����
        _strategyList[_randIdx].Attack(_unit);

        // ##TODO : �������� �ν����� â���� ��������
        _nowAttack = _strategyList[_randIdx];
    }

}
