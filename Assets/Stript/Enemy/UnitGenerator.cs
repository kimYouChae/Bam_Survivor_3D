using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    [Header("===Spawn Point===")]
    [SerializeField]
    private Transform[] _spawnPointTrs;         // ���� ��ġ ����Ʈ 
    [SerializeField]
    private List<Tuple<int, float>> _closetSpawnerSort;    // �÷��̾�� ����� ������
    [SerializeField]
    private List<Transform> _spawner;

    void Start()
    {
        //StartCoroutine(IE_GenerateEnemy());

        // �ʱ�ȭ
        _closetSpawnerSort = new List<Tuple<int, float>>();

    }

    public List<Transform> F_GetClosetSpawner(int _n) 
    {
        _closetSpawnerSort.Clear();

        for (int i = 0; i < _spawnPointTrs.Length; i++) 
        {
            // index, marker�� ������ �Ÿ� distace 
            _closetSpawnerSort.Add(new Tuple<int, float>
                (i, Vector3.Distance(PlayerManager.Instance.markerHeadTrasform.position, _spawnPointTrs[i].position)));
        }

        // distance�� ���� sort
        _closetSpawnerSort.Sort((a, b) => a.Item2.CompareTo(b.Item2));

        for (int i = 0; i < _n; i++) 
        {
            _spawner.Add( _spawnPointTrs[_closetSpawnerSort[i].Item1] );
        }

        return _spawner;
    }


    /*
    IEnumerator IE_GenerateEnemy() 
    { 
        while (true) 
        {
            // unity ���� 
            GameObject _instance = Instantiate(_tempUnit, _unitSpawn.position , Quaternion.identity );
            yield return new WaitForSeconds(GameManager.Instance.unitGenerateTime);
        }
    }
    */
}
