using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    /// <summary>
    /// �ӽ� :  �����ð����� enemy ���� 
    /// 
    /// </summary>

    [SerializeField] Transform _unitSpawn;      // spawn ����
    [SerializeField] GameObject _tempUnit;      // �� ������ 

    void Start()
    {
        StartCoroutine(IE_GenerateEnemy());
    }

    IEnumerator IE_GenerateEnemy() 
    { 
        while (true) 
        {
            // unity ���� 
            GameObject _instance = Instantiate(_tempUnit, _unitSpawn.position , Quaternion.identity );
            yield return new WaitForSeconds(GameManager.instance.unitGenerateTime);
        }
    }
}
