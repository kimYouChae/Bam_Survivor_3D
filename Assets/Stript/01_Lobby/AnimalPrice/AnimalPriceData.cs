using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animal Price Data", menuName = "AnimalPrice")]
public class AnimalPriceData : ScriptableObject
{
    [SerializeField]
    private AnimalType _type;
    [SerializeField]
    private int _price;

    public AnimalType animalType => _type;
    public int AnimalPrice => _price;
}
