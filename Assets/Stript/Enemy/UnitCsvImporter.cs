using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class UnitCsvImporter : MonoBehaviour
{
    // csv �Ľ� ���Խ� 
    string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    [Header("===Container===")]
    [SerializeField]
    private Dictionary<Unit_Animal_Type, UnitState> DICT_AnimalTypeToUnitState;

    // type�� state return
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

    // type �� state return
    public UnitState F_AnimalTypeToState(Unit_Animal_Type _type)
    {
        if (!DICT_AnimalTypeToUnitState.ContainsKey(_type))
            return null;

        return DICT_AnimalTypeToUnitState[_type];
    }


    private void F_InitUnit() 
    {
        // ���� �ؽ�Ʈ ���� ��������
        TextAsset _textAsset = Resources.Load("Unit") as TextAsset;

        // �ະ�� �ڸ���
        string[] lines = Regex.Split(_textAsset.text, LINE_SPLIT_RE);

        // ù���� �� �ڸ��� 
        string[] header = Regex.Split(lines[0], SPLIT_RE);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = Regex.Split(lines[i], SPLIT_RE);
            
            // state ���� 
            UnitState _unitState = new UnitState(values);

            // DICT�� �ֱ� 
            DICT_AnimalTypeToUnitState.Add( _unitState.AnimalType , _unitState );
        }

    }
}
