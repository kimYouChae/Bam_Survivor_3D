using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    [Header("===Trasform===")]
    [SerializeField] private Transform[] _unitGenerator;
    // ##TODO : ���� ���߿� ���� �ٲ���� 

    protected override void Singleton_Awake()
    {

    }


}
