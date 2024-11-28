using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        // exp 일 때 
        if (other.gameObject.layer == LayerManager.Instance.expLayerNum) 
        {
            // magnet 중심쪽으로 이동
            other.gameObject.transform.position
                    = Vector3.Lerp(other.gameObject.transform.position, gameObject.transform.position, 3 * Time.deltaTime);

            // 위치가 어느정도 가까워 지면
            if(Vector3.Distance(other.gameObject.transform.position , gameObject.transform.position) <= 0.1f)
            {
                // experience Set
                PoolingManager.Instance.experiencePooling.F_SetExperience(other.gameObject);
            }
        }

    }
}
