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
    /*
    [SerializeField]
    private PropsCsvImporter _propsCsvImporter;
    */
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
        // building �ʱ�ȭ 
        F_SetUpBuidling();

        // Field �ʱ�ȭ
        F_SetUpField();
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

    private void F_SetUpField() 
    {
        for (int i = 0; i < _inGamePropsStateList.Length; i++) 
        {
            // 1. Field�� enum �־� �ֱ� 
            _propsField[i].F_PlantProps(_inGamePropsStateList[i]);

            // 2. Field�� PropsBuilding ��ũ��Ʈ �ֱ� 
            PropsBuilding _building = _propsBuilding[i+1];
            _propsField[i].F_SetUpBuilding(_building);
        }
    }

    private void F_SetUpBuidling() 
    {
        // �������� state ����
        F_SuffleAlgorithm(ref _inGamePropsStateList);
        // [0] [1] [2] : �۹�

        // 1. �ǹ�����
        for (int i = 0; i < _inGamePropsStateList.Length; i++) 
        {
            int enumIdx = (int)_inGamePropsStateList[i];

            // enum�� �ش��ϴ� ������ ����
            GameObject _obj = Instantiate(_buildingPrdfab[enumIdx] 
                , _propsField[i].buildingTransform);

            // List�� �ֱ� 
            _propsBuilding.Add(_obj.GetComponent<PropsBuilding>());
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
