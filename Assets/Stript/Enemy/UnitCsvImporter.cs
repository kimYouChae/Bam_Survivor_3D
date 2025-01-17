using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitCsvImporter : CSVManager
{
    [Header("===Container===")]
    [SerializeField]
    private Dictionary<Unit_Animal_Type, UnitState> DICT_AnimalTypeToUnitState;

    // type 별 state return
    public UnitState F_AnimalTypeToState(Unit_Animal_Type _type)
    {
        if (!DICT_AnimalTypeToUnitState.ContainsKey(_type))
            return null;

        // 생성 후 복사 
        UnitState _state = new UnitState(DICT_AnimalTypeToUnitState[_type]); 
        return _state;
    }

    protected override void F_InitContainer()
    {
        //파일명 초기화 
        FileName = "Unit";

        // 컨테이너 초기화 
        DICT_AnimalTypeToUnitState = new Dictionary<Unit_Animal_Type, UnitState>();
    }

    protected override void F_ProcessData(string[] _data)
    {
        // state 생성 
        UnitState _unitState = new UnitState(_data);

        // DICT에 넣기 
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
