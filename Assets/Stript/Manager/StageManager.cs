using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    // csv �Ľ� ���Խ� 
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
        // �ʱ�ȭ 
        _currStage = 0;
        F_InitStage();
    }

    private void Start()
    {
        StartCoroutine(IE_StageFlow());
    }

    IEnumerator IE_StageFlow() 
    {
        // �ʱ⼼��
        float _nextStageTime = _stages[1].StageMinites;
        _minutes = 0;
        _seconds = 0;

        for (int i = 1; i < _stages.Count - 1; i++) 
        {
            Debug.Log(_nextStageTime * 60);

            // �ð������������� ��� while�� ���α� 
            while (true)
            {
                _seconds += Time.deltaTime;

                _minutes = _seconds / 60;

                if( _seconds >= _nextStageTime * 60 )
                {
                    break;
                }
                
                // ��������
                yield return null;

            }

            _nextStageTime = _stages[1 + i].StageMinites;
            _currStage++;
        }

    }

    public void F_InitStage() 
    {
        // ����Ʈ �ʱ�ȭ 
        _stages = new List<Stage>();

        // �ؽ�Ʈ ���Ϸ�
        TextAsset _text = Resources.Load("Stage") as TextAsset;
        // �ະ�� �ڸ���
        string[] _lines = Regex.Split(_text.text , LINE_SPLIT_RE);

        for (int i = 1; i < _lines.Length; i++)
        {
            string[] _temp = Regex.Split(_lines[i], SPLIT_RE);

            Stage _stage = new Stage(_temp);

            _stages.Add( _stage );
        }

    }


}
