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
    [Header("===Grid===")]
    private bool[,] _mapGrid;
    [SerializeField] private int _mapSize = 140;
    [SerializeField] private string _mapGridFileName = "mapGridSaveFile";
    [SerializeField] private GameObject _object;
    [SerializeField] private Transform _parent;

    private void Start()
    {
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
        for (int i = 0; i < _mapSize; i++) 
        {
            for (int j = 0; j < _mapSize; j++)
            {
                _mapGrid[i, j] = temp[ _mapSize * i + j ];
            }
        }

        /*
        // 잘들어갔는지 테스트 
        for (int i = 0; i < _mapSize; i++)
        {
            for (int j = 0; j < _mapSize; j++)
            {
                if (_mapGrid[i, j] == true)
                    Instantiate(_object, new Vector3(i, 0, j), Quaternion.identity, _parent);
            }
        }
        */
    }

}
