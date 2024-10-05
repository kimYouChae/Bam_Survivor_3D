using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // �̱���
    public static UnitManager Instance;

    [Header("===LayerMask===")]
    [SerializeField] private LayerMask _unitLayer;
    [SerializeField] private LayerMask _chunckBoundary;

    [Header("===Trasform===")]
    [SerializeField] private Transform[] _unitGenerator;
    // ##TODO : ���� ���߿� ���� �ٲ���� 

    // ������Ƽ
    public LayerMask unitLayer => _unitLayer;
    public LayerMask chunckBoundary => _chunckBoundary; 

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _unitLayer = LayerMask.GetMask("Unit");
        _chunckBoundary = LayerMask.GetMask("ChunckBoundary");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
