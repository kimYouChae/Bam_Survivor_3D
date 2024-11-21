using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitPooling : MonoBehaviour
{
    /// <summary>
    /// poolParent
    ///     ㄴ unitPool
    ///     ㄴ unitPool
    ///     ㄴ unitPool
    /// </summary>


    [Header("===Pool===")]
    [SerializeField]
    private Transform _unitPoolParent;      // 쉴드 pool 부모
    [SerializeField]
    private List<GameObject> _unitPool;     // 쉴드 pool
    [SerializeField]
    private GameObject[] _unitPrefabs;      // 유닛 프리팹
    [SerializeField]
    private Dictionary<Unit_Animal_Type, Stack<GameObject>> DICT_AnimalTypeToStack;

    private void Start()
    {
        F_InitUnitPool();
    }

    private void F_InitUnitPool()
    {
        // Resource에 있는 프리팹 가져오기 
        _unitPrefabs = Resources.LoadAll<GameObject>("Unit");
        // 번호순대로 sort
        //System.Array.Sort(_unitPrefabs,(a,b)=>a.name.CompareTo(b.name));

        // dict 초기화
        DICT_AnimalTypeToStack = new Dictionary<Unit_Animal_Type, Stack<GameObject>>();

        Unit_Animal_Type[] _type = (Unit_Animal_Type[])System.Enum.GetValues(typeof(Unit_Animal_Type));

        // 빈 pool 생성 
        for (int i = 0; i < _type.Length; i++)
        {
            GameObject _pool = Instantiate(GameManager.Instance.emptyObject);
            _pool.transform.parent = _unitPoolParent;
            _pool.name = _type[i].ToString();

            _unitPool.Add(_pool);
        }

        // Unit 생성
        for (int i = 0; i < _type.Length; i++) 
        {
            Stack<GameObject> _stack = new Stack<GameObject>();
            for (int j = 0; j < GameManager.Instance.UNIT_POOL_COUNT; j++) 
            {
                // 생성 후 스택에 넣기 
                _stack.Push(F_CreateUnit(_type[i]));
            }

            DICT_AnimalTypeToStack.Add(_type[i], _stack);
        }
    }

    private GameObject F_CreateUnit(Unit_Animal_Type _type) 
    {
        GameObject _unit = Instantiate(_unitPrefabs[(int)_type]);
        _unit.SetActive(false);
        _unit.transform.position = Vector3.zero;
        _unit.transform.parent = _unitPool[(int)_type].transform;

        // type 별 UnitState 지정해주기 
        try
        {
            _unit.GetComponent<Unit>().unitState
                = UnitManager.Instance.UnitCsvImporter.F_AnimalTypeToState(_type);
        }
        catch (Exception e) 
        {
            Debug.LogError(e.ToString());
        }

        return _unit;        
    }


}
