using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingData<T> : ScriptableObject where T : Enum
{
    [SerializeField] private T _propsType;
    [SerializeField] private string _buildingName;
    [SerializeField] private float _generateSecond;
    [SerializeField] private Sprite _propsSprite;
    [SerializeField] private int _gainAmount;

    public T PropsType { get => _propsType; set => _propsType = value; }
    public string BuildingName { get => _buildingName; set => _buildingName = value; }
    public float GenerateSecond { get => _generateSecond; set => _generateSecond = value; }
    public Sprite PropsSprite { get => _propsSprite; set => _propsSprite = value; }
    public int GainAmount { get => _gainAmount;}
}

[CreateAssetMenu(fileName = "Building Data", menuName = "Buildings/Crops Building", order = 1)]
public class CropsBuildingData : BuildingData<CropsType> { }


[CreateAssetMenu(fileName = "Building Data", menuName = "Buildings/Goods Building", order = 1)]
public class GoodsBuilidngData : BuildingData<GoodsType> { }

