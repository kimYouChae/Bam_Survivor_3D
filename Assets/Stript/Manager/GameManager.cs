using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [Header("===Ratio / 0~1사이 ===")]
    [SerializeField] private float _basicRatio;
    [SerializeField] private float _commonRatio;
    [SerializeField] private float _rareRatio;
    [SerializeField] private float _epicRatio;
    [SerializeField] private float _legendaryRatio;

    [Header("===Eenemy Generator Time===")]
    [SerializeField] private float _unitGenerateTime;

    [Header("===Pooling===")]
    [SerializeField] private GameObject _emptyObject;       // 빈 오브젝트  
    [SerializeField] private const int _POOLCOUNT = 10;     // pool에 초기 생성할 count 

    //프로퍼티
    public float BasicRatio => _basicRatio;
    public float CommonRatio => _commonRatio;
    public float RareRatio => _rareRatio;
    public float EpicRatio => _epicRatio;
    public float LegaryRatio => _legendaryRatio;
    public float unitGenerateTime => _unitGenerateTime;
    public GameObject emptyObject => _emptyObject;
    public int POOLCOUNT => _POOLCOUNT;

    protected override void Singleton_Awake()
    {

    }

    /*
    public void F_SetGridMap(int v_x , int v_z , bool v_flag) 
    {
        _mapGrid[v_x , v_z] = v_flag;
    }
    */
}
