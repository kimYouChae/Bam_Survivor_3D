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
        //���ϸ� �ʱ�ȭ 
        FileName = "PlayerAnimal";

        // �����̳� �ʱ�ȭ
        DICT_AnimalToMarkerState = new Dictionary<string, MarkerState>();
    }

    protected override void F_ProcessData(string[] _data)
    {
        // Marker ����
        MarkerState _marker = new MarkerState(_data);

        // DICT�� �ֱ� 
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
