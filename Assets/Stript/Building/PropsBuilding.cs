using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsBuilding : MonoBehaviour
{
    [Header("===State===")]
    [SerializeField]
    private PropsBuildingCollider _buildingColliderObj;    // 충돌 감지하는 collider

    // Building 할당
    public void F_SetBuildingState(Building _buil) 
    {
        // PropsBuilidngCollider에 값 넣기
        _buildingColliderObj.F_SetBuilding(_buil);
        
    }

   
}
