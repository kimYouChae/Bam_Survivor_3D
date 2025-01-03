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
    [SerializeField]
    private Transform _cropsParnet;

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

    public void F_PlantProps(CropsType _type , GameObject _crops) 
    {
        // TODO : type에 따른 오브젝트 가져와서 심기
        for(int i = 0; i < _propsPlantTrs.Length; i++) 
        {
            // type에 맞는 오브젝트 
            GameObject _obj = Instantiate(_crops , _propsPlantTrs[i].position , Quaternion.identity);
            _obj.transform.parent = _cropsParnet;
            
            _obj.GetComponent<Props>().F_CropsStartToGrowth
                (cropsType : _type, index : i , generateTime : _CropsData.GenerateSecond , sprite:_CropsData.PropsSprite);
        }
    }

}
