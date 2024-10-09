using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UIElements;

enum CombineType 
{
    Water,
    Soil
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
    private List<GameObject> _combineObject;

    [Header("===Container===")]
    private List<CombineInstance> _combineList;


    private void Start()
    {
         _combineList = new List<CombineInstance>();

        // 1.Conbine 하기 
        F_CombineMesh(_waterParent, CombineType.Water);
        F_CombineMesh(_soilParent , CombineType.Soil);
    }

    private void F_CombineMesh(List<GameObject> v_list , CombineType v_type) 
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

            // Combine 한 오브젝트는 다 끄기 
            v_list[i].gameObject.SetActive(false);
        }

        // mesh 합치기 
        Mesh _combineMesh = new Mesh();
        _combineMesh.CombineMeshes(_combineList.ToArray() , true);

        Debug.Log(v_type + " : " + _combineList.Count);

        // 시각적으로 보기 
        _combineObject[(int)v_type].GetComponent<MeshFilter>().sharedMesh = _combineMesh;
        _combineObject[(int)v_type].GetComponent<MeshRenderer>().sharedMaterial = _material;

        // 오브젝트에 콜라이더 추가
        if (_combineObject[(int)v_type].GetComponent<MeshCollider>() == null)
            _combineObject[(int)v_type].AddComponent<MeshCollider>();
        

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
