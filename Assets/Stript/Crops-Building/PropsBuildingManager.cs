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
    [SerializeField] // [0] Rice [1] Tomato [2] Carrot (enum 순서대로)
    private CropsType[] _inGamePropsStateList;

    [SerializeField] // [0] Rice [1] Tomato [2] Carrot (enum 순서대로)
    private GameObject[] _buildingPrdfab;       // building 프리팹
    
    [SerializeField]
    private PropsField[] _propsField;           // Field 오브젝트

    [SerializeField]
    private List<PropsBuilding> _propsBuilding; // Building 스크립트 

    [SerializeField] // [0] Rice [1] Tomato [2] Carrot (enum 순서대로)
    private List<GameObject> _cropsPrefab;      // 작물 prefab


    protected override void Singleton_Awake()
    {
        DICT_inGamePropsToCount = new Dictionary<CropsType, int>();

        _inGamePropsStateList = (CropsType[])System.Enum.GetValues(typeof(CropsType));
    }

    private void Start()
    {
        // building 초기화 
        F_SetUpBuidling();

        // Field 초기화
        F_SetUpField();
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

    private void F_SetUpField() 
    {
        for (int i = 0; i < _inGamePropsStateList.Length; i++) 
        {
            int idx = (int)_inGamePropsStateList[i];

            // 1. Field에 PropsBuilding 스크립트 넣기 
            PropsBuilding _building = _propsBuilding[i+1];
            _propsField[i].F_SetUpBuilding(_building);

            // 2. Field에 enum 과 오브젝트 넘겨주기 
            _propsField[i].F_PlantProps(_inGamePropsStateList[i] , _cropsPrefab[idx]);

        }
    }

    private void F_SetUpBuidling() 
    {
        // 랜덤으로 state 섞기
        F_SuffleAlgorithm(ref _inGamePropsStateList);
        // [0] [1] [2] : 작물

        // 1. 건물생성
        for (int i = 0; i < _inGamePropsStateList.Length; i++) 
        {
            int enumIdx = (int)_inGamePropsStateList[i];

            // enum에 해당하는 프리팹 생성
            GameObject _obj = Instantiate(_buildingPrdfab[enumIdx] 
                , _propsField[i].buildingTransform);

            // List에 넣기 
            _propsBuilding.Add(_obj.GetComponent<PropsBuilding>());
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
