
using System.Collections;
using UnityEngine;

public class PropsBuilding : MonoBehaviour
{
    [Header("===Building data===")]
    [SerializeField]
    private BuildingData<CropsType> _CropsData;
    [SerializeField]
    private BuildingData<GoodsType> _GoodsData;

    [Header("===State===")]
    [SerializeField]
    private bool _readyToHarvest;       // ��Ȯ�� �Ǵ���
    [SerializeField]
    private float _currTime = 0;
    [SerializeField]
    private float _buildingGenerateTime;
    [SerializeField]
    private Sprite _buildingSprite;

    public BuildingData<CropsType> CropsData { get { return _CropsData; } }

    private void Start()
    {
        _readyToHarvest = false;

        if (_CropsData != null && _GoodsData == null)
        {
            _buildingGenerateTime = _CropsData.GenerateSecond;
            _buildingSprite = _CropsData.PropsSprite;
        }
        // goods???
        if (_CropsData == null && _GoodsData != null)
        {
            _buildingGenerateTime = _GoodsData.GenerateSecond;
            _buildingSprite = _GoodsData.PropsSprite;
        }

        StartCoroutine(IE_PropsGrowth());

    }


    public IEnumerator IE_PropsGrowth()
    {

        while (true)
        {
            // ��Ȯ�� false�� �ɶ����� ��� 
            yield return new WaitUntil(() => _readyToHarvest == false);

            // ��Ȯ�� �ð��� �ƴٸ�
            if (_currTime >= _buildingGenerateTime)
            {
                _readyToHarvest = true;
                _currTime = 0;
            }

            // Ÿ�̸�
            _currTime += Time.deltaTime;

            // �������� ���
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // marker�� enter �ϸ� 
        if (other.gameObject.layer
            == LayerManager.Instance.markerLayerNum)
        {
            // ��Ȯ�� �غ� �Ǿ�����
            if (_readyToHarvest)
            {
                // propsState �ֱ� 
                // crops�ϋ�
                if (_CropsData != null && _GoodsData == null)
                {
                    F_CropsAddCount();
                }
                // goods�϶�
                if (_CropsData == null && _GoodsData == null)
                {
                    F_GoodsAddCount();
                }

                _readyToHarvest = false;
            }
            else
            {
                Debug.Log("���� ������ �� �Ǿ����ϴ�");
            }

        }
    }

    public void F_CropsAddCount()
    {
        // PropsBuilidngManager�� �Լ� ���� 
        PropsBuildingManager.Instance.F_GetProps(_CropsData.PropsType);
    }

    public void F_GoodsAddCount()
    {
        // GoodManager�� �Լ� ����
        GoodsManager.Instance.F_GetGoods(_GoodsData.PropsType);
    }


}
