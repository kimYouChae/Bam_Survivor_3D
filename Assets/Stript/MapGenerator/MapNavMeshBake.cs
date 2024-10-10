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
    private GameObject _tempObject;                 // tree에 대응하는 오브젝트 
    [SerializeField]
    private GameObject _baseField;

    [Header("===To Be Obstacle List===")]
    [SerializeField]
    private List<GameObject> _obstacleList = new List<GameObject>();

    [Header("===tree에 대응하는 오브젝트 리스트 ===")]
    private List<GameObject> _tempTreeObject = new List<GameObject>();

    [Header("===Path===")]
    private string _saveMeshpath = "Assets/Mesh/";

    private void Start()
    {
        #region tree위치에 오브젝트 생성 , combine 후 Asset폴더 하위에 mesh 저장 ! 
        /*
        F_InstanceTreeObjec();

        // tree에 대응하는 오브젝트 combine
        MapManager.instance.mapCombiner.F_CombineTreeMesh(_tempTreeObject, CombineType.Tree);

        // obstacle 리스트에 들어온 오브젝를 저장하기
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
        // mapGrid의 true (장애물)인 부분에 오브젝트 생성

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
