using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsBuilding : MonoBehaviour
{
    [Header("===State===")]
    [SerializeField]
    private Building _buildingState;

    // Building 할당
    public void F_SetBuildingState(Building _buil) 
    {
        this._buildingState = _buil;
    }

    // player가 enter 했을 때 
    private void OnCollisionEnter(Collision collision)
    {
        // marker가 enter 하면 
        if (collision.gameObject.layer
            == LayerManager.Instance.markerLayer) 
        {
            // propsState 넣기 
            PropsBuildingManager.Instance.F_GetProps(_buildingState.PropsType);
        }
    }
}
