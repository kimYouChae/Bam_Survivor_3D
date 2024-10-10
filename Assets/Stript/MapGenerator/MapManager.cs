using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGridData 
{
    public List<bool> _visited = new List<bool>();
}

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [Header("===Grid===")]
    private bool[,] _mapGrid;
    [SerializeField] private int _mapSize = 140;
    [SerializeField] private string _mapGridFileName = "mapGridSaveFile";

    [Header("===Script===")]
    [SerializeField]
    private MapMeshCombiner _combiner;
    [SerializeField]
    private MapNavMeshBake _mapNavMeshBake;
    
    // ������Ƽ 
    public int mapSize => _mapSize;
    public bool[,] mapGrid => _mapGrid;
    public MapMeshCombiner mapCombiner => _combiner;
    public MapNavMeshBake mapNavMeshBake => _mapNavMeshBake;

    private void Awake()
    {
        instance = this;

        // �⺻�� false , �湮 �� true 
        _mapGrid = new bool[_mapSize + 1, _mapSize + 1];

        // Grid Init
        F_InitGrid();
    }

    private void F_InitGrid()
    {
        // Resources ���������� mapGridSaveFile.txt �������� 

        TextAsset textFile = Resources.Load(_mapGridFileName) as TextAsset;

        if (textFile == null)
        {
            Debug.LogError("MAP GRID ������ �������� �ʽ��ϴ�");
            return;
        }

        // text file�� json ���¶� json���� �ٲ㼭 ������ ��������
        // fromJson : json�� Ŭ������ ��ȯ<T>(json���� ��ȯ�� string)
        MapGridData mapGridData = JsonUtility.FromJson<MapGridData>(textFile.text);

        // �ӽ� 1������ ��� 
        List<bool> temp = new List<bool>();
        temp = mapGridData._visited;

        // 2ä���迭�� ��ȯ
        for (int y = 0; y < _mapSize; y++) 
        {
            for (int x = 0; x < _mapSize; x++)
            {
                _mapGrid[y,x] = temp[ _mapSize * x + y ];
            }
        }

    }


}
