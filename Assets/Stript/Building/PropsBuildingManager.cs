using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PropsBuildingManager : Singleton<PropsBuildingManager>
{
    [Header("===InGame Props Building===")]
    [SerializeField] private Dictionary<InGamePropState, int> DICT_inGamePropsToCount;     // �ΰ��ӳ����� ȹ���� props To Count
    [SerializeField]
    private Transform[] _buildingInitTrs;       // building ������ ��ġ 
    [SerializeField]
    private GameObject[] _buildingPrepabs;      // building ������
    [SerializeField]
    private InGamePropState[] _inGamePropsStateList;
    [SerializeField]
    private Transform _buildingParnet;          // ������ building��Ƶ� �θ� 

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
        // �ʱ�ȭ 
        F_SetUpBuidling();
    }

    // �ΰ��� ������ ȹ���� props
    public void F_GetProps(InGamePropState _state)
    {
        if (!DICT_inGamePropsToCount.ContainsKey(_state))
        {
            DICT_inGamePropsToCount.Add(_state, 0);
        }

        // ȹ�� count ++ 
        DICT_inGamePropsToCount[_state]++;
    }

    private void F_SetUpBuidling() 
    {
        // �������� building ��ġ 
        F_SuffleAlgorithm(ref _inGamePropsStateList);

        for (int i = 0; i < _inGamePropsStateList.Length; i++) 
        {
            int _nowBuildingIdx = (int)_inGamePropsStateList[i];

            // �������� ���� type�� �ش��ϴ� ������ ����, ��ġ�� 0���� �������
            GameObject _buil = Instantiate(_buildingPrepabs[_nowBuildingIdx] , _buildingInitTrs[i]);
            _buil.transform.SetParent(_buildingParnet);

            // PropsBuilding ��ũ��Ʈ �߰�
            if (_buil.GetComponent<PropsBuilding>() == null )
                _buil.AddComponent<PropsBuilding>();

            // csv Importer�� ����� Building Ŭ���� �Ҵ��ϱ�
            Building _curBuild = _propsCsvImporter.F_StateToBuilding(_inGamePropsStateList[i]);
            _buil.GetComponent<PropsBuilding>().F_SetBuildingState(_curBuild);

            // navMesh Obstacle ������ �߰� 
            if (_buil.GetComponent<NavMeshObstacle>() == null)
                _buil.AddComponent<NavMeshObstacle>();
            
            // curve üũ 
            _buil.GetComponent<NavMeshObstacle>().carving = true;
        }
    }

    private void F_SuffleAlgorithm(ref InGamePropState[] _array) 
    {
        for (int i = 0; i < _array.Length - 1; i++) 
        {
            // �� �ڷ� �����ε���
            int _ranIndex = Random.Range(i, _array.Length);

            // ���� ��ġ�� �����ϰ� ���� ��ġ ��ȯ
            InGamePropState _state = _array[i];
            _array[i] = _array[_ranIndex];
            _array[_ranIndex] = _state;
        }
    }

}
