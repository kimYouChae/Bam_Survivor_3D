using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // 싱글톤
    public static UnitManager Instance;

    [Header("===LayerMask===")]
    [SerializeField] private LayerMask _unitLayer;

    [Header("===Trasform===")]
    [SerializeField] private Transform[] _unitGenerator;
    // ##TODO : 생성 나중에 로직 바꿔야함 

    // 프로퍼티
    public LayerMask unitLayer => _unitLayer;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _unitLayer = LayerMask.GetMask("Unit");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
