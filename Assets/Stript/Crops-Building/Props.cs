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
    private bool _readyToHarvest;       // ��Ȯ�� �Ǵ���
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

        // �ʱ�ȭ
        _readyToHarvest = false;

        _buildingRenderer = GetComponent<MeshRenderer>();

        // �ڷ�ƾ ���� 
        StartCoroutine(IE_PropsGrowth());
    }

    public IEnumerator IE_PropsGrowth()
    {
        // �� ó������ ������
        F_MeshOnOff(false);

        while (true)
        {
            // ��Ȯ�� false�� �ɶ����� ��� 
            yield return new WaitUntil(() => _readyToHarvest == false);

            // ��Ȯ�� �ð��� �ƴٸ�
            if (_currTime >= _buildingGenerateTime)
            {
                _readyToHarvest = true;
                _currTime = 0;

                // mesh on
                F_MeshOnOff(true);
            }

            // Ÿ�̸�
            _currTime += Time.deltaTime;

            // �������� ���
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ enter ���� �� 
        if (other.gameObject.layer
            == LayerManager.Instance.markerLayerNum) 
        {
            if (_readyToHarvest)
            {
                // crops ȹ�� 
                PropsBuildingManager.Instance.F_GetProps(_type);

                // �Ⱥ��̰�
                F_MeshOnOff(false);

                _readyToHarvest = false;
            }
            else 
            {
                Debug.Log("���� " + _type + "�� ���� ");
            }
        }
    }

    private void F_MeshOnOff(bool _flag) 
    {
        _buildingRenderer.enabled = _flag;
    }
}
