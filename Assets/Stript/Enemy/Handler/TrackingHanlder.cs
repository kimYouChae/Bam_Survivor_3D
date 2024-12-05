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

    [Header("===�׺���̼� ��Ÿ��===")]
    [SerializeField] const float _navActionCoolDown = 1f;

    // ������
    public TrackingHanlder(Unit _unit) 
    {
        this._unit = _unit;
    }

    public IEnumerator IE_TrackinCorutine()
    {

        if (_unit.gameObject.GetComponent<NavMeshAgent>() == null)
        {
            Debug.LogError("Unit�� agent�� null");
            yield return null;
        }
        else
        {
            _unitAgent = _unit.gameObject.GetComponent<NavMeshAgent>();
        }

        // update�� ȿ�� 
        while (true)
        {
            // ��ó�� navMesh�� ��������
            if (TH_CheckIsOnNavMesh())
            {
                // marker�� ù��° ��ġ�� ��������
                _destiPosition = PlayerManager.Instance.markerHeadTrasform.position;

                Debug.Log(_unit.gameObject.name + "�� ������ + " + _destiPosition);

                // agent�� ������ ����ֱ� 
                _unitAgent.SetDestination(_destiPosition);
            }

            yield return new WaitForSeconds(_navActionCoolDown);
        }
    }

    // Traking���� ���¸� ��ȭ�ϴ� �� (����)
    // marker(�÷��̾�)�� �����ȿ� ������ changeState
    public void TH_EvaluateStateTransition()
    {
        _playerPos2D.x = PlayerManager.Instance.markers[0].transform.position.x;
        _playerPos2D.y = PlayerManager.Instance.markers[0].transform.position.z;

        _unitPos2D.x = _unit.transform.position.x;
        _unitPos2D.y = _unit.transform.position.z;

        //Debug.Log(Vector2.Distance(_playerPos2D, _unitPos2D));

        // �Ÿ��� searchRadious���� ������ 
        if (Vector2.Distance(_playerPos2D, _unitPos2D) <= _unit.unitSearchRadious)
        {
            _unit.F_ChangeState(UNIT_STATE.Attack);
        }
    }

    // ��ó�� navMesh�� �ִ��� üũ
    private bool TH_CheckIsOnNavMesh()
    {
        NavMeshHit hit;

        // ��ó���� 1.0 �ȿ� navmesh�� �ִ��� 
        if (NavMesh.SamplePosition(_unit.transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            float _distanceToMesh = Vector3.Distance(_unit.transform.position, hit.position);

            // �������� : 0.1f
            if (_distanceToMesh < 0.1f)
            {
                Debug.Log("Agent�� Navmesh���� �ֽ��ϴ�");
                return true;
            }
            else
            {
                Debug.Log("Agent�� Navmesh �ۿ� �ֽ��ϴ�.");
                return false;
            }
        }

        // ��ó�� �ƾ� ������ 
        Debug.Log("��ó�� Navmesh�� �����ϴ�.");
        return false;

    }


}
