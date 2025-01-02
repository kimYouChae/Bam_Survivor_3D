using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Props : MonoBehaviour
{
    [SerializeField]
    private CropsType _type;

    public CropsType cropsType { set { _type = value; } }
}
