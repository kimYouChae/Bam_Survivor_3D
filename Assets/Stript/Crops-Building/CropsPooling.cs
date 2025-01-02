using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropsPooling : MonoBehaviour
{
    [Header("===Pool===")]
    [SerializeField]
    private Transform _cropsPoolParent;        // Crops pool 부모 
    [SerializeField]
    private List<GameObject> _cropsPool;   // Crops pool 
    [SerializeField]
    private List<GameObject> _cropsPrefab;       // Crops 오브젝트 
    [SerializeField]
    private Dictionary<CropsType, Stack<GameObject>> DICT_CropsTypeToStack;

    private void Start()
    {
        // pool 초기화 
        F_InitCropsPool();
    }

    private void F_InitCropsPool()
    {
        DICT_CropsTypeToStack = new Dictionary<CropsType, Stack<GameObject>>();

        CropsType[] _effect = (CropsType[])System.Enum.GetValues(typeof(CropsType));

        // pool 오브젝트 생성
        for (int i = 0; i < _effect.Length; i++)
        {
            GameObject _obj = Instantiate(GameManager.Instance.emptyObject);
            _obj.transform.parent = _cropsPoolParent;
            _obj.name = _effect[i].ToString();

            _cropsPool.Add(_obj);
        }

        // bullet enum 만큼 pool 생성  
        for (int i = 0; i < _effect.Length; i++)
        {
            Stack<GameObject> _stack = new Stack<GameObject>();
            for (int j = 0; j < GameManager.Instance.POOLCOUNT; j++)
            {
                // 스택에 오브젝트 생성해서 넣기 
                _stack.Push(F_CreateCrops(_effect[i]));
            }

            DICT_CropsTypeToStack.Add(_effect[i], _stack);
        }

    }

    // effect에 맞는 bullet 생성  
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
        // Effect에 해당하는 오브젝트가 없을떄 
        if (!DICT_CropsTypeToStack.ContainsKey(_crops))
        {
            Debug.LogError(this + " : Crops DICTIONARY ISNT CONTAIN KEY");
            return null;
        }

        // 스택이 비어있으면 ? 
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
