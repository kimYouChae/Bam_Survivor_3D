using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    /// <summary>
    /// 임시 :  일정시간마다 enemy 생성 
    /// 
    /// </summary>

    [SerializeField] Transform _unitSpawn;      // spawn 지점
    [SerializeField] GameObject _tempUnit;      // 적 프리팹 

    void Start()
    {
        StartCoroutine(IE_GenerateEnemy());
    }

    IEnumerator IE_GenerateEnemy() 
    { 
        while (true) 
        {
            // unity 생성 
            GameObject _instance = Instantiate(_tempUnit, _unitSpawn.position , Quaternion.identity );
            yield return new WaitForSeconds(GameManager.instance.unitGenerateTime);
        }
    }
}
