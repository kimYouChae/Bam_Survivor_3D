using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ShieldCSVImporter : MonoBehaviour
{
    public static ShieldCSVImporter instance;
    // ##TODO : Shieldmanager ��ũ��Ʈ�� �����ϸ� �ű⿡ �־����

    // ShieldSize CSV ������ �ҷ��ͼ�
    // ���� pooling���� �ʱ������ �� ���� �־��ֱ�

    // csv �Ľ� ���Խ� 
    string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    [Header("===Container===")]
    [SerializeField]
    private Dictionary<Shield_Effect, Tuple<Vector3, Vector3>> DIC_effectByMinMaxSize; // ����Ʈ�� min, max ������

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // �ʱ�ȭ 
        DIC_effectByMinMaxSize = new Dictionary<Shield_Effect, Tuple<Vector3, Vector3>>();

        // CVS
        F_InitShieldDic();
    }

    // shield effect ���� dictionary�� �� ���� �Ǿ��ٴ� �����Ͽ� ���ܴ� ���� 
    public Vector3 ShieldMin(Shield_Effect _effect) => DIC_effectByMinMaxSize[_effect].Item1;
    public Vector3 ShieldMax(Shield_Effect _effect) => DIC_effectByMinMaxSize[_effect].Item2;

    private void F_InitShieldDic() 
    {
        // cvs ������ �ؽ�Ʈ ���Ϸ� �������� 
        TextAsset textAsset = Resources.Load("ShieldSize") as TextAsset;
        // �� ���� �ڸ��� 
        string[] lines = Regex.Split(textAsset.text, LINE_SPLIT_RE);
        // ù��° �� �ڸ��� 
        string[] header = Regex.Split(lines[0], SPLIT_RE);

        for (int i = 1; i < lines.Length; i++)
        {
            // ���� �ܾ�� �ڸ��� 
            string[] values = Regex.Split(lines[i], SPLIT_RE);

            // value[0] : _shieldType
            // value[1] : _minSize
            // value[2] : _maxSize

            // type string -> enum 
            Shield_Effect _effect = (Shield_Effect)Enum.Parse(typeof(Shield_Effect), values[0]);
            // min ���ڿ� �ڸ��� 
            string[] minList = values[1].Split("/");
            Vector3 _minVec = new Vector3(float.Parse(minList[0]), float.Parse(minList[1]), float.Parse(minList[2]));
            // max ���ڿ� �ڸ���
            string[] maxList = values[2].Split("/");
            Vector3 _maxVec = new Vector3(float.Parse(maxList[0]), float.Parse(maxList[1]), float.Parse(maxList[2]));

            // Dictionary�� �ֱ� 
            DIC_effectByMinMaxSize.Add(_effect , new Tuple<Vector3,Vector3>(_minVec , _maxVec));
        }
    }

}
