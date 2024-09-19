using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    private int _size;
    [SerializeField]
    private GameObject _gridObject;
    [SerializeField]
    private GameObject _gridParent;

    public void F_GenerateGrid3D()
    {
        Debug.Log("¸Ê »ý¼º");

        for(int i = _size * - 1; i< _size; i++) 
        {
            for (int j = _size * -1; j < _size; j++)
            {
                Instantiate(_gridObject , new Vector3(i , -0.5f , j) , Quaternion.identity , _gridParent.transform);

            }
        }

    }
}
