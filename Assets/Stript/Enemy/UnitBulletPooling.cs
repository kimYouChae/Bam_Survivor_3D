using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBulletPooling : MonoBehaviour
{
    [Header("===Pool===")]
    [SerializeField]
    private Transform _bulletParnet;        // bullet pool 부모 
    [SerializeField]
    private List<GameObject> _bulletPool;   // bullet pool 
    [SerializeField]
    private List<GameObject> _bullet;       // bullet 오브젝트 
    [SerializeField]
    private Dictionary< UnitBullet, Stack<GameObject>> DICT_BulletTypeToStack;

    private void Start()
    {
        // pool 초기화 
        F_InitBulletPool();
    }

    private void F_InitBulletPool()
    {
        DICT_BulletTypeToStack = new Dictionary<UnitBullet, Stack<GameObject>>();

        UnitBullet[] _effect = (UnitBullet[])System.Enum.GetValues(typeof(UnitBullet));

        // pool 오브젝트 생성
        for (int i = 0; i < _effect.Length; i++)
        {
            GameObject _obj = Instantiate(GameManager.Instance.emptyObject);
            _obj.transform.parent = _bulletParnet;
            _obj.name = _effect[i].ToString();

            _bulletPool.Add(_obj);
        }

        // bullet enum 만큼 pool 생성  
        for (int i = 0; i < _effect.Length; i++)
        {
            Stack<GameObject> _stack = new Stack<GameObject>();
            for (int j = 0; j < GameManager.Instance.POOLCOUNT; j++)
            {
                // 스택에 오브젝트 생성해서 넣기 
                _stack.Push(F_CreateUnitBullet(_effect[i]));
            }

            DICT_BulletTypeToStack.Add(_effect[i], _stack);
        }

    }

    // effect에 맞는 bullet 생성  
    private GameObject F_CreateUnitBullet(UnitBullet _bullet)
    {
        GameObject _obj = Instantiate(this._bullet[(int)_bullet]);
        _obj.SetActive(false);
        _obj.transform.position = Vector3.zero;
        _obj.transform.parent = _bulletPool[(int)_bullet].transform;

        return _obj;
    }

    // bullet Get
    public GameObject F_UnitBulletGet(UnitBullet _bullet)
    {
        // Effect에 해당하는 오브젝트가 없을떄 
        if (!DICT_BulletTypeToStack.ContainsKey(_bullet))
        {
            Debug.LogError(this + " : SHIELD DICTIONARY ISNT CONTAIN KEY");
            return null;
        }

        // 스택이 비어있으면 ? 
        if (DICT_BulletTypeToStack[_bullet].Count == 0)
        {
            DICT_BulletTypeToStack[_bullet].Push(F_CreateUnitBullet(_bullet));
        }

        GameObject _shield = DICT_BulletTypeToStack[_bullet].Pop();
        _shield.SetActive(true);

        return _shield;
    }

    // bullet Set
    public void F_UnitBulletSet(GameObject _bullet, UnitBullet _type)
    {
        _bullet.SetActive(false);
        _bullet.transform.localPosition = Vector3.zero;

        DICT_BulletTypeToStack[_type].Push(_bullet);

    }
}
