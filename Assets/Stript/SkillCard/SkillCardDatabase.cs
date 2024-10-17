using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;   // regex ���
using UnityEngine;

public class SkillCardDatabase : MonoBehaviour
{
    // csv �Ľ� ���Խ� 
    string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    // cvs���� '_classSpritName'�� �� �̸� 
    const string _skillcardColunm = "_classSpritName";

    [Header("===Container===")]
    [SerializeField]
    private Dictionary<CardTier, List<SkillCard>> _tierBySkillCard;  // Ƽ� ��ųī��

    // ������Ƽ
    public Dictionary<CardTier, List<SkillCard>> tierBySkillCard => _tierBySkillCard;

    private void Awake()
    {
        // dictionary �ʱ�ȭ
        F_InitDictionary();

        // cvs�� ������ �������� 
        F_InitSkillCard();

        // �ߵƴ��� �ѹ� �����غ���
        /*
        foreach (var temp in _tierBySkillCard) 
        {
            Debug.Log(temp.Key + "�� ����Ʈ : ");

            for (int i = 0; i < temp.Value.Count; i++)
                Debug.Log(temp.Value[i].cardName);
        }
        */
    }

    private void F_InitDictionary()
    {
        // ������ �� dictonray�� �ʱ�ȭ �ϰ�
        // cvs���� ������ ������ �� �Լ��� �� ������ �ɵ� 

        _tierBySkillCard = new Dictionary<CardTier, List<SkillCard>>();

        // 0. list �ʱ�ȭ ,  ī�� Ƽ�ŭ �ʱ�ȭ 
        for (int i = 0; i < System.Enum.GetValues(typeof(CardTier)).Length; i++)
        {
            CardTier _temp = (CardTier)i;

            // dictionary�� key�� ������? �߰� �� list �ʱ�ȭ
            if (!tierBySkillCard.ContainsKey(_temp)) 
            {
                tierBySkillCard[_temp] = new List<SkillCard>();
            }
        }

    }

    private void F_InitSkillCard() 
    {

        // cvs ������ �ؽ�Ʈ ���Ϸ� �������� 
        TextAsset textAsset = Resources.Load("SkillCard") as TextAsset;
        // �� ���� �ڸ��� 
        string[] lines = Regex.Split( textAsset.text , LINE_SPLIT_RE);
        // ù��° �� �ڸ��� 
        string[] header = Regex.Split(lines[0] , SPLIT_RE);

        // _cardName�� ���° �ε������� ã��
        int _nameIdx = Array.IndexOf(header, _skillcardColunm);     // 2

        for (int i = 1; i < lines.Length; i++)
        {
            // 1. ���� �ܾ�� �ڸ��� 
            string[] values = Regex.Split(lines[i], SPLIT_RE);

            try
            {
                // 2. Skillcard ���� 
                // 2-1. name�� �̿��ؼ� Ŭ���� ���� ( Acticator ��� )
                SkillCard _card = (SkillCard)Activator.CreateInstance(Type.GetType(values[_nameIdx]));

                // 2-2. skillcard�� �� �ֱ� 
                _card.F_InitField(values);

                // 3. dictonary�� �ֱ�
                tierBySkillCard[_card.cardTier].Add(_card);

                // ##TODO : 4. card�� effect�� �˻��ؼ� , �� ��ũ��Ʈ�� ��ųʸ��� �����صα� 
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

        // ��ųʸ� ����غ���
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
