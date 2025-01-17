using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SkillCardCsvImporter : CSVManager
{
    // cvs���� '_classSpritName'�� �� �̸� 
    const string _skillcardColunm = "_classSpritName";
    // Ŭ������ �ش��ϴ� �ε��� 
    int _nameIdx;

    [Header("===Container===")]
    [SerializeField]
    private Dictionary<CardTier, List<SkillCard>> _tierBySkillCard;  // Ƽ� ��ųī��

    // ������Ƽ
    public Dictionary<CardTier, List<SkillCard>> tierBySkillCard => _tierBySkillCard;

    protected override void F_InitContainer()
    {
        // �����̸� �ʱ�ȭ
        FileName = "SkillCard";

        // dictionary �ʱ�ȭ
        F_InitDictionary();
    }

    protected override void F_ProcessData(string[] _data)
    {
        // _cardName�� ���° �ε������� ã��
        _nameIdx = Array.IndexOf(headerArray, _skillcardColunm);     // 2

        try
        {
            // 2. Skillcard ���� 
            // 2-1. name�� �̿��ؼ� Ŭ���� ���� ( Acticator ��� )
            SkillCard _card = (SkillCard)Activator.CreateInstance(Type.GetType(_data[_nameIdx]));

            // 2-2. skillcard�� �� �ֱ� 
            _card.F_InitField(_data);

            // 3. dictonary�� �ֱ�
            tierBySkillCard[_card.cardTier].Add(_card);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
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

}
