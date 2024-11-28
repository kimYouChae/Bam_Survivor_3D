using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePooling : MonoBehaviour
{
    [Header("===Experience")]
    [SerializeField]
    private GameObject _ExpPrefab;
    [SerializeField]
    private Transform _expPoolParent;
    [SerializeField]
    private const int InitCount = 50;
    [SerializeField]
    private Stack<GameObject> _experienceStack;

    private void Start()
    {
        _experienceStack = new Stack<GameObject>();

        F_InitPooling();
    }

    // pool √ ±‚»≠ 
    private void F_InitPooling() 
    {
        for (int i = 0; i < InitCount; i++)
        {
            _experienceStack.Push(F_InitEXP());
        }
    }

    // craete EXP
    private GameObject F_InitEXP() 
    {
        GameObject _obj = Instantiate(_ExpPrefab, _expPoolParent);
        _obj.SetActive(false);
        _obj.transform.position = Vector3.zero;

        return _obj;
    }

    // exp Get
    public GameObject F_GetExperience() 
    {
        if (_experienceStack.Count <= 0)
        {
            _experienceStack.Push(F_InitEXP());
        }

        GameObject _obj = _experienceStack.Pop();
        _obj.SetActive(true);
        return _obj;
    }

    // exp Set
    public void F_SetExperience(GameObject _obj) 
    {
        _obj.SetActive(false);
        _obj.transform.position = Vector3.zero;

        _experienceStack.Push(_obj);
    }
}
