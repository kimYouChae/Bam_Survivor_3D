using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == GameManager.instance.mapPropsLayer) 
        {
            GameManager.instance.F_SetGridMap((int)transform.position.x, (int)transform.position.z, true);
            
            Destroy(gameObject);
        }
    }
}
