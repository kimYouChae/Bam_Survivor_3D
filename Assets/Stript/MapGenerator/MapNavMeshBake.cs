using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class MapNavMeshBake : MonoBehaviour
{
    [Header("===Object===")]
    [SerializeField]
    private GameObject _tempObject;                 // tree�� �����ϴ� ������Ʈ 
    [SerializeField]
    private GameObject _baseField;

    [Header("===To Be Obstacle List===")]
    [SerializeField]
    private List<GameObject> _obstacleList = new List<GameObject>();

    [Header("===tree�� �����ϴ� ������Ʈ ����Ʈ ===")]
    private List<GameObject> _tempTreeObject = new List<GameObject>();

    [Header("===Path===")]
    private string _saveMeshpath = "Assets/Mesh/";

    private void Start()
    {
        #region tree��ġ�� ������Ʈ ���� , combine �� Asset���� ������ mesh ���� ! 
        /*
        F_InstanceTreeObjec();

        // tree�� �����ϴ� ������Ʈ combine
        MapManager.instance.mapCombiner.F_CombineTreeMesh(_tempTreeObject, CombineType.Tree);

        // obstacle ����Ʈ�� ���� �������� �����ϱ�
        for (int i = 0; i < _obstacleList.Count; i++) 
        {
            string _fullpath = _saveMeshpath + i.ToString() + ".asset";

            AssetDatabase.CreateAsset(_obstacleList[i].GetComponent<MeshFilter>().mesh, _fullpath);
            AssetDatabase.SaveAssets();

            _obstacleList[i].SetActive(false);
        }
        */
        #endregion

        
    }

    private void F_InstanceTreeObjec() 
    {
        // mapGrid�� true (��ֹ�)�� �κп� ������Ʈ ����

        for(int y = 8; y < MapManager.instance.mapSize - 8; y++) 
        {
            for (int x = 8; x < MapManager.instance.mapSize - 8; x++)
            {
                if (MapManager.instance.mapGrid[y, x] == true)
                { 
                    GameObject _temp = Instantiate(_tempObject , new Vector3( x, 0 , y) , Quaternion.identity);
                    _tempTreeObject.Add(_temp); 

                }
                
            }
        }

    }

    public void F_AddObstacle(GameObject v_obj) 
    {
        _obstacleList.Add(v_obj);
    }
}
