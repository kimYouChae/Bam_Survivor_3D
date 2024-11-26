using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsBuilding : MonoBehaviour
{
    [Header("===State===")]
    [SerializeField]
    private Building _buildingState;

    // Building �Ҵ�
    public void F_SetBuildingState(Building _buil) 
    {
        this._buildingState = _buil;
    }

    // player�� enter ���� �� 
    private void OnCollisionEnter(Collision collision)
    {
        // marker�� enter �ϸ� 
        if (collision.gameObject.layer
            == LayerManager.Instance.markerLayer) 
        {
            // propsState �ֱ� 
            PropsBuildingManager.Instance.F_GetProps(_buildingState.PropsType);
        }
    }
}
