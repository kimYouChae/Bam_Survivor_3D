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
