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
    
    // 프로퍼티 
    public int mapSize => _mapSize;
    public bool[,] mapGrid => _mapGrid;
    public MapMeshCombiner mapCombiner => _combiner;
    public MapNavMeshBake mapNavMeshBake => _mapNavMeshBake;

    private void Awake()
    {
        instance = this;

        // 기본값 false , 방문 시 true 
        _mapGrid = new bool[_mapSize + 1, _mapSize + 1];

        // Grid Init
        F_InitGrid();
    }

    private void F_InitGrid()
    {
        // Resources 폴더하위의 mapGridSaveFile.txt 가져오기 

        TextAsset textFile = Resources.Load(_mapGridFileName) as TextAsset;

        if (textFile == null)
        {
            Debug.LogError("MAP GRID 파일이 존재하지 않습니다");
            return;
        }

        // text file이 json 형태라서 json으로 바꿔서 저장후 얕은복사
        // fromJson : json을 클래스로 변환<T>(json으로 변환된 string)
        MapGridData mapGridData = JsonUtility.FromJson<MapGridData>(textFile.text);

        // 임시 1차원에 담기 
        List<bool> temp = new List<bool>();
        temp = mapGridData._visited;

        // 2채원배열로 변환
        for (int y = 0; y < _mapSize; y++) 
        {
            for (int x = 0; x < _mapSize; x++)
            {
                _mapGrid[y,x] = temp[ _mapSize * x + y ];
            }
        }

    }


}
