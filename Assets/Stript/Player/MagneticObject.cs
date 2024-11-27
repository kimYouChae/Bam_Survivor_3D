using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        // exp ¿œ ∂ß 
        if (other.gameObject.layer == LayerManager.Instance.expLayerNum) 
        {
            other.gameObject.transform.position
                    = Vector3.Lerp(other.gameObject.transform.position, gameObject.transform.position, 3 * Time.deltaTime);
        }

    }
}
