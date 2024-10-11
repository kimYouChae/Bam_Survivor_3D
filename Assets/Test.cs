using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    NavMeshAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        
        _agent.SetDestination(PlayerManager.instance.markers[0].transform.position);
         
    }

    // Update is called once per frame
    void Update()
    {
    }
}
