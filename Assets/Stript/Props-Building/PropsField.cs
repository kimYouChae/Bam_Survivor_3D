using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsField : MonoBehaviour
{
    [SerializeField]
    private Transform[] _propsPlantTrs;

    [SerializeField]
    private Transform _buildingTrs;

    // ������Ƽ 
    public Transform buildingTransform => _buildingTrs;

    public void F_PlantProps(InGamePropState _type) 
    {
        // TODO : type�� ���� ������Ʈ �����ͼ� �ɱ�
        for(int i = 0; i < _propsPlantTrs.Length; i++) 
        { 
        
        }
    }

}
