using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsBuildingCollider : MonoBehaviour
{
    [SerializeField]
    private Building _building;
    [SerializeField]
    private bool _readyToHarvest;       // 수확이 되는지

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
            // 수확이 false가 될때까지 대기 
            yield return new WaitUntil(() => _readyToHarvest == false);

            // 수확할 시간이 됐다면
            if(_currTime >= _building.GenerateSecond) 
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
                PropsBuildingManager.Instance.F_GetProps(_building.PropsType);

                _readyToHarvest = false;
            }
            else
            {
                Debug.Log("아직 성장이 덜 되었습니다");
            }

        }
    }
}
