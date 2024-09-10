using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectUI : MonoBehaviour
{
    [Header("===CardSelectTool===")]
    [SerializeField]
    private GameObject _cardSelectUi;       // ī�� ����ui �θ� 
    [SerializeField]
    private int _currCardIndex;             // ���� ���� �� ī�� �ε���  
    [SerializeField]
    private GameObject[] _cardList;        // ī�� ���� ui�� ��ųī�� 
    [SerializeField]
    private TextMeshProUGUI[] _cardNameList;    // ī�� �̸� ����Ʈ
    [SerializeField]
    private Image[] _cardImageList;             // ī�� �̹��� ����Ʈ
    [SerializeField]
    private TextMeshProUGUI[] _cardToopTipList; // ī�� ���� ����Ʈ 

    [Header("===CardTierSprit===")]
    [SerializeField]
    private Sprite[] _cardTierSprite;           // tier�� ���� ��������Ʈ 

    [SerializeField]
    private List<Tuple<CardTier, SkillCard>> _finalSelectCard; // �������� ���� �� ī�� 

    void Update()
    {
        //  ##TODO : �ӽ÷� L ������ 
        if (Input.GetKeyDown(KeyCode.L))
        {
            F_ShowCard();
        }
    }

    // ī�� ui On  
    private void F_ShowCard()
    {
        // idx �ʱ�ȭ
        _currCardIndex = -1;

        // ui on 
        _cardSelectUi.SetActive(true);

        // ���� ���õ� card List �޾ƿ��� 
        _finalSelectCard = SkillCardManager.instance.F_FinalSelectCard();

        // ī�� ǥ���ϱ� 
        for (int i = 0; i < _finalSelectCard.Count; i++)
        {
            CardTier _currTier = _finalSelectCard[i].Item1;
            SkillCard _currCard = _finalSelectCard[i].Item2;

            // tier�� ���� ī�� �̹��� �ٲٱ� 
            _cardList[i].GetComponent<Image>().sprite
                = _cardTierSprite[(int)_finalSelectCard[i].Item1];

            Debug.Log(_finalSelectCard[i].Item1);

            // ī���̸�
            _cardNameList[i].text = _currCard.cardName;

            // ī�� idx�� �´� sprite
            _cardImageList[i].sprite = ResourceManager.instance.skillCardSprites[_currCard.cardIndex];

            // ī�� ����
            _cardToopTipList[i].text = _currCard.cardToolTip;
        }

    }

    // ui card idx �޾ƿ��� (0���� 4����)
    public void F_SetCardIndex(int v_idx) 
    {

        // ���� �ε��� != Ŭ���ε��� => card ui ȿ���ֱ� 
        if(_currCardIndex != v_idx) 
        {
            // ���õ� ī�� ũ�� Ű��� 
            _cardList[v_idx].GetComponent<RectTransform>().sizeDelta += new Vector2(20,20);

            // ���� ī�� ũ�� �۰�
            if(_currCardIndex != -1)
                _cardList[_currCardIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(350,600);

            // ���� idx ���� 
            _currCardIndex = v_idx;

        }
        // ���� �ε��� == Ŭ�����ؽ� => �ش� skillcard�� ȿ�� ���� 
        else 
        {
            // Ŀ���� ī�� �ǵ�������
            _cardList[_currCardIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(350, 600);

            // ui on 
            _cardSelectUi.SetActive(false);

            // index�� �´� skillcard Ŭ������ return�� �� ���� 
            SkillCardManager.instance.F_applyEffectBySkillcard(_finalSelectCard[_currCardIndex]);
        }
    }

}
