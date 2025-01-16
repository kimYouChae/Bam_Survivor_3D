using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ShieldCSVImporter : CSVManager
{

    [Header("===Container===")]
    [SerializeField]
    private Dictionary<Shield_Effect, Tuple<Vector3, Vector3>> DIC_effectByMinMaxSize; // ����Ʈ�� min, max ������

    // shield effect ���� dictionary�� �� ���� �Ǿ��ٴ� �����Ͽ� ���ܴ� ���� 
    public Vector3 ShieldMin(Shield_Effect _effect) => DIC_effectByMinMaxSize[_effect].Item1;
    public Vector3 ShieldMax(Shield_Effect _effect) => DIC_effectByMinMaxSize[_effect].Item2;

    protected override void F_InitContainer()
    {
        // ���ϸ��ʱ�ȭ
        FileName = "ShieldSize";

        // �ʱ�ȭ 
        DIC_effectByMinMaxSize = new Dictionary<Shield_Effect, Tuple<Vector3, Vector3>>();

    }

    protected override void F_ProcessData(string[] _data)
    {
        // type string -> enum 
        Shield_Effect _effect = (Shield_Effect)Enum.Parse(typeof(Shield_Effect), _data[0]);
        // min ���ڿ� �ڸ��� 
        string[] minList = _data[1].Split("/");
        Vector3 _minVec = new Vector3(float.Parse(minList[0]), float.Parse(minList[1]), float.Parse(minList[2]));
        // max ���ڿ� �ڸ���
        string[] maxList = _data[2].Split("/");
        Vector3 _maxVec = new Vector3(float.Parse(maxList[0]), float.Parse(maxList[1]), float.Parse(maxList[2]));

        // Dictionary�� �ֱ� 
        DIC_effectByMinMaxSize.Add(_effect, new Tuple<Vector3, Vector3>(_minVec, _maxVec));
    }

}
