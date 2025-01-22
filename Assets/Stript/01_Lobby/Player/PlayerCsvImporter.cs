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

    // type�� ���� state return
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
        //���ϸ� �ʱ�ȭ 
        FileName = "PlayerAnimal";

        // �����̳� �ʱ�ȭ
        DICT_AnimalToMarkerState = new Dictionary<AnimalType, PlayerAnimalState>();
    }

    protected override void F_ProcessData(string[] _data)
    {
        // Marker ����
        PlayerAnimalState _marker = new PlayerAnimalState(_data);

        // DICT�� �ֱ� 
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
