using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Props : MonoBehaviour
{
    [SerializeField]
    private CropsType _type;
    [SerializeField]
    private int _cropsIndex;

    public CropsType cropsType { set { _type = value; } }
    public int cropsIndex { set { _cropsIndex = value; } }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 enter 했을 때 
        if (other.gameObject.layer
            == LayerManager.Instance.markerLayerNum) 
        {
            PropsBuildingManager.Instance.F_GetProps(_type);      
        }
    }
}
