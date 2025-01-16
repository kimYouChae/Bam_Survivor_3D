using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ShieldCSVImporter : CSVManager
{

    [Header("===Container===")]
    [SerializeField]
    private Dictionary<Shield_Effect, Tuple<Vector3, Vector3>> DIC_effectByMinMaxSize; // 이펙트별 min, max 사이즈

    // shield effect 별로 dictionary가 잘 세팅 되었다는 가정하에 예외는 없음 
    public Vector3 ShieldMin(Shield_Effect _effect) => DIC_effectByMinMaxSize[_effect].Item1;
    public Vector3 ShieldMax(Shield_Effect _effect) => DIC_effectByMinMaxSize[_effect].Item2;

    protected override void F_InitContainer()
    {
        // 파일명초기화
        FileName = "ShieldSize";

        // 초기화 
        DIC_effectByMinMaxSize = new Dictionary<Shield_Effect, Tuple<Vector3, Vector3>>();

    }

    protected override void F_ProcessData(string[] _data)
    {
        // type string -> enum 
        Shield_Effect _effect = (Shield_Effect)Enum.Parse(typeof(Shield_Effect), _data[0]);
        // min 문자열 자르기 
        string[] minList = _data[1].Split("/");
        Vector3 _minVec = new Vector3(float.Parse(minList[0]), float.Parse(minList[1]), float.Parse(minList[2]));
        // max 문자열 자르기
        string[] maxList = _data[2].Split("/");
        Vector3 _maxVec = new Vector3(float.Parse(maxList[0]), float.Parse(maxList[1]), float.Parse(maxList[2]));

        // Dictionary에 넣기 
        DIC_effectByMinMaxSize.Add(_effect, new Tuple<Vector3, Vector3>(_minVec, _maxVec));
    }

}
