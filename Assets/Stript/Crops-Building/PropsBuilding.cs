
using UnityEngine;

public class PropsBuilding : MonoBehaviour
{
    [Header("===State===")]
    [SerializeField]
    private PropsBuildingCollider _buildingColliderObj;    // �浹 �����ϴ� collider ������Ʈ 
    [SerializeField]
    private BuildingData<CropsType> _CropsData;
    [SerializeField]
    private BuildingData<GoodsType> _GoodsData;

    private void Start()
    {
        _buildingColliderObj.F_SettingBuildingData(_CropsData , _GoodsData);

    }


}
