using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropsPooling : MonoBehaviour
{
    [Header("===Pool===")]
    [SerializeField]
    private Transform _cropsPoolParent;        // Crops pool �θ� 
    [SerializeField]
    private List<GameObject> _cropsPool;   // Crops pool 
    [SerializeField]
    private List<GameObject> _cropsPrefab;       // Crops ������Ʈ 
    [SerializeField]
    private Dictionary<CropsType, Stack<GameObject>> DICT_CropsTypeToStack;

    private void Start()
    {
        // pool �ʱ�ȭ 
        F_InitCropsPool();
    }

    private void F_InitCropsPool()
    {
        DICT_CropsTypeToStack = new Dictionary<CropsType, Stack<GameObject>>();

        CropsType[] _effect = (CropsType[])System.Enum.GetValues(typeof(CropsType));

        // pool ������Ʈ ����
        for (int i = 0; i < _effect.Length; i++)
        {
            GameObject _obj = Instantiate(GameManager.Instance.emptyObject);
            _obj.transform.parent = _cropsPoolParent;
            _obj.name = _effect[i].ToString();

            _cropsPool.Add(_obj);
        }

        // bullet enum ��ŭ pool ����  
        for (int i = 0; i < _effect.Length; i++)
        {
            Stack<GameObject> _stack = new Stack<GameObject>();
            for (int j = 0; j < GameManager.Instance.POOLCOUNT; j++)
            {
                // ���ÿ� ������Ʈ �����ؼ� �ֱ� 
                _stack.Push(F_CreateCrops(_effect[i]));
            }

            DICT_CropsTypeToStack.Add(_effect[i], _stack);
        }

    }

    // effect�� �´� bullet ����  
    private GameObject F_CreateCrops(CropsType _type)
    {
        GameObject _obj = Instantiate(this._cropsPrefab[(int)_type]);
        _obj.SetActive(false);
        _obj.transform.position = Vector3.zero;
        _obj.transform.parent = _cropsPool[(int)_type].transform;

        return _obj;
    }

    // bullet Get
    public GameObject F_UnitCropsGet(CropsType _crops)
    {
        // Effect�� �ش��ϴ� ������Ʈ�� ������ 
        if (!DICT_CropsTypeToStack.ContainsKey(_crops))
        {
            Debug.LogError(this + " : Crops DICTIONARY ISNT CONTAIN KEY");
            return null;
        }

        // ������ ��������� ? 
        if (DICT_CropsTypeToStack[_crops].Count == 0)
        {
            DICT_CropsTypeToStack[_crops].Push(F_CreateCrops(_crops));
        }

        GameObject _shield = DICT_CropsTypeToStack[_crops].Pop();
        _shield.SetActive(true);

        return _shield;
    }

    // bullet Set
    public void F_UnitCropsSet(GameObject _bullet, CropsType _type)
    {
        _bullet.SetActive(false);
        _bullet.transform.localPosition = Vector3.zero;

        DICT_CropsTypeToStack[_type].Push(_bullet);

    }
}
