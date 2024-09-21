using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JetBrains.Annotations;

[System.Serializable]
public class GridMapWrapper
{
    public List<bool> _visited = new List<bool>();

    // 생성자
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
    [Header("===저장경로===")]
    private string _savePath;
    private string _mapGridFileName = "mapGridSaveFile";

    private void Start()
    {
        // 경로지정 (경로 : C:\Users\[user name]\AppData\LocalLow\[company name]\[product name])
        _savePath = Application.persistentDataPath + "/save/";

        StartCoroutine(F_DelayMapGridSave());
    }

    IEnumerator F_DelayMapGridSave() 
    {
        yield return new WaitForSeconds(1f);

        // map Grid 저장 
        F_MapGridSave();

    }

    private void F_MapGridSave() 
    {
        // 파일이 있으면 ? return
        if (File.Exists(_savePath + _mapGridFileName))
        {
            Debug.Log("이미 파일이 있습니다");
            return;
        }

        // 디렉토리(파일)가 없으면 -> 만들기 
        if (!Directory.Exists(_savePath)) 
        {
            Debug.Log(_savePath + " 디렉토리를 생성합니다 ");
            Directory.CreateDirectory(_savePath);
        }

        Debug.Log(_mapGridFileName + "을 저장합니다");
        // wrapper 생성
        GridMapWrapper gridMapWrapper = new GridMapWrapper();

        // 제이슨으로 변환 
        string mapGridJson = JsonUtility.ToJson(gridMapWrapper);

        Debug.Log(mapGridJson);

        // 파일 쓰기
        File.WriteAllText(_savePath + _mapGridFileName, mapGridJson);

    }
}
