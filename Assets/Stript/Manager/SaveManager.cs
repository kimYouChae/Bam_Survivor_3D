using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JetBrains.Annotations;

[System.Serializable]
public class GridMapWrapper
{
    public List<bool> _visited = new List<bool>();

    // ������
    public GridMapWrapper() 
    {
        F_InitGridMapVisiedList();
    }

    public void F_InitGridMapVisiedList() 
    {
        int mapSize = GameManager.instance.mapSize;
        for(int i = 0; i < mapSize; i++) 
        {
            for (int j = 0; j < mapSize; j++)
            {
                _visited.Add(GameManager.instance.mapGrid[i,j]);
            }
        }
    }
    

}

public class SaveManager : MonoBehaviour
{
    [Header("===������===")]
    private string _savePath;
    private string _mapGridFileName = "mapGridSaveFile";

    private void Start()
    {
        // ������� (��� : C:\Users\[user name]\AppData\LocalLow\[company name]\[product name])
        _savePath = Application.persistentDataPath + "/save/";

        StartCoroutine(F_DelayMapGridSave());
    }

    IEnumerator F_DelayMapGridSave() 
    {
        yield return new WaitForSeconds(1f);

        // map Grid ���� 
        F_MapGridSave();

    }

    private void F_MapGridSave() 
    {
        // ������ ������ ? return
        if (File.Exists(_savePath + _mapGridFileName))
        {
            Debug.Log("�̹� ������ �ֽ��ϴ�");
            return;
        }

        // ���丮(����)�� ������ -> ����� 
        if (!Directory.Exists(_savePath)) 
        {
            Debug.Log(_savePath + " ���丮�� �����մϴ� ");
            Directory.CreateDirectory(_savePath);
        }

        Debug.Log(_mapGridFileName + "�� �����մϴ�");
        // wrapper ����
        GridMapWrapper gridMapWrapper = new GridMapWrapper();

        // ���̽����� ��ȯ 
        string mapGridJson = JsonUtility.ToJson(gridMapWrapper);

        Debug.Log(mapGridJson);

        // ���� ����
        File.WriteAllText(_savePath + _mapGridFileName, mapGridJson);

    }
}
