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
    ///     �� tree1
    ///     �� tree2
    /// downTree
    ///     �� tree1
    ///     �� tree2
    /// : �̷��������� �θ� ���, �� ������ �ڽ��� ���鼭 combine 
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

    // ������Ƽ 
    public List<GameObject> combineFinishObject => _combineFinishObjectList;

    private void Start()
    {
        _meshCount = 0;
        _overMaxCountMesh = 0;
        _combineList = new List<CombineInstance>();

        // 1.Conbine �ϱ� 
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


    #region NavMesh�ϱ����� �����۾� (tree�� �ش��ϴ� ������Ʈ ���� �� mesh ��ġ��)
    /*
    public void F_CombineTreeMesh( List<GameObject> v_list , CombineType v_type) 
    {
        _combineList.Clear();
        Material _material = null;

        for (int i = 0; i < v_list.Count; i++) 
        {
            Transform _child = v_list[i].transform;

            // mesh �ִ� vertex �� ������ �ȵ� 
            if (_meshCount > 65500) 
            {
                // 1. �θ� �ϳ� �� ����� 
                GameObject _temp        = Instantiate(_treeMeshParentPrefab);
                _temp.transform.parent  = _combineObject[0].transform.parent.transform;
                _combineObject.Add( _temp );

                // 2. navMesh Obstacle ����Ʈ�� �ֱ�
                MapManager.instance.mapNavMeshBake.F_AddObstacle(_temp);

                // 3. mesh ��ġ��
                F_CombineMesh(v_type , _material , _overMaxCountMesh);
                _overMaxCountMesh ++ ;

                // 4. �ʱ�ȭ
                _combineList.Clear();
                _meshCount = 0;
            }

            _meshCount += 24;
            F_CreateComIns(_child);

            // ##TODO
            // �ӽ� ) destory
            Destroy(_child.gameObject);

            if (_material != null)
                continue;

            _material = _child.GetComponent<MeshRenderer>().material;
        }

        // �������� ��� box�� 
        // 1 .�θ� �����
        GameObject _temp2 = Instantiate(_treeMeshParentPrefab);
        _temp2.transform.parent = _combineObject[0].transform.parent.transform;
        _combineObject.Add(_temp2);

        // 2. obstacle ����Ʈ�� �ֱ�
        MapManager.instance.mapNavMeshBake.F_AddObstacle(_temp2);

        // 3. Mesh ��ġ�� 
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

            // Combine �� ������Ʈ�� �� ���� (tree ���� )
            v_list[i].gameObject.SetActive(false);
        }


        F_CombineMesh(v_type, _material, 0);

    }

    private void F_CombineMesh(CombineType v_type , Material v_mat, int v_blank) 
    {
        // mesh ��ġ�� 
        Mesh _combineMesh = new Mesh();
        _combineMesh.CombineMeshes(_combineList.ToArray(), true);

        Debug.Log(v_type + " : " + _combineList.Count);

        // �ð������� ���� 
        _combineFinishObjectList[(int)v_type + v_blank].GetComponent<MeshFilter>().sharedMesh = _combineMesh;
        _combineFinishObjectList[(int)v_type + v_blank].GetComponent<MeshRenderer>().sharedMaterial = v_mat;

        // ������Ʈ�� �ݶ��̴� �߰�
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
