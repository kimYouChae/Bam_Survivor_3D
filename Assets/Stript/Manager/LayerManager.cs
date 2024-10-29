using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    public static LayerManager instance;

    [Header("===Layer===")]
    [SerializeField] private LayerMask _markerLayer;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _unitLayer;
    [SerializeField] private LayerMask _bulletLayer;
    [SerializeField] private LayerMask _shieldLayer;
    [SerializeField] private LayerMask _mapPropsLayer;

    [Header("===Layer int===")]
    [SerializeField] private int _markerLayerNum;
    [SerializeField] private int _wallLayerNum;
    [SerializeField] private int _unitLayerNum;
    [SerializeField] private int _bulletLayerNum;
    [SerializeField] private int _shieldLayerNum;
    [SerializeField] private int _mapPropsLayerNum;

    // 프로퍼티
    public LayerMask markerLayer => _markerLayer;
    public LayerMask wallLayer => _wallLayer;
    public LayerMask unitLayer => _unitLayer;
    public LayerMask bulletLayer => _bulletLayer;
    public LayerMask shieldLayer => _shieldLayer;
    public LayerMask mapPropsLayer => _mapPropsLayer;

    public int markerLayerNum => _markerLayerNum;
    public int wallLayerNum => _wallLayerNum;
    public int unitLayerNum => _unitLayerNum;
    public int shieldLayerNum => _shieldLayerNum;
    public int mapPropsLayerNum => _mapPropsLayerNum;   

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        // 레이어
        _markerLayer    = LayerMask.GetMask("Marker");
        _wallLayer      = LayerMask.GetMask("Wall");
        _unitLayer      = LayerMask.GetMask("Unit");
        _bulletLayer    = LayerMask.GetMask("Bullet");
        _shieldLayer    = LayerMask.GetMask("Shield");
        _mapPropsLayer  = LayerMask.GetMask("MapProps");

        // 레이어 num
        _markerLayerNum     = LayerMask.NameToLayer("Marker");
        _wallLayerNum       = LayerMask.NameToLayer("Wall");
        _unitLayerNum       = LayerMask.NameToLayer("Unit");
        _bulletLayerNum     = LayerMask.NameToLayer("Bullet");
        _shieldLayerNum     = LayerMask.NameToLayer("Shield");
        _mapPropsLayerNum   = LayerMask.NameToLayer("MapProps");

    }

}
