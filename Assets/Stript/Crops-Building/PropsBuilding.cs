
using UnityEngine;

public class PropsBuilding : MonoBehaviour
{
    [Header("===State===")]
    [SerializeField]
    private PropsBuildingCollider _buildingColliderObj;    // 충돌 감지하는 collider 오브젝트 
    [SerializeField]
    private BuildingData<CropsType> _CropsData;
    [SerializeField]
    private BuildingData<GoodsType> _GoodsData;

    public BuildingData<CropsType> CropsData { get { return _CropsData; } }

    private void Start()
    {
        _buildingColliderObj.F_SettingBuildingData(_CropsData , _GoodsData);

    }


}
