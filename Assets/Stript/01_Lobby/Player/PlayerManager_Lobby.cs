using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager_Lobby : Singleton<PlayerManager_Lobby>
{
    [Header("===Script===")]
    [SerializeField] private PlayerCsvImporter _csvImporter;

    // 프로퍼티
    public PlayerCsvImporter csvImporter => _csvImporter;

    protected override void Singleton_Awake()
    {
        
    }

    public PlayerAnimalState F_AnimaTypeToState(AnimalType _type ) 
    {
        return _csvImporter.F_AnimalTypeToState(_type);
    }
}
