using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : Singleton<UnitManager>
{
    [Header("===Scipt===")]
    [SerializeField] private UnitGenerator _unitGenerator;
    [SerializeField] private UnitPooling _unitPooling;
    [SerializeField] private UnitBulletPooling _bulletPooling;
    [SerializeField] private UnitCsvImporter _unitCsvImporter;

    [Header("===Bullet===")]
    [SerializeField] private GameObject _unitBullet;
    
    // 프로퍼티
    public UnitGenerator UnitGenerator => _unitGenerator;
    public UnitPooling UnitPooling => _unitPooling;
    public UnitBulletPooling UnitBulletPooling => _bulletPooling;
    public UnitCsvImporter UnitCsvImporter => _unitCsvImporter;
    public GameObject UnitBullet => _unitBullet;    

    protected override void Singleton_Awake()
    {
        
    }


}
