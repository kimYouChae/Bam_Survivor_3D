using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Random = UnityEngine.Random;

public class PropsBuildingManager : Singleton<PropsBuildingManager>
{
    [Header("===InGame Props Building===")]
    [SerializeField] private Dictionary<InGamePropState, int> DICT_inGamePropsToCount;     // �ΰ��ӳ����� ȹ���� props To Count

    [Header("===Props Building Init===")]
    [SerializeField]
    private InGamePropState[] _inGamePropsStateList;
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
        // �������� state ����
        F_SuffleAlgorithm(ref _inGamePropsStateList);
        // [0] : crystal
        // [1] [2] [3] : �۹�

        // 1. �ǹ�����
        for (int i = 1; i < _inGamePropsStateList.Length; i++) 
        {
            // enum�� �ش��ϴ� ������ ����
            GameObject _obj = Instantiate(_buildingPrdfab[ (int)_inGamePropsStateList[i] ] 
                , _propsField[i-1].buildingTransform);

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

    private void F_SuffleAlgorithm(ref InGamePropState[] _array) 
    {
        // [0]�� crystal , ����
        for (int i = 1; i < _array.Length - 1; i++) 
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
