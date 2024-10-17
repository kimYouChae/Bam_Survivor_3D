using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;   // regex 사용
using UnityEngine;

public class SkillCardDatabase : MonoBehaviour
{
    // csv 파싱 정규식 
    string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    // cvs에서 '_classSpritName'인 열 이름 
    const string _skillcardColunm = "_classSpritName";

    [Header("===Container===")]
    [SerializeField]
    private Dictionary<CardTier, List<SkillCard>> _tierBySkillCard;  // 티어별 스킬카드

    // 프로퍼티
    public Dictionary<CardTier, List<SkillCard>> tierBySkillCard => _tierBySkillCard;

    private void Awake()
    {
        // dictionary 초기화
        F_InitDictionary();

        // cvs로 데이터 가져오기 
        F_InitSkillCard();

        // 잘됐는지 한번 실행해보자
        /*
        foreach (var temp in _tierBySkillCard) 
        {
            Debug.Log(temp.Key + "의 리스트 : ");

            for (int i = 0; i < temp.Value.Count; i++)
                Debug.Log(temp.Value[i].cardName);
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

    private void F_InitSkillCard() 
    {

        // cvs 파일을 텍스트 파일로 가져오기 
        TextAsset textAsset = Resources.Load("SkillCard") as TextAsset;
        // 행 별로 자르기 
        string[] lines = Regex.Split( textAsset.text , LINE_SPLIT_RE);
        // 첫번째 행 자르기 
        string[] header = Regex.Split(lines[0] , SPLIT_RE);

        // _cardName이 몇번째 인덱스인지 찾기
        int _nameIdx = Array.IndexOf(header, _skillcardColunm);     // 2

        for (int i = 1; i < lines.Length; i++)
        {
            // 1. 행을 단어별로 자르기 
            string[] values = Regex.Split(lines[i], SPLIT_RE);

            try
            {
                // 2. Skillcard 생성 
                // 2-1. name을 이용해서 클래스 생성 ( Acticator 사용 )
                SkillCard _card = (SkillCard)Activator.CreateInstance(Type.GetType(values[_nameIdx]));

                // 2-2. skillcard에 값 넣기 
                _card.F_InitField(values);

                // 3. dictonary에 넣기
                tierBySkillCard[_card.cardTier].Add(_card);

                // ##TODO : 4. card의 effect를 검사해서 , 각 스크립트의 딕셔너리에 저장해두기 
                /*
                switch (_card.cardAbility)
                {
                    case CardAbility.BulletExplosion:
                        PlayerManager.instance.markerExplosionConteroller.F_DictionaryInt(_card);
                        break;
                    case CardAbility.Shield:
                        PlayerManager.instance.markerShieldController.F_DictionaryInt(_card);
                        break;
                }
                */
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                continue;
            } 
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

}
