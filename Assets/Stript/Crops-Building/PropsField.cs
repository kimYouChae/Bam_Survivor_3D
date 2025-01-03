using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PropsField : MonoBehaviour
{
    [Header("===Trs===")]
    [SerializeField]
    private Transform[] _propsPlantTrs;
    [SerializeField]
    private Transform _buildingTrs;

    [Header("BuildingData")]
    [SerializeField]
    private BuildingData<CropsType> _CropsData;
    [SerializeField]
    private BuildingData<GoodsType> _GoodsData;
    [SerializeField]
    private PropsBuilding _building;

    // ������Ƽ 
    public Transform buildingTransform => _buildingTrs;

    public void F_SetUpBuilding(PropsBuilding _building) 
    {
        // building ���� 
        this._building = _building;

        try
        {
            // field�� ���� buildingState�� Crops�ۿ� ���� 
            _CropsData = _building.CropsData;
        }
        catch (Exception e) 
        {
            Debug.LogException(e);
        }
        
    }

    public void F_PlantProps(CropsType _type) 
    {
        // TODO : type�� ���� ������Ʈ �����ͼ� �ɱ�
        for(int i = 0; i < _propsPlantTrs.Length; i++) 
        {
            // type�� �´� ������Ʈ 
            GameObject _obj = PropsBuildingManager.Instance.CropsPooling.F_UnitCropsGet(_type); 

            // ��ġ���� 
            _obj.transform.position = _propsPlantTrs[i].transform.position;

            // type �������ֱ�
            _obj.GetComponent<Props>().cropsType = _type;

            // index�������ֱ� 
            _obj.GetComponent<Props>().cropsIndex = i;
        }
    }

}
