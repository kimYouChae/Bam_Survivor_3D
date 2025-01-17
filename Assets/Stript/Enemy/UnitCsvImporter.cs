using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitCsvImporter : CSVManager
{
    [Header("===Container===")]
    [SerializeField]
    private Dictionary<Unit_Animal_Type, UnitState> DICT_AnimalTypeToUnitState;

    // type �� state return
    public UnitState F_AnimalTypeToState(Unit_Animal_Type _type)
    {
        if (!DICT_AnimalTypeToUnitState.ContainsKey(_type))
            return null;

        // ���� �� ���� 
        UnitState _state = new UnitState(DICT_AnimalTypeToUnitState[_type]); 
        return _state;
    }

    protected override void F_InitContainer()
    {
        //���ϸ� �ʱ�ȭ 
        FileName = "Unit";

        // �����̳� �ʱ�ȭ 
        DICT_AnimalTypeToUnitState = new Dictionary<Unit_Animal_Type, UnitState>();
    }

    protected override void F_ProcessData(string[] _data)
    {
        // state ���� 
        UnitState _unitState = new UnitState(_data);

        // DICT�� �ֱ� 
        try
        {
            DICT_AnimalTypeToUnitState.Add(_unitState.AnimalType, _unitState);
        }
        catch (Exception e) 
        {
            Debug.Log(e);
        }
    }

}
