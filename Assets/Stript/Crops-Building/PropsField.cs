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

    // 프로퍼티 
    public Transform buildingTransform => _buildingTrs;

    public void F_SetUpBuilding(PropsBuilding _building) 
    {
        // building 세팅 
        this._building = _building;

        try
        {
            // field에 넣을 buildingState는 Crops밖에 없음 
            _CropsData = _building.CropsData;
        }
        catch (Exception e) 
        {
            Debug.LogException(e);
        }
        
    }

    public void F_PlantProps(CropsType _type) 
    {
        // TODO : type에 따른 오브젝트 가져와서 심기
        for(int i = 0; i < _propsPlantTrs.Length; i++) 
        {
            // type에 맞는 오브젝트 
            GameObject _obj = PropsBuildingManager.Instance.CropsPooling.F_UnitCropsGet(_type); 

            // 위치설정 
            _obj.transform.position = _propsPlantTrs[i].transform.position;

            // type 지정해주기
            _obj.GetComponent<Props>().cropsType = _type;

            // index지정해주기 
            _obj.GetComponent<Props>().cropsIndex = i;
        }
    }

}
