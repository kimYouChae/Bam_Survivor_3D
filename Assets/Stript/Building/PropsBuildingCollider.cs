using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsBuildingCollider : MonoBehaviour
{
    [SerializeField]
    private Building _building;
    [SerializeField]
    private bool _readyToHarvest;       // ��Ȯ�� �Ǵ���

    [SerializeField]
    private float _currTime = 0;

    public void F_SetBuilding(Building _build) 
    {
        this._building = _build;      

        _readyToHarvest = false;

        StartCoroutine(IE_PropsGrowth());
    }

    IEnumerator IE_PropsGrowth() 
    {

        while (true) 
        {
            // ��Ȯ�� false�� �ɶ����� ��� 
            yield return new WaitUntil(() => _readyToHarvest == false);

            // ��Ȯ�� �ð��� �ƴٸ�
            if(_currTime >= _building.GenerateSecond) 
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
                PropsBuildingManager.Instance.F_GetProps(_building.PropsType);

                _readyToHarvest = false;
            }
            else
            {
                Debug.Log("���� ������ �� �Ǿ����ϴ�");
            }

        }
    }
}
