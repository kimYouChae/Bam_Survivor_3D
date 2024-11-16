using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    [Header("===Trasform===")]
    [SerializeField] private Transform[] _unitGenerator;
    // ##TODO : 생성 나중에 로직 바꿔야함 

    protected override void Singleton_Awake()
    {

    }


}
