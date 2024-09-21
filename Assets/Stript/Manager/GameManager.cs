using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("===Ratio / 0~1사이 ===")]
    [SerializeField] private float _basicRatio;
    [SerializeField] private float _commonRatio;
    [SerializeField] private float _rareRatio;
    [SerializeField] private float _epicRatio;
    [SerializeField] private float _legendaryRatio;

    [Header("===Eenemy Generator Time===")]
    [SerializeField] private float _unitGenerateTime;

    [Header("===LayerMask===")]
    [SerializeField] private int _mapPropsLayer;

    [Header("===GridMap===")]
    [SerializeField] private bool[ , ] _mapGrid;
    private int _mapSize = 140;
    
    //프로퍼티
    public float BasicRatio => _basicRatio;
    public float CommonRatio => _commonRatio;
    public float RareRatio => _rareRatio;
    public float EpicRatio => _epicRatio;
    public float LegaryRatio => _legendaryRatio;
    public float unitGenerateTime => _unitGenerateTime;
    public int mapPropsLayer => _mapPropsLayer;
    public bool[,] mapGrid => _mapGrid;
    public int mapSize => _mapSize;

    private void Awake()
    {
        instance = this;

        _mapPropsLayer = LayerMask.NameToLayer("MapProps");

        // 기본값 false , 방문 시 true 
        _mapGrid = new bool[_mapSize + 1 ,_mapSize + 1];
    }

    public void F_SetGridMap(int v_x , int v_z , bool v_flag) 
    {
        _mapGrid[v_x , v_z] = v_flag;
    }
    
}
