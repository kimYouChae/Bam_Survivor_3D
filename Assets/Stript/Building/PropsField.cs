using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsField : MonoBehaviour
{
    [SerializeField]
    private Transform[] _propsPlantTrs;

    [SerializeField]
    private Transform _buildingTrs;

    // 프로퍼티 
    public Transform buildingTransform => _buildingTrs;

    public void F_PlantProps(InGamePropState _type) 
    {
        // TODO : type에 따른 오브젝트 가져와서 심기
        for(int i = 0; i < _propsPlantTrs.Length; i++) 
        { 
        
        }
    }

}
