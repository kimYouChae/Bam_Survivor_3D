using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : Singleton<UnitManager>
{
    [Header("===Trasform===")]
    [SerializeField] private List<Transform> _spawner;

    [Header("===Scipt===")]
    [SerializeField] private UnitGenerator _unitGenerator;
    [SerializeField] private UnitPooling _unitPooling;
    [SerializeField] private UnitCsvImporter _unitCsvImporter;

    [Header("===Bullet===")]
    [SerializeField] private GameObject _unitBullet;
    
    // 프로퍼티
    public UnitGenerator UnitGenerator => _unitGenerator;
    public UnitPooling UnitPooling => _unitPooling;
    public UnitCsvImporter UnitCsvImporter => _unitCsvImporter;
    public GameObject UnitBullet => _unitBullet;    

    protected override void Singleton_Awake()
    {
        _spawner = new List<Transform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) 
        {
            GameObject _insUnit = _unitPooling.F_GetUnit(Unit_Animal_Type.Pig);
            F_ObjectOnOffNavmesh(_insUnit, GameManager.Instance._unitTestGeneration);
        }
    }

    // stage 정보에 맞게 
    private void F_EnemyInstanceByStage() 
    {
        Stage _myState = StageManager.Instance.F_CurrentStage();

        // animal type 갯수
        int _unitTypeCount      = _myState.GenerateUnitList.Count;
        int _unitInstanceCount  = _myState.GenerateCount / _unitTypeCount;

        // animal type 만큼 스포너 
        _spawner = _unitGenerator.F_GetClosetSpawner(_unitTypeCount);

        Debug.Log("생성할 동물 type 갯수 : " + _unitTypeCount + " 각 동물이 생성할 count : " + _unitInstanceCount);
        Debug.Log("선택된 스포너 카운터" + _spawner.Count);

        for (int i = 0; i < _spawner.Count; i++)
        {
            for (int j = 0; j < _unitInstanceCount; j++) 
            {
                GameObject _insUnit = _unitPooling.F_GetUnit(_myState.GenerateUnitList[i]);
                F_ObjectOnOffNavmesh(_insUnit, _spawner[i].transform);
            }
        }
    }

    // 위치를 바꾸기 위한 navmesh on off
    public void F_ObjectOnOffNavmesh(GameObject _obj , Transform _trs) 
    {
        _obj.GetComponent<NavMeshAgent>().enabled = false;
        _obj.transform.position = _trs.position;
        _obj.GetComponent<NavMeshAgent>().enabled = true;
    }
}
