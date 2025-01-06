using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    [SerializeField]
    private Vector3 _pos;


    private void Start()
    {
        NavMeshHit _hit;

        if (NavMesh.SamplePosition(_pos, out _hit, 10f, NavMesh.AllAreas)) 
        {
            GameObject t = Instantiate(GameManager.Instance.emptyObject , _hit.position , Quaternion.identity);
            t.name = "sampleposition���� ���� ������Ʈ ";

            // ����� : ���� ����� navmesh���� ������Ʈ ���� 
        }
        else 
        {
            Debug.Log("�ȵ�");
        }

    }

}
