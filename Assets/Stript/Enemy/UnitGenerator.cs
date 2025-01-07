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

        // 초기화는 start에서 하도록 생활화 합시다 하하하하하하하 
        _xOffset = 12;
        _yOffset = 6f;
        _markerX = 0;
        _markerY = 0;

        StartCoroutine(IE_Test());
    }

   private IEnumerator IE_Test() 
    {
        yield return new WaitForSeconds(1f);
        // 테스트
        F_EnemyInstanceByStage();
    }

    // stage 정보에 맞게 
    private void F_EnemyInstanceByStage()
    {
        // 현재 스테이지 
        _currState = StageManager.Instance.F_CurrentStage();

        // animal type 갯수
        List<Unit_Animal_Type> _unitTypeCount       = _currState.GenerateUnitList;
        int _unitInstanceCount                      = _currState.GenerateCount / _unitTypeCount.Count;

        // 기준 marker 위치
        Transform _markerTrs = PlayerManager.Instance.markerHeadTrasform;

        _markerX = _markerTrs.position.x;
        _markerY = _markerTrs.position.z;

        for (int i = 0; i < _unitTypeCount.Count; i++)
        {
            Tuple<float, float> _randPosition = F_RandomPotision();

            for (int j = 0; j < _unitInstanceCount; j++)
            {
                // type에 맞는 오브젝트 get
                GameObject _insUnit = UnitManager.Instance.UnitPooling.F_GetUnit(_unitTypeCount[i]);

                // 위치 설정해주기
                F_ObjectOnOffNavmesh(_insUnit , _randPosition);
            }
        }
    }

    // 랜덤 위치 return
    private Tuple<float, float> F_RandomPotision()
    {
        // 랜덤위치를 구하기 위한 
        int _boundaryDir = Random.Range(0, 4);

        float _randRanX = 0;
        float _randRanY = 0;

        // 방향따라 x,y 랜덤값 구하기 
        switch (_boundaryDir) 
        {
            // 위
            case 0:
                _randRanX = Random.Range(_markerX - _xOffset, _markerX + _xOffset);
                _randRanY = _markerY + _yOffset;
                Debug.Log($"{_randRanX} / {_randRanY}");
                break;
            // 오른
            case 1:
                _randRanX = _markerX + _xOffset;
                _randRanY = Random.Range(_markerY - _yOffset, _markerY + _yOffset);
                break;
            // 아래
            case 2:
                _randRanX = Random.Range(_markerX - _xOffset , _markerX + _xOffset);
                _randRanY = _markerY - _yOffset;
                break; 
            // 왼
            case 3:
                _randRanX = _markerX - _xOffset;
                _randRanY = Random.Range(_markerY - _yOffset, _markerY + _yOffset);
                break;
        }

        // 0을 넘거나 max를 넘으면 안됨
        //_randRanX = Math.Clamp(_randRanX , 0 , GameManager.Instance.MAP_SIZE);
        //_randRanY = Math.Clamp(_randRanY , 0 , GameManager.Instance.MAP_SIZE);

        return F_NavMeshSample(_randRanX,_randRanY);
    }

    private Tuple<float, float> F_NavMeshSample(float x, float y)
    {
        Vector3 _pos = new Vector3(x, 0, y);

        // 위치 기준으로 , 10f안에있는 navmesh찾기
        if (NavMesh.SamplePosition(_pos, out _hit, 10f, NavMesh.AllAreas))
        {
            return new Tuple<float, float>(_hit.position.x, _hit.position.z);
        }

        else
        { 
            return new Tuple<float, float>(0f, 0f);
        }
    }

    // 위치를 바꾸기 위한 navmesh on off
    public void F_ObjectOnOffNavmesh(GameObject _obj, Tuple<float,float> _position)
    {
        // 끄기 
        _obj.GetComponent<NavMeshAgent>().enabled = false;

        // 위치 바꾸기
        _obj.transform.position = new Vector3(_position.Item1 , 0.5f, _position.Item2);

        // 켜기 
        _obj.GetComponent<NavMeshAgent>().enabled = true;
    }
}
