using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class UnitGenerator : MonoBehaviour
{
    [Header("===Spawn Point===")]
    [SerializeField] private float _xOffset = 12f;
    [SerializeField] private float _yOffset = 6f;
    private float _markerX;
    private float _markerY;

    [Header("===Stage===")]
    [SerializeField] private Stage _currState;

    [Header("===Navmesh===")]
    [SerializeField] private NavMeshHit _hit;

    private void Start()
    {
        _currState = StageManager.Instance.F_CurrentStage();

        // �ʱ�ȭ�� start���� �ϵ��� ��Ȱȭ �սô� �������������� 
        _xOffset = 12;
        _yOffset = 6f;
        _markerX = 0;
        _markerY = 0;

        StartCoroutine(IE_Test());
    }

   private IEnumerator IE_Test() 
    {
        yield return new WaitForSeconds(1f);
        // �׽�Ʈ
        F_EnemyInstanceByStage();
    }

    // stage ������ �°� 
    private void F_EnemyInstanceByStage()
    {
        // ���� �������� 
        _currState = StageManager.Instance.F_CurrentStage();

        // animal type ����
        List<Unit_Animal_Type> _unitTypeCount       = _currState.GenerateUnitList;
        int _unitInstanceCount                      = _currState.GenerateCount / _unitTypeCount.Count;

        // ���� marker ��ġ
        Transform _markerTrs = PlayerManager.Instance.markerHeadTrasform;

        _markerX = _markerTrs.position.x;
        _markerY = _markerTrs.position.z;

        for (int i = 0; i < _unitTypeCount.Count; i++)
        {
            Tuple<float, float> _randPosition = F_RandomPotision();

            for (int j = 0; j < _unitInstanceCount; j++)
            {
                // type�� �´� ������Ʈ get
                GameObject _insUnit = UnitManager.Instance.UnitPooling.F_GetUnit(_unitTypeCount[i]);

                // ��ġ �������ֱ�
                F_ObjectOnOffNavmesh(_insUnit , _randPosition);
            }
        }
    }

    // ���� ��ġ return
    private Tuple<float, float> F_RandomPotision()
    {
        // ������ġ�� ���ϱ� ���� 
        int _boundaryDir = Random.Range(0, 4);

        float _randRanX = 0;
        float _randRanY = 0;

        // ������� x,y ������ ���ϱ� 
        switch (_boundaryDir) 
        {
            // ��
            case 0:
                _randRanX = Random.Range(_markerX - _xOffset, _markerX + _xOffset);
                _randRanY = _markerY + _yOffset;
                Debug.Log($"{_randRanX} / {_randRanY}");
                break;
            // ����
            case 1:
                _randRanX = _markerX + _xOffset;
                _randRanY = Random.Range(_markerY - _yOffset, _markerY + _yOffset);
                break;
            // �Ʒ�
            case 2:
                _randRanX = Random.Range(_markerX - _xOffset , _markerX + _xOffset);
                _randRanY = _markerY - _yOffset;
                break; 
            // ��
            case 3:
                _randRanX = _markerX - _xOffset;
                _randRanY = Random.Range(_markerY - _yOffset, _markerY + _yOffset);
                break;
        }

        // 0�� �Ѱų� max�� ������ �ȵ�
        //_randRanX = Math.Clamp(_randRanX , 0 , GameManager.Instance.MAP_SIZE);
        //_randRanY = Math.Clamp(_randRanY , 0 , GameManager.Instance.MAP_SIZE);

        return F_NavMeshSample(_randRanX,_randRanY);
    }

    private Tuple<float, float> F_NavMeshSample(float x, float y)
    {
        Vector3 _pos = new Vector3(x, 0, y);

        // ��ġ �������� , 10f�ȿ��ִ� navmeshã��
        if (NavMesh.SamplePosition(_pos, out _hit, 10f, NavMesh.AllAreas))
        {
            return new Tuple<float, float>(_hit.position.x, _hit.position.z);
        }

        else
        { 
            return new Tuple<float, float>(0f, 0f);
        }
    }

    // ��ġ�� �ٲٱ� ���� navmesh on off
    public void F_ObjectOnOffNavmesh(GameObject _obj, Tuple<float,float> _position)
    {
        // ���� 
        _obj.GetComponent<NavMeshAgent>().enabled = false;

        // ��ġ �ٲٱ�
        _obj.transform.position = new Vector3(_position.Item1 , 0.5f, _position.Item2);

        // �ѱ� 
        _obj.GetComponent<NavMeshAgent>().enabled = true;
    }
}
