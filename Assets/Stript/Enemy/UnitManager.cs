using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // 싱글톤
    public static UnitManager Instance;

    [Header("===LayerMask===")]
    [SerializeField] private LayerMask _unitLayer;
    [SerializeField] private LayerMask _chunckBoundary;

    [Header("===Trasform===")]
    [SerializeField] private Transform[] _unitGenerator;
    // ##TODO : 생성 나중에 로직 바꿔야함 

    // 프로퍼티
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
