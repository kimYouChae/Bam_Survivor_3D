using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _gridObject;
    [SerializeField]
    private GameObject _gridParent;

    public void F_GenerateGrid3D()
    {
        Debug.Log("¸Ê »ý¼º");

        int _size = 140;

        for(int i = 0; i< _size; i++) 
        {
            for (int j = 0; j < _size; j++)
            {
                Instantiate(_gridObject , new Vector3(i , -0.5f , j) , Quaternion.identity , _gridParent.transform);

            }
        }

    }
}
