using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        // exp �� �� 
        if (other.gameObject.layer == LayerManager.Instance.expLayerNum) 
        {
            // magnet �߽������� �̵�
            other.gameObject.transform.position
                    = Vector3.Lerp(other.gameObject.transform.position, gameObject.transform.position, 3 * Time.deltaTime);

            // ��ġ�� ������� ����� ����
            if(Vector3.Distance(other.gameObject.transform.position , gameObject.transform.position) <= 0.1f)
            {
                // experience Set
                PoolingManager.Instance.experiencePooling.F_SetExperience(other.gameObject);
            }
        }

    }
}
