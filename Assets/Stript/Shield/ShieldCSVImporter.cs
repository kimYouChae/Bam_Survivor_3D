using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ShieldCSVImporter : MonoBehaviour
{
    public static ShieldCSVImporter instance;
    // ##TODO : Shieldmanager 스크립트로 수정하면 거기에 넣어놓기

    // ShieldSize CSV 파일을 불러와서
    // 쉴드 pooling에서 초기생성할 때 값을 넣어주기

    // csv 파싱 정규식 
    string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    [Header("===Container===")]
    [SerializeField]
    private Dictionary<Shield_Effect, Tuple<Vector3, Vector3>> DIC_effectByMinMaxSize; // 이펙트별 min, max 사이즈

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // 초기화 
        DIC_effectByMinMaxSize = new Dictionary<Shield_Effect, Tuple<Vector3, Vector3>>();

        // CVS
        F_InitShieldDic();
    }

    // shield effect 별로 dictionary가 잘 세팅 되었다는 가정하에 예외는 없음 
    public Vector3 ShieldMin(Shield_Effect _effect) => DIC_effectByMinMaxSize[_effect].Item1;
    public Vector3 ShieldMax(Shield_Effect _effect) => DIC_effectByMinMaxSize[_effect].Item2;

    private void F_InitShieldDic() 
    {
        // cvs 파일을 텍스트 파일로 가져오기 
        TextAsset textAsset = Resources.Load("ShieldSize") as TextAsset;
        // 행 별로 자르기 
        string[] lines = Regex.Split(textAsset.text, LINE_SPLIT_RE);
        // 첫번째 행 자르기 
        string[] header = Regex.Split(lines[0], SPLIT_RE);

        for (int i = 1; i < lines.Length; i++)
        {
            // 행을 단어별로 자르기 
            string[] values = Regex.Split(lines[i], SPLIT_RE);

            // value[0] : _shieldType
            // value[1] : _minSize
            // value[2] : _maxSize

            // type string -> enum 
            Shield_Effect _effect = (Shield_Effect)Enum.Parse(typeof(Shield_Effect), values[0]);
            // min 문자열 자르기 
            string[] minList = values[1].Split("/");
            Vector3 _minVec = new Vector3(float.Parse(minList[0]), float.Parse(minList[1]), float.Parse(minList[2]));
            // max 문자열 자르기
            string[] maxList = values[2].Split("/");
            Vector3 _maxVec = new Vector3(float.Parse(maxList[0]), float.Parse(maxList[1]), float.Parse(maxList[2]));

            // Dictionary에 넣기 
            DIC_effectByMinMaxSize.Add(_effect , new Tuple<Vector3,Vector3>(_minVec , _maxVec));
        }
    }

}
