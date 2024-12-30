using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsBuilding : MonoBehaviour
{
    [Header("===State===")]
    [SerializeField]
    private PropsBuildingCollider _buildingColliderObj;    // �浹 �����ϴ� collider

    // Building �Ҵ�
    public void F_SetBuildingState(Building _buil) 
    {
        // PropsBuilidngCollider�� �� �ֱ�
        _buildingColliderObj.F_SetBuilding(_buil);
        
    }

   
}
