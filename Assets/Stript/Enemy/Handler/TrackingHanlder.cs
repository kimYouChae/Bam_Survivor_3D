using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class TrackingHanlder : ITrackingHandler
{
    [Header("===Unit===")]
    private Unit _unit;

    [Header("===Tracking===")]
    [SerializeField] private NavMeshAgent _unitAgent    = null;
    [SerializeField] private Vector3 _destiPosition     = Vector3.zero;
    [SerializeField] private Vector2 _playerPos2D       = Vector2.zero;
    [SerializeField] private Vector2 _unitPos2D         = Vector2.zero;

    [Header("===네비게이션 쿨타임===")]
    [SerializeField] const float _navActionCoolDown = 1f;

    // 생성자
    public TrackingHanlder(Unit _unit) 
    {
        this._unit = _unit;
    }

    public IEnumerator IE_TrackinCorutine()
    {

        if (_unit.gameObject.GetComponent<NavMeshAgent>() == null)
        {
            Debug.LogError("Unit의 agent가 null");
            yield return null;
        }
        else
        {
            _unitAgent = _unit.gameObject.GetComponent<NavMeshAgent>();
        }

        // update문 효과 
        while (true)
        {
            // 근처에 navMesh가 있을때만
            if (TH_CheckIsOnNavMesh())
            {
                // marker의 첫번째 위치를 목적지고
                _destiPosition = PlayerManager.Instance.markerHeadTrasform.position;

                Debug.Log(_unit.gameObject.name + "의 도착지 + " + _destiPosition);

                // agent의 도착지 잡아주기 
                _unitAgent.SetDestination(_destiPosition);
            }

            yield return new WaitForSeconds(_navActionCoolDown);
        }
    }

    // Traking에서 상태를 변화하는 평가 (기준)
    // marker(플레이어)가 범위안에 들어오면 changeState
    public void TH_EvaluateStateTransition()
    {
        _playerPos2D.x = PlayerManager.Instance.markers[0].transform.position.x;
        _playerPos2D.y = PlayerManager.Instance.markers[0].transform.position.z;

        _unitPos2D.x = _unit.transform.position.x;
        _unitPos2D.y = _unit.transform.position.z;

        //Debug.Log(Vector2.Distance(_playerPos2D, _unitPos2D));

        // 거리가 searchRadious보다 작으면 
        if (Vector2.Distance(_playerPos2D, _unitPos2D) <= _unit.unitSearchRadious)
        {
            _unit.F_ChangeState(UNIT_STATE.Attack);
        }
    }

    // 근처에 navMesh가 있는지 체크
    private bool TH_CheckIsOnNavMesh()
    {
        NavMeshHit hit;

        // 근처에서 1.0 안에 navmesh가 있는지 
        if (NavMesh.SamplePosition(_unit.transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            float _distanceToMesh = Vector3.Distance(_unit.transform.position, hit.position);

            // 범위오차 : 0.1f
            if (_distanceToMesh < 0.1f)
            {
                Debug.Log("Agent가 Navmesh위에 있습니다");
                return true;
            }
            else
            {
                Debug.Log("Agent가 Navmesh 밖에 있습니다.");
                return false;
            }
        }

        // 근처에 아얘 없으면 
        Debug.Log("근처에 Navmesh가 없습니다.");
        return false;

    }


}
