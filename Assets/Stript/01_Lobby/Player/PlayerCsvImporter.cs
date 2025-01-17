using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCsvImporter : CSVManager
{
    [Header("===Container===")]
    [SerializeField]
    private Dictionary<string, MarkerState> DICT_AnimalToMarkerState;

    protected override void F_InitContainer()
    {
        //파일명 초기화 
        FileName = "PlayerAnimal";

        // 컨테이너 초기화
        DICT_AnimalToMarkerState = new Dictionary<string, MarkerState>();
    }

    protected override void F_ProcessData(string[] _data)
    {
        // Marker 생성
        MarkerState _marker = new MarkerState(_data);

        // DICT에 넣기 
        try
        {
            DICT_AnimalToMarkerState.Add(_marker.markerName, _marker);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
