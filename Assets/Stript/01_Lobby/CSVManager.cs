using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class CSVManager : MonoBehaviour
{
    // csv �Ľ� ���Խ� 
    [SerializeField]
    protected string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    [SerializeField]
    protected string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    [SerializeField]
    protected string FileName;      // resource���� �ҷ��� ���ϸ�
    [SerializeField]
    protected string[] headerArray; // Skillcard Importer���� ����� 

    private void Awake()
    {
        // �������� ���� �����̳� �ʱ�ȭ
        F_InitContainer();

        // csv �ҷ�����
        F_CsvImport();
    }

    protected void F_CsvImport()
    {
        // ���� �ؽ�Ʈ ���� ��������
        TextAsset _textAsset = Resources.Load(FileName) as TextAsset;

        // �ະ�� �ڸ���
        string[] lines = Regex.Split(_textAsset.text, LINE_SPLIT_RE);

        // ù���� �� �ڸ��� 
        string[] header = Regex.Split(lines[0], SPLIT_RE);

        // skillard importer���� �����
        headerArray = header;

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = Regex.Split(lines[i], SPLIT_RE);

            // �ڽ� ��ũ��Ʈ���� ������ ó�� 
            F_ProcessData(values);
        }
    }

    protected abstract void F_ProcessData(string[] _data);
    protected abstract void F_InitContainer();
   
}
