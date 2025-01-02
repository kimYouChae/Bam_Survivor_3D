using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Random = UnityEngine.Random;

public class PropsBuildingManager : Singleton<PropsBuildingManager>
{
    [Header("===InGame Props Building===")]
    [SerializeField] private Dictionary<CropsType, int> DICT_inGamePropsToCount;     // 인게임내에서 획득한 props To Count

    [Header("===Props Building Init===")]
    [SerializeField]
    private CropsType[] _inGamePropsStateList;
    // [0] : crystal
    // [1] [2] [3] : 작물
    [SerializeField]
    private GameObject[] _buildingPrdfab;       // building 프리팹
    // [0] Rice [1] Tomato [2] Carrot (enum 순서대로)
    [SerializeField]
    private PropsField[] _propsField;           // Field 오브젝트
    [SerializeField]
    private List<PropsBuilding> _propsBuilding; // Building 스크립트 


    [Header("===Sciprt===")]
    [SerializeField]
    private PropsCsvImporter _propsCsvImporter;
    [SerializeField]
    private CropsPooling _cropsPooling;

    // 프로퍼티
    public CropsPooling CropsPooling { get => _cropsPooling; }

    protected override void Singleton_Awake()
    {
        DICT_inGamePropsToCount = new Dictionary<CropsType, int>();

        _inGamePropsStateList = (CropsType[])System.Enum.GetValues(typeof(CropsType));
    }

    private void Start()
    {
        // 초기화 
        F_SetUpBuidling();
    }

    // 인게임 내에서 획득한 props
    public void F_GetProps(CropsType _state)
    {
        if (!DICT_inGamePropsToCount.ContainsKey(_state))
        {
            DICT_inGamePropsToCount.Add(_state, 0);
        }

        // 획득 count ++ 
        DICT_inGamePropsToCount[_state]++;
    }

    private void F_SetUpBuidling() 
    {
        // 랜덤으로 state 섞기
        F_SuffleAlgorithm(ref _inGamePropsStateList);
        // [0] : crystal
        // [1] [2] [3] : 작물

        // 1. 건물생성
        for (int i = 0; i < _inGamePropsStateList.Length; i++) 
        {
            int enumIdx = (int)_inGamePropsStateList[i];

            // enum에 해당하는 프리팹 생성
            GameObject _obj = Instantiate(_buildingPrdfab[enumIdx] 
                , _propsField[enumIdx].buildingTransform);

            // List에 넣기 
            _propsBuilding.Add(_obj.GetComponent<PropsBuilding>());
        }

        // 2. Building State 넣어주기
        for(int i = 0; i < _inGamePropsStateList.Length; i++) 
        {
            // type 별 클래스 
            Building _build = _propsCsvImporter.F_StateToBuilding(_inGamePropsStateList[i] );

            // build 클래스 넣어주기 
            _propsBuilding[i].F_SetBuildingState( _build ); 
        }
    }

    private void F_SuffleAlgorithm(ref CropsType[] _array) 
    {
        for (int i = 0; i < _array.Length - 1; i++) 
        {
            // 내 뒤로 랜덤인덱스
            int _ranIndex = Random.Range(i, _array.Length);

            // 현재 위치와 랜덤하게 섞인 위치 교환
            CropsType _state = _array[i];
            _array[i] = _array[_ranIndex];
            _array[_ranIndex] = _state;
        }
    }

}
