using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SkillCardCsvImporter : CSVManager
{
    // cvs에서 '_classSpritName'인 열 이름 
    const string _skillcardColunm = "_classSpritName";
    // 클래스명에 해당하는 인덱스 
    int _nameIdx;

    [Header("===Container===")]
    [SerializeField]
    private Dictionary<CardTier, List<SkillCard>> _tierBySkillCard;  // 티어별 스킬카드

    // 프로퍼티
    public Dictionary<CardTier, List<SkillCard>> tierBySkillCard => _tierBySkillCard;

    protected override void F_InitContainer()
    {
        // 파일이름 초기화
        FileName = "SkillCard";

        // dictionary 초기화
        F_InitDictionary();
    }

    protected override void F_ProcessData(string[] _data)
    {
        // _cardName이 몇번째 인덱스인지 찾기
        _nameIdx = Array.IndexOf(headerArray, _skillcardColunm);     // 2

        try
        {
            // 2. Skillcard 생성 
            // 2-1. name을 이용해서 클래스 생성 ( Acticator 사용 )
            SkillCard _card = (SkillCard)Activator.CreateInstance(Type.GetType(_data[_nameIdx]));

            // 2-2. skillcard에 값 넣기 
            _card.F_InitField(_data);

            // 3. dictonary에 넣기
            tierBySkillCard[_card.cardTier].Add(_card);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }

        // 딕셔너리 출력해보기
        /*
        for (int i = 0; i < _tierBySkillCard.Count; i++) 
        {
            var temp = _tierBySkillCard.ElementAt(i);
            Debug.Log(temp.Key + " ");
            for (int j = 0; j < temp.Value.Count; j++) 
            {
                Debug.Log(temp.Value[j].skillCardName);
            }
        }
        */
    }

    private void F_InitDictionary()
    {
        // 시작할 때 dictonray를 초기화 하고
        // cvs에서 데이터 가져올 때 함수로 값 넣으면 될듯 

        _tierBySkillCard = new Dictionary<CardTier, List<SkillCard>>();

        // 0. list 초기화 ,  카드 티어만큼 초기화 
        for (int i = 0; i < System.Enum.GetValues(typeof(CardTier)).Length; i++)
        {
            CardTier _temp = (CardTier)i;

            // dictionary에 key가 없으면? 추가 후 list 초기화
            if (!tierBySkillCard.ContainsKey(_temp))
            {
                tierBySkillCard[_temp] = new List<SkillCard>();
            }
        }

    }

}
