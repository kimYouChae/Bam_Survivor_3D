using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    [Header("===Trasform===")]
    [SerializeField] private Transform[] _unitGenerator;
    // ##TODO : ���� ���߿� ���� �ٲ���� 

    [Header("===Scipt===")]
    [SerializeField] private UnitPooling _unitPooling;
    [SerializeField] private UnitCsvImporter _unitCsvImporter;

    // ������Ƽ
    public UnitPooling UnitPooling => _unitPooling;
    public UnitCsvImporter UnitCsvImporter => _unitCsvImporter;

    protected override void Singleton_Awake()
    {

    }


}
