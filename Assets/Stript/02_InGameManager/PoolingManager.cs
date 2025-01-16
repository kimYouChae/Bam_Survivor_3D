using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : Singleton<PoolingManager>
{
    [Header("===Pooling Script===")]
    [SerializeField] private ExperiencePooling _experiencePooling;

    public ExperiencePooling experiencePooling => _experiencePooling;

    protected override void Singleton_Awake()
    {
        
    }
}
