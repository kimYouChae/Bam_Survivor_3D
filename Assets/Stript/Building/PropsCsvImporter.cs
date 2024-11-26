using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class PropsCsvImporter : MonoBehaviour
{
    // csv �Ľ� ���Խ� 
    string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    [Header("===Container===")]
    [SerializeField]
    private Dictionary<InGamePropState, Building> DICT_proptsTOBuilding;
    
    // state�� �´� building 
    public Building F_StateToBuilding(InGamePropState _state) => DICT_proptsTOBuilding[_state];

    private void Start()
    {
        DICT_proptsTOBuilding = new Dictionary<InGamePropState, Building>();

        F_InitUnit();
    }


    private void F_InitUnit() 
    {
        // �ؽ�Ʈ ���� ��������
        TextAsset _textAsset = Resources.Load("PropsBuilding") as TextAsset;

        // �ະ�� �ڸ���
        string[] lines = Regex.Split(_textAsset.text, LINE_SPLIT_RE);

        InGamePropState[] _type = (InGamePropState[])System.Enum.GetValues(typeof(InGamePropState));

        for (int i = 1; i < lines.Length; i++) 
        {
            string[] _value = Regex.Split(lines[i], SPLIT_RE);

            // building ���� 
            Building _bulding = new Building(_value);

            // ��ųʸ��� �߰� 
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
