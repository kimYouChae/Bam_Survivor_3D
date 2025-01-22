using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class PlayerCsvImporter : CSVManager
{
    [Header("===Container===")]
    [SerializeField]
    private Dictionary<AnimalType, PlayerAnimalState> DICT_AnimalToMarkerState;

    // type에 따른 state return
    public PlayerAnimalState F_AnimalTypeToState(AnimalType _type) 
    {
        //Debug.Log(DICT_AnimalToMarkerState[_type].markerName);

        if (DICT_AnimalToMarkerState.ContainsKey(_type))
            return DICT_AnimalToMarkerState[_type];
        else
            return null;
    }


    protected override void F_InitContainer()
    {
        //파일명 초기화 
        FileName = "PlayerAnimal";

        // 컨테이너 초기화
        DICT_AnimalToMarkerState = new Dictionary<AnimalType, PlayerAnimalState>();
    }

    protected override void F_ProcessData(string[] _data)
    {
        // Marker 생성
        PlayerAnimalState _marker = new PlayerAnimalState(_data);

        // DICT에 넣기 
        try
        {
            DICT_AnimalToMarkerState.Add(_marker.markerPlayerType, _marker);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }
}
