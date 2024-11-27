using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PropsBuildingManager : Singleton<PropsBuildingManager>
{
    [Header("===InGame Props Building===")]
    [SerializeField] private Dictionary<InGamePropState, int> DICT_inGamePropsToCount;     // 인게임내에서 획득한 props To Count
    [SerializeField]
    private Transform[] _buildingInitTrs;       // building 생성할 위치 
    [SerializeField]
    private GameObject[] _buildingPrepabs;      // building 프리팹
    [SerializeField]
    private InGamePropState[] _inGamePropsStateList;
    [SerializeField]
    private Transform _buildingParnet;          // 생성할 building담아둘 부모 

    [Header("===Sciprt===")]
    [SerializeField]
    private PropsCsvImporter _propsCsvImporter;

    protected override void Singleton_Awake()
    {
        DICT_inGamePropsToCount = new Dictionary<InGamePropState, int>();

        _inGamePropsStateList = (InGamePropState[])System.Enum.GetValues(typeof(InGamePropState));
    }

    private void Start()
    {
        // 초기화 
        F_SetUpBuidling();
    }

    // 인게임 내에서 획득한 props
    public void F_GetProps(InGamePropState _state)
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
        // 랜덤으로 building 설치 
        F_SuffleAlgorithm(ref _inGamePropsStateList);

        for (int i = 0; i < _inGamePropsStateList.Length; i++) 
        {
            int _nowBuildingIdx = (int)_inGamePropsStateList[i];

            // 랜덤으로 섞인 type에 해당하는 프리팹 생성, 위치는 0부터 순서대로
            GameObject _buil = Instantiate(_buildingPrepabs[_nowBuildingIdx] , _buildingInitTrs[i]);
            _buil.transform.SetParent(_buildingParnet);

            // PropsBuilding 스크립트 추가
            if (_buil.GetComponent<PropsBuilding>() == null )
                _buil.AddComponent<PropsBuilding>();

            // csv Importer에 저장된 Building 클래스 할당하기
            Building _curBuild = _propsCsvImporter.F_StateToBuilding(_inGamePropsStateList[i]);
            _buil.GetComponent<PropsBuilding>().F_SetBuildingState(_curBuild);

            // navMesh Obstacle 없으면 추가 
            if (_buil.GetComponent<NavMeshObstacle>() == null)
                _buil.AddComponent<NavMeshObstacle>();
            
            // curve 체크 
            _buil.GetComponent<NavMeshObstacle>().carving = true;
        }
    }

    private void F_SuffleAlgorithm(ref InGamePropState[] _array) 
    {
        for (int i = 0; i < _array.Length - 1; i++) 
        {
            // 내 뒤로 랜덤인덱스
            int _ranIndex = Random.Range(i, _array.Length);

            // 현재 위치와 랜덤하게 섞인 위치 교환
            InGamePropState _state = _array[i];
            _array[i] = _array[_ranIndex];
            _array[_ranIndex] = _state;
        }
    }

}
