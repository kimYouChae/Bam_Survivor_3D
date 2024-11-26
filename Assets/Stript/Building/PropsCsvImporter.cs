using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class PropsCsvImporter : MonoBehaviour
{
    // csv 파싱 정규식 
    string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    [Header("===Container===")]
    [SerializeField]
    private Dictionary<InGamePropState, Building> DICT_proptsTOBuilding;
    
    // state에 맞는 building 
    public Building F_StateToBuilding(InGamePropState _state) => DICT_proptsTOBuilding[_state];

    private void Start()
    {
        DICT_proptsTOBuilding = new Dictionary<InGamePropState, Building>();

        F_InitUnit();
    }


    private void F_InitUnit() 
    {
        // 텍스트 에셋 가져오기
        TextAsset _textAsset = Resources.Load("PropsBuilding") as TextAsset;

        // 행별로 자르기
        string[] lines = Regex.Split(_textAsset.text, LINE_SPLIT_RE);

        InGamePropState[] _type = (InGamePropState[])System.Enum.GetValues(typeof(InGamePropState));

        for (int i = 1; i < lines.Length; i++) 
        {
            string[] _value = Regex.Split(lines[i], SPLIT_RE);

            // building 생성 
            Building _bulding = new Building(_value);

            // 딕셔너리에 추가 
            F_PropsAddToDict(_type[i - 1] , _bulding);
        }
    }

    private void F_PropsAddToDict(InGamePropState _state , Building _build) 
    {
        if (!DICT_proptsTOBuilding.ContainsKey(_state))
        {
            DICT_proptsTOBuilding.Add(_state, _build);
        }
    }
}
