using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Props : MonoBehaviour
{
    [Header("===Crops===")]
    [SerializeField]
    private CropsType _type;
    [SerializeField]
    private int _cropsIndex;

    [Header("===State===")]
    [SerializeField]
    private bool _readyToHarvest;       // 수확이 되는지
    [SerializeField]
    private float _currTime = 0;
    [SerializeField]
    private float _buildingGenerateTime;
    [SerializeField]
    private Sprite _buildingSprite;

    [Header("===Component===")]
    private MeshRenderer _buildingRenderer;

    public CropsType cropsType { set { _type = value; } }
    public int cropsIndex { set { _cropsIndex = value; } }

    public void F_CropsStartToGrowth(CropsType cropsType , int index , float generateTime , Sprite sprite) 
    {
        //
        this._type = cropsType;
        this._cropsIndex = index;
        this._buildingGenerateTime = generateTime;
        this._buildingSprite = sprite;

        // 초기화
        _readyToHarvest = false;

        _buildingRenderer = GetComponent<MeshRenderer>();

        // 코루틴 시작 
        StartCoroutine(IE_PropsGrowth());
    }

    public IEnumerator IE_PropsGrowth()
    {
        // 맨 처음에는 꺼놓기
        F_MeshOnOff(false);

        while (true)
        {
            // 수확이 false가 될때까지 대기 
            yield return new WaitUntil(() => _readyToHarvest == false);

            // 수확할 시간이 됐다면
            if (_currTime >= _buildingGenerateTime)
            {
                _readyToHarvest = true;
                _currTime = 0;

                // mesh on
                F_MeshOnOff(true);
            }

            // 타이머
            _currTime += Time.deltaTime;

            // 한프레임 대기
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 enter 했을 때 
        if (other.gameObject.layer
            == LayerManager.Instance.markerLayerNum) 
        {
            if (_readyToHarvest)
            {
                // crops 획득 
                PropsBuildingManager.Instance.F_GetProps(_type);

                // 안보이게
                F_MeshOnOff(false);

                _readyToHarvest = false;
            }
            else 
            {
                Debug.Log("아직 " + _type + "덜 성장 ");
            }
        }
    }

    private void F_MeshOnOff(bool _flag) 
    {
        _buildingRenderer.enabled = _flag;
    }
}
