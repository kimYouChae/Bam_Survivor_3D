using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class CSVManager : MonoBehaviour
{
    // csv 파싱 정규식 
    [SerializeField]
    protected string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    [SerializeField]
    protected string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    [SerializeField]
    protected string FileName;      // resource에서 불러올 파일명
    [SerializeField]
    protected string[] headerArray; // Skillcard Importer에서 사용함 

    private void Awake()
    {
        // 하위에서 각자 컨테이너 초기화
        F_InitContainer();

        // csv 불러오기
        F_CsvImport();
    }

    protected void F_CsvImport()
    {
        // 유닛 텍스트 에셋 가져오기
        TextAsset _textAsset = Resources.Load(FileName) as TextAsset;

        // 행별로 자르기
        string[] lines = Regex.Split(_textAsset.text, LINE_SPLIT_RE);

        // 첫번쨰 행 자르기 
        string[] header = Regex.Split(lines[0], SPLIT_RE);

        // skillard importer에서 사용할
        headerArray = header;

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = Regex.Split(lines[i], SPLIT_RE);

            // 자식 스크립트에서 데이터 처리 
            F_ProcessData(values);
        }
    }

    protected abstract void F_ProcessData(string[] _data);
    protected abstract void F_InitContainer();
   
}
