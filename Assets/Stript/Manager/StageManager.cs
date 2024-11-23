using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    // csv 파싱 정규식 
    string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    [SerializeField]
    private float _minutes;
    [SerializeField]
    private float _seconds;
    [SerializeField]
    private int _currStage;
    [SerializeField]
    private List<Stage> _stages;

    protected override void Singleton_Awake()
    {
        // 초기화 
        _currStage = 0;
        F_InitStage();
    }

    private void Start()
    {
        StartCoroutine(IE_StageFlow());
    }

    IEnumerator IE_StageFlow() 
    {
        // 초기세팅
        float _nextStageTime = _stages[1].StageMinites;
        _minutes = 0;
        _seconds = 0;

        for (int i = 1; i < _stages.Count - 1; i++) 
        {
            Debug.Log(_nextStageTime * 60);

            // 시간지나갈때까지 잠시 while에 가두기 
            while (true)
            {
                _seconds += Time.deltaTime;

                _minutes = _seconds / 60;

                if( _seconds >= _nextStageTime * 60 )
                {
                    break;
                }
                
                // 매프레임
                yield return null;

            }

            _nextStageTime = _stages[1 + i].StageMinites;
            _currStage++;
        }

    }

    public void F_InitStage() 
    {
        // 리스트 초기화 
        _stages = new List<Stage>();

        // 텍스트 파일로
        TextAsset _text = Resources.Load("Stage") as TextAsset;
        // 행별로 자르기
        string[] _lines = Regex.Split(_text.text , LINE_SPLIT_RE);

        for (int i = 1; i < _lines.Length; i++)
        {
            string[] _temp = Regex.Split(_lines[i], SPLIT_RE);

            Stage _stage = new Stage(_temp);

            _stages.Add( _stage );
        }

    }


}
