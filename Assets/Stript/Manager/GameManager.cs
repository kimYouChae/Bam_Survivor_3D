using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{

    [Header("===Ratio / 0~1사이 ===")]
    [SerializeField] private float _basicRatio;
    [SerializeField] private float _commonRatio;
    [SerializeField] private float _rareRatio;
    [SerializeField] private float _epicRatio;
    [SerializeField] private float _legendaryRatio;

    [Header("===Eenemy Generator Time===")]
    [SerializeField] private float _unitGenerateTime;

    [Header("===Pooling===")]
    [SerializeField] private GameObject _emptyObject;           // 빈 오브젝트  
    [SerializeField] private const int defualt_POOLCOUNT = 10;    // pool에 초기 생성할 count 
    [SerializeField] private const int unit_POOLCOUNT = 2;    // (Unit) pool에 초기 생성할 count

    [Header("===Map Size===")]
    [SerializeField] private const int map_size = 70;

    [Header("===Delete Object ===")]
    // 테스트오브젝트 ##TODO 나중에 삭제
    public GameObject _testObject;
    public GameObject _testUnitPig;
    public Transform _unitTestGeneration;

    //프로퍼티
    public float BasicRatio => _basicRatio;
    public float CommonRatio => _commonRatio;
    public float RareRatio => _rareRatio;
    public float EpicRatio => _epicRatio;
    public float LegaryRatio => _legendaryRatio;
    public float unitGenerateTime => _unitGenerateTime;
    public GameObject emptyObject => _emptyObject;
    public int POOLCOUNT => defualt_POOLCOUNT;
    public int UNIT_POOL_COUNT => unit_POOLCOUNT;
    public int MAP_SIZE => map_size;

    protected override void Singleton_Awake()
    {

    }

    private void Start()
    {
        // unit 네브매쉬 테스트
        /*
        GameObject _temp = Instantiate(_testUnitPig , new Vector3(12f,0,99f), Quaternion.identity);
        _temp.gameObject.name = "?!!!!!!!!!!!!!!!!!!";
        _temp.GetComponent<NavMeshAgent>().SetDestination(PlayerManager.Instance.markerHeadTrasform.position);
        */

    }


    #region GridMap 생성 (X)
    /*
    public void F_SetGridMap(int v_x , int v_z , bool v_flag) 
    {
        _mapGrid[v_x , v_z] = v_flag;
    }
    */
    #endregion

}
