using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsOnOff : MonoBehaviour
{
    [Header("===Animal Gameobject===")]
    [SerializeField]
    private GameObject[] _animals;

    [SerializeField]
    private int _preIdex = -1;

    public void F_OnOFfAnimalsByIndex(int _currIdx) 
    {
        if(_preIdex != -1)
            _animals[_preIdex].SetActive(false);

        _animals[_currIdx].SetActive(true);

        _preIdex = _currIdx;
    }


}
