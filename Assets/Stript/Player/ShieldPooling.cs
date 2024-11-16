using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class ShieldPooling : MonoBehaviour
{
    public static ShieldPooling instance;

    [Header("===Pool===")]
    [SerializeField]
    private Transform _shieldPoolParent;    // 쉴드 pool 부모 
    [SerializeField]
    private List<GameObject> _shieldPool;   // 쉴드 pool 
    [SerializeField]
    private List<GameObject> _shield;       // 쉴드 오브젝트 
    [SerializeField]
    private Dictionary<Shield_Effect, Stack<GameObject>> DICT_shieldEffectToObject;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // pool 초기화 
        F_InitShieldPool();
    }

    private void F_InitShieldPool() 
    {
        DICT_shieldEffectToObject = new Dictionary<Shield_Effect, Stack<GameObject>>();

        Shield_Effect[] _effect = (Shield_Effect[])System.Enum.GetValues(typeof(Shield_Effect));

        // pool 오브젝트 생성
        for(int i = 0; i < _effect.Length; i++) 
        {
            GameObject _obj = Instantiate(GameManager.instance.emptyObject);
            _obj.transform.parent = _shieldPoolParent;
            _obj.name = _effect[i].ToString();

            _shieldPool.Add( _obj );
        }

        // shield effect enum 만큼 pool 생성  
        for (int i = 0; i < _effect.Length; i++) 
        {
            Stack<GameObject> _stack = new Stack<GameObject>(); 
            for(int j = 0; j < GameManager.instance.POOLCOUNT; j++) 
            {
                // 스택에 오브젝트 생성해서 넣기 
                _stack.Push(F_CreateShield(_effect[i]));    
            }

            DICT_shieldEffectToObject.Add(_effect[i] , _stack );
        }
        
    }

    // effect에 맞는 쉴드 생성  
    private GameObject F_CreateShield(Shield_Effect _effect) 
    {
        GameObject _obj = Instantiate(_shield[(int)_effect]);
        _obj.SetActive(false);
        _obj.transform.position = Vector3.zero; 
        _obj.transform.parent = _shieldPool[(int)_effect].transform;

        // effect별 min,max Size 넣기
        try
        {
            _obj.GetComponent<ShieldObject>().F_SettingShiledObject
                (
                    _effect,
                    ShieldCSVImporter.instance.ShieldMin(_effect),
                    ShieldCSVImporter.instance.ShieldMax(_effect)
                ); 
        }
        catch (Exception e) 
        {
            Debug.LogError(e.ToString());
        }
            
        

        return _obj;
    }

    // shield Get
    public GameObject F_ShieldGet(Shield_Effect _effect)
    {
        // Effect에 해당하는 오브젝트가 없을떄 
        if (!DICT_shieldEffectToObject.ContainsKey(_effect))
        {
            Debug.LogError(this + " : SHIELD DICTIONARY ISNT CONTAIN KEY");
            return null;
        }

        // 스택이 비어있으면 ? 
        if (DICT_shieldEffectToObject[_effect].Count == 0 )
        {
            DICT_shieldEffectToObject[_effect].Push(F_CreateShield(_effect)); 
        }

        GameObject _shield = DICT_shieldEffectToObject[_effect].Pop();
        _shield.SetActive(true);

        return _shield;
    }

    // shiled Set
    public void F_ShieldSet(GameObject _shield, Shield_Effect _effct ) 
    {
        _shield.SetActive(false);
        _shield.transform.localPosition = Vector3.zero;

        DICT_shieldEffectToObject[_effct].Push(_shield);

    }

}
