using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class UnitCsvImporter : MonoBehaviour
{
    // csv 파싱 정규식 
    string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    [Header("===Container===")]
    [SerializeField]
    private Dictionary<Unit_Animal_Type, UnitState> DICT_AnimalTypeToUnitState;

    // type별 state return
    public UnitState F_TypeByState(Unit_Animal_Type _type) 
    {
        if(DICT_AnimalTypeToUnitState.ContainsKey(_type))
            return DICT_AnimalTypeToUnitState[_type];

        return null;
    }

    void Start()
    {
        DICT_AnimalTypeToUnitState = new Dictionary<Unit_Animal_Type, UnitState>();

        F_InitUnit();
    }

    // type 별 state return
    public UnitState F_AnimalTypeToState(Unit_Animal_Type _type)
    {
        if (!DICT_AnimalTypeToUnitState.ContainsKey(_type))
            return null;

        return DICT_AnimalTypeToUnitState[_type];
    }


    private void F_InitUnit() 
    {
        // 유닛 텍스트 에셋 가져오기
        TextAsset _textAsset = Resources.Load("Unit") as TextAsset;

        // 행별로 자르기
        string[] lines = Regex.Split(_textAsset.text, LINE_SPLIT_RE);

        // 첫번쨰 행 자르기 
        string[] header = Regex.Split(lines[0], SPLIT_RE);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = Regex.Split(lines[i], SPLIT_RE);
            
            // state 생성 
            UnitState _unitState = new UnitState(values);

            // DICT에 넣기 
            DICT_AnimalTypeToUnitState.Add( _unitState.AnimalType , _unitState );
        }

    }
}
