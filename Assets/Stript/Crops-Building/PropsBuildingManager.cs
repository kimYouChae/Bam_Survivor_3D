using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Random = UnityEngine.Random;

public class PropsBuildingManager : Singleton<PropsBuildingManager>
{
    [Header("===InGame Props Building===")]
    [SerializeField] private Dictionary<CropsType, int> DICT_inGamePropsToCount;     // �ΰ��ӳ����� ȹ���� props To Count

    [Header("===Props Building Init===")]
    [SerializeField]
    private CropsType[] _inGamePropsStateList;
    // [0] : crystal
    // [1] [2] [3] : �۹�
    [SerializeField]
    private GameObject[] _buildingPrdfab;       // building ������
    // [0] Rice [1] Tomato [2] Carrot (enum �������)
    [SerializeField]
    private PropsField[] _propsField;           // Field ������Ʈ
    [SerializeField]
    private List<PropsBuilding> _propsBuilding; // Building ��ũ��Ʈ 


    [Header("===Sciprt===")]
    [SerializeField]
    private PropsCsvImporter _propsCsvImporter;
    [SerializeField]
    private CropsPooling _cropsPooling;

    // ������Ƽ
    public CropsPooling CropsPooling { get => _cropsPooling; }

    protected override void Singleton_Awake()
    {
        DICT_inGamePropsToCount = new Dictionary<CropsType, int>();

        _inGamePropsStateList = (CropsType[])System.Enum.GetValues(typeof(CropsType));
    }

    private void Start()
    {
        // �ʱ�ȭ 
        F_SetUpBuidling();
    }

    // �ΰ��� ������ ȹ���� props
    public void F_GetProps(CropsType _state)
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
        // �������� state ����
        F_SuffleAlgorithm(ref _inGamePropsStateList);
        // [0] : crystal
        // [1] [2] [3] : �۹�

        // 1. �ǹ�����
        for (int i = 0; i < _inGamePropsStateList.Length; i++) 
        {
            int enumIdx = (int)_inGamePropsStateList[i];

            // enum�� �ش��ϴ� ������ ����
            GameObject _obj = Instantiate(_buildingPrdfab[enumIdx] 
                , _propsField[enumIdx].buildingTransform);

            // List�� �ֱ� 
            _propsBuilding.Add(_obj.GetComponent<PropsBuilding>());
        }

        // 2. Building State �־��ֱ�
        for(int i = 0; i < _inGamePropsStateList.Length; i++) 
        {
            // type �� Ŭ���� 
            Building _build = _propsCsvImporter.F_StateToBuilding(_inGamePropsStateList[i] );

            // build Ŭ���� �־��ֱ� 
            _propsBuilding[i].F_SetBuildingState( _build ); 
        }
    }

    private void F_SuffleAlgorithm(ref CropsType[] _array) 
    {
        for (int i = 0; i < _array.Length - 1; i++) 
        {
            // �� �ڷ� �����ε���
            int _ranIndex = Random.Range(i, _array.Length);

            // ���� ��ġ�� �����ϰ� ���� ��ġ ��ȯ
            CropsType _state = _array[i];
            _array[i] = _array[_ranIndex];
            _array[_ranIndex] = _state;
        }
    }

}
