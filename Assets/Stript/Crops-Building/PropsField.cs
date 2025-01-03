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
