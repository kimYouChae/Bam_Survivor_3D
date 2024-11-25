using System.Collections;
using System.Collections.Generic;
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

    
    // ������Ƽ
    public UnitGenerator UnitGenerator => _unitGenerator;
    public UnitPooling UnitPooling => _unitPooling;
    public UnitCsvImporter UnitCsvImporter => _unitCsvImporter;

    protected override void Singleton_Awake()
    {
        _spawner = new List<Transform>();
    }

    private void Start()
    {
        StartCoroutine(IE_Test());
    }

    IEnumerator IE_Test() 
    {
        yield return new WaitForSeconds(1f);
        F_EnemyInstanceByStage();

    }

    // stage ������ �°� 
    private void F_EnemyInstanceByStage() 
    {
        Stage _myState = StageManager.Instance.F_CurrentStage();

        // animal type ����
        int _unitTypeCount      = _myState.GenerateUnitList.Count;
        int _unitInstanceCount  = _myState.GenerateCount / _unitTypeCount;

        // animal type ��ŭ ������ 
        _spawner = _unitGenerator.F_GetClosetSpawner(_unitTypeCount);

        Debug.Log("������ ���� type ���� : " + _unitTypeCount + " �� ������ ������ count : " + _unitInstanceCount);
        Debug.Log("���õ� ������ ī����" + _spawner.Count);

        for (int i = 0; i < _spawner.Count; i++)
        {
            for (int j = 0; j < _unitInstanceCount; j++) 
            {
                GameObject _insUnit = _unitPooling.F_GetUnit(_myState.GenerateUnitList[i]);
                F_ObjectOnOffNavmesh(_insUnit, _spawner[i].transform);
            }
        }
    }

    // ��ġ�� �ٲٱ� ���� navmesh on off
    public void F_ObjectOnOffNavmesh(GameObject _obj , Transform _trs) 
    {
        _obj.GetComponent<NavMeshAgent>().enabled = false;
        _obj.transform.position = _trs.position;
        _obj.GetComponent<NavMeshAgent>().enabled = true;
    }
}
