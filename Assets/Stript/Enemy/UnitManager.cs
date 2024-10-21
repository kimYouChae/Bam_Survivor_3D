using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // �̱���
    public static UnitManager Instance;

    [Header("===LayerMask===")]
    [SerializeField] private LayerMask _unitLayer;
    [SerializeField] private LayerMask _chunckBoundaryLayer;
    [SerializeField] private int _unitLayerInt;

    [Header("===Trasform===")]
    [SerializeField] private Transform[] _unitGenerator;
    // ##TODO : ���� ���߿� ���� �ٲ���� 

    // ������Ƽ
    public LayerMask unitLayer => _unitLayer;
    public LayerMask chunckBoundary => _chunckBoundaryLayer; 
    public int unitLayerInt => _unitLayerInt;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _unitLayer              = LayerMask.GetMask("Unit");
        _chunckBoundaryLayer    = LayerMask.GetMask("ChunckBoundary");
        _unitLayerInt           = LayerMask.NameToLayer("Unit");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
