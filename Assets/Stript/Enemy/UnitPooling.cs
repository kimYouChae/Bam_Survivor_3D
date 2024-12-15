using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class UnitPooling : MonoBehaviour
{
    /// <summary>
    /// poolParent
    ///     �� unitPool
    ///     �� unitPool
    ///     �� unitPool
    /// </summary>


    [Header("===Pool===")]
    [SerializeField]
    private Transform _unitPoolParent;      // ���� pool �θ�
    [SerializeField]
    private List<GameObject> _unitPool;     // ���� pool
    [SerializeField]
    private GameObject[] _unitPrefabs;      // ���� ������

    [Header("===Dictionary===")]
    [SerializeField]
    private Dictionary<Unit_Animal_Type, Stack<GameObject>> DICT_AnimalTypeToStack;

    private void Start()
    {
        // �ʱ�ȭ
        DICT_AnimalTypeToStack = new Dictionary<Unit_Animal_Type, Stack<GameObject>>();

        F_InitUnitPool();
    }

    private void F_InitUnitPool()
    {
        // Resource�� �ִ� ������ �������� 
        _unitPrefabs = Resources.LoadAll<GameObject>("Unit");
        // ��ȣ����� sort
        //System.Array.Sort(_unitPrefabs,(a,b)=>a.name.CompareTo(b.name));

        Unit_Animal_Type[] _type = (Unit_Animal_Type[])System.Enum.GetValues(typeof(Unit_Animal_Type));

        // �� pool ���� 
        for (int i = 0; i < _type.Length; i++)
        {
            GameObject _pool = Instantiate(GameManager.Instance.emptyObject);
            _pool.transform.parent = _unitPoolParent;
            _pool.name = _type[i].ToString();

            _unitPool.Add(_pool);
        }

        // Unit ����
        for (int i = 0; i < _type.Length; i++) 
        {
            Stack<GameObject> _stack = new Stack<GameObject>();
            for (int j = 0; j < GameManager.Instance.UNIT_POOL_COUNT; j++) 
            {
                // ���� �� ���ÿ� �ֱ� 
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

        // type �� UnitState �������ֱ� 
        try
        {
            // ##TODO : ���⼭ �׳� state �����ϱ� �� �����ϰ��վ factory�� new �ؼ� ��ũ��Ʈ �ִ��� �ؾ��� !
            _unit.GetComponent<Unit>().unitState
                = UnitManager.Instance.UnitCsvImporter.F_AnimalTypeToState(_type); 
        }
        catch (Exception e) 
        {
            Debug.LogError(e.ToString());
        }

        return _unit;        
    }

    // Get
    public GameObject F_GetUnit(Unit_Animal_Type _type ) 
    {
        // type�� ��ųʸ��� ���� ��  
        if (!DICT_AnimalTypeToStack.ContainsKey(_type))
        {
            Debug.LogError(this + " : UNIT DICTIONARY ISNT CONTAIN KEY <<GET>> ");
            return null;
        }

        // stack�� 0�̸� 
        if (DICT_AnimalTypeToStack[_type].Count == 0) 
        {
            // �ֱ� 
            DICT_AnimalTypeToStack[_type].Push(F_CreateUnit(_type));
        }

        GameObject _unit = DICT_AnimalTypeToStack[_type].Pop();
        _unit.SetActive(true);

        return _unit;

    }

    // Set
    public void F_SetUnit(Unit _unit, Unit_Animal_Type _type) 
    {
        // type�� ��ųʸ��� ���� �� 
        if (!DICT_AnimalTypeToStack.ContainsKey(_type))
        {
            Debug.LogError(this + " : UNIT DICTIONARY ISNT CONTAIN KEY <<SET>>");
            return;
        }

        // type�� �°� push
        DICT_AnimalTypeToStack[_type].Push(_unit.gameObject);

        // ���� 
        _unit.gameObject.SetActive(false);

        _unit.gameObject.transform.localPosition = Vector3.zero;

        _unit.gameObject.transform.SetParent(_unitPool[(int)_type].transform);
    }

}
