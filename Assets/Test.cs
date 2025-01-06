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
            t.name = "sampleposition으로 생긴 오브젝트 ";

            // 결과값 : 가장 가까운 navmesh에서 오브젝트 생김 
        }
        else 
        {
            Debug.Log("안됨");
        }

    }

}
