using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsBuildingCollider : MonoBehaviour
{
    [Header("State")]
    [SerializeField]
    private bool _readyToHarvest;       // 수확이 되는지
    [SerializeField]
    private float _currTime = 0;

    [Header("BuildingData")]
    [SerializeField]
    private BuildingData<CropsType> _CropsData;
    [SerializeField]
    private BuildingData<GoodsType> _GoodsData;

    [SerializeField]
    private float _buildingGenerateTime;
    [SerializeField]
    private Sprite _buildingSprite;

    private void Start()
    { 

        _readyToHarvest = false;

        StartCoroutine(IE_PropsGrowth());

    }

    public void F_SettingBuildingData(BuildingData<CropsType> _crops, BuildingData<GoodsType> _goods) 
    {
        this._CropsData = _crops;
        this._GoodsData = _goods;

        if (_CropsData != null && _GoodsData == null)
        {
            _buildingGenerateTime = _CropsData.GenerateSecond;
            _buildingSprite = _CropsData.PropsSprite;
        }
        // goods일때
        if (_CropsData == null && _GoodsData != null)
        {
            _buildingGenerateTime = _GoodsData.GenerateSecond;
            _buildingSprite = _GoodsData.PropsSprite;
        }

    }

    IEnumerator IE_PropsGrowth() 
    {

        while (true) 
        {
            // 수확이 false가 될때까지 대기 
            yield return new WaitUntil(() => _readyToHarvest == false);

            // 수확할 시간이 됐다면
            if(_currTime >= _buildingGenerateTime) 
            { 
                _readyToHarvest = true;
                _currTime = 0;
            }

            // 타이머
            _currTime += Time.deltaTime; 
        
            // 한프레임 대기
             yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // marker가 enter 하면 
        if (other.gameObject.layer
            == LayerManager.Instance.markerLayerNum)
        {
            // 수확할 준비가 되었으면
            if (_readyToHarvest)
            {
                // propsState 넣기 
                // crops일떄
                if (_CropsData != null && _GoodsData == null)
                {
                    F_CropsAddCount();
                }
                // goods일때
                if (_CropsData == null && _GoodsData == null)
                {
                    F_GoodsAddCount();
                }

                _readyToHarvest = false;
            }
            else
            {
                Debug.Log("아직 성장이 덜 되었습니다");
            }

        }
    }


    public void F_CropsAddCount()
    {
        // PropsBuilidngManager의 함수 실행 
        PropsBuildingManager.Instance.F_GetProps(_CropsData.PropsType);
    }

    public void F_GoodsAddCount()
    {
        // GoodManager의 함수 실행
        GoodsManager.Instance.F_GetGoods(_GoodsData.PropsType);
    }


}
