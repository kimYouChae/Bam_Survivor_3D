using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public enum CombineType 
{
    Water,
    Soil,
    Tree
}

public class MapMeshCombiner : MonoBehaviour
{
    /// <summary>
    /// upTree
    ///     ㄴ tree1
    ///     ㄴ tree2
    /// downTree
    ///     ㄴ tree1
    ///     ㄴ tree2
    /// : 이런구조여서 부모를 담고, 그 하위에 자식을 돌면서 combine 
    /// 
    /// </summary>

    [Header("=== TODO Combine ===")]
    [SerializeField]
    private List<GameObject> _waterParent;
    [SerializeField]
    private List<GameObject> _soilParent;

    [Header("===Finish Conbine Object===")]
    [SerializeField]
    private List<GameObject> _combineFinishObjectList;

    [Header("===Container===")]
    private List<CombineInstance> _combineList;

    private int _overMaxCountMesh;
    private int _meshCount;

    [Header("===Tree Object===")]
    [SerializeField]
    private GameObject _treeMeshParentPrefab;

    [Header("===Path===")]
    private string _saveMeshpath = "Assets/Mesh/";

    // 프로퍼티 
    public List<GameObject> combineFinishObject => _combineFinishObjectList;

    private void Start()
    {
        _meshCount = 0;
        _overMaxCountMesh = 0;
        _combineList = new List<CombineInstance>();

        // 1.Conbine 하기 
        F_CombineMesh(_waterParent, CombineType.Water);
        F_CombineMesh(_soilParent , CombineType.Soil);

        for (int i = 0; i < 2; i++)
        {
            string _fullpath = _saveMeshpath + "Map Object " + i +".asset";

            AssetDatabase.CreateAsset(_combineFinishObjectList[i].GetComponent<MeshFilter>().mesh, _fullpath);
            AssetDatabase.SaveAssets();

            _combineFinishObjectList[i].SetActive(false);

        }

    }


    #region NavMesh하기위한 사전작업 (tree에 해당하는 오브젝트 생성 후 mesh 합치기)
    /*
    public void F_CombineTreeMesh( List<GameObject> v_list , CombineType v_type) 
    {
        _combineList.Clear();
        Material _material = null;

        for (int i = 0; i < v_list.Count; i++) 
        {
            Transform _child = v_list[i].transform;

            // mesh 최대 vertex 수 넘으면 안됨 
            if (_meshCount > 65500) 
            {
                // 1. 부모 하나 더 만들기 
                GameObject _temp        = Instantiate(_treeMeshParentPrefab);
                _temp.transform.parent  = _combineObject[0].transform.parent.transform;
                _combineObject.Add( _temp );

                // 2. navMesh Obstacle 리스트에 넣기
                MapManager.instance.mapNavMeshBake.F_AddObstacle(_temp);

                // 3. mesh 합치기
                F_CombineMesh(v_type , _material , _overMaxCountMesh);
                _overMaxCountMesh ++ ;

                // 4. 초기화
                _combineList.Clear();
                _meshCount = 0;
            }

            _meshCount += 24;
            F_CreateComIns(_child);

            // ##TODO
            // 임시 ) destory
            Destroy(_child.gameObject);

            if (_material != null)
                continue;

            _material = _child.GetComponent<MeshRenderer>().material;
        }

        // 마지막에 담긴 box들 
        // 1 .부모 만들기
        GameObject _temp2 = Instantiate(_treeMeshParentPrefab);
        _temp2.transform.parent = _combineObject[0].transform.parent.transform;
        _combineObject.Add(_temp2);

        // 2. obstacle 리스트에 넣기
        MapManager.instance.mapNavMeshBake.F_AddObstacle(_temp2);

        // 3. Mesh 함치기 
        F_CombineMesh(v_type, _material, _overMaxCountMesh);
    }
    */
    #endregion

    public void F_CombineMesh( List<GameObject> v_list , CombineType v_type) 
    {
        _combineList.Clear();
        Material _material = null;

        for (int i = 0; i < v_list.Count; i++) 
        {
            for (int j = 0; j < v_list[i].gameObject.transform.childCount; j++) 
            {

                Transform _child = v_list[i].transform.GetChild(j);

                F_CreateComIns(_child);

                if (_material != null)
                    continue;

                _material = _child.GetComponent<MeshRenderer>().material;
            }

            // Combine 한 오브젝트는 다 끄기 (tree 제외 )
            v_list[i].gameObject.SetActive(false);
        }


        F_CombineMesh(v_type, _material, 0);

    }

    private void F_CombineMesh(CombineType v_type , Material v_mat, int v_blank) 
    {
        // mesh 합치기 
        Mesh _combineMesh = new Mesh();
        _combineMesh.CombineMeshes(_combineList.ToArray(), true);

        Debug.Log(v_type + " : " + _combineList.Count);

        // 시각적으로 보기 
        _combineFinishObjectList[(int)v_type + v_blank].GetComponent<MeshFilter>().sharedMesh = _combineMesh;
        _combineFinishObjectList[(int)v_type + v_blank].GetComponent<MeshRenderer>().sharedMaterial = v_mat;

        // 오브젝트에 콜라이더 추가
        if (_combineFinishObjectList[(int)v_type + v_blank].GetComponent<MeshCollider>() == null)
            _combineFinishObjectList[(int)v_type + v_blank].AddComponent<MeshCollider>();
    }

    private void F_CreateComIns(Transform v_trs) 
    {
        CombineInstance cbi = new CombineInstance();

        cbi.mesh = v_trs.GetComponent<MeshFilter>().sharedMesh;
        cbi.transform = v_trs.GetComponent<MeshFilter>().transform.localToWorldMatrix;
        cbi.subMeshIndex = 0;

        _combineList.Add(cbi);
    }
}
