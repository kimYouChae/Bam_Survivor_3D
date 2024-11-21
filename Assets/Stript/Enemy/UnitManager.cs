using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    [Header("===Trasform===")]
    [SerializeField] private Transform[] _unitGenerator;
    // ##TODO : 생성 나중에 로직 바꿔야함 

    [Header("===Scipt===")]
    [SerializeField] private UnitPooling _unitPooling;
    [SerializeField] private UnitCsvImporter _unitCsvImporter;

    // 프로퍼티
    public UnitPooling UnitPooling => _unitPooling;
    public UnitCsvImporter UnitCsvImporter => _unitCsvImporter;

    protected override void Singleton_Awake()
    {

    }


}
