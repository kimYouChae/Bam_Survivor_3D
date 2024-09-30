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

    // ������Ƽ 
    public int mapSize => _mapSize;
    public bool[,] mapGrid => _mapGrid;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
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
        for (int i = 0; i < _mapSize; i++) 
        {
            for (int j = 0; j < _mapSize; j++)
            {
                _mapGrid[i, j] = temp[ _mapSize * i + j ];
            }
        }

    }

}
