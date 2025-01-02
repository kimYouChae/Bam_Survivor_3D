using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingData : ScriptableObject
{
    [SerializeField] private CropsType _propsType;
    [SerializeField] private string _buildingName;
    [SerializeField] private float _generateSecond;
    [SerializeField] private Sprite _propsSprite;

    public CropsType PropsType { get => _propsType; set => _propsType = value; }
    public string BuildingName { get => _buildingName; set => _buildingName = value; }
    public float GenerateSecond { get => _generateSecond; set => _generateSecond = value; }
    public Sprite PropsSprite { get => _propsSprite; set => _propsSprite = value; }
}
