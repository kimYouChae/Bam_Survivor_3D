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
    private GameObject          _cardSelectUi;              // ī�� ����ui �θ� 
    [SerializeField]
    private int                 _currCardIndex;             // ���� ���� �� ī�� �ε���  

    [Header("===UI===")]
    [SerializeField]
    private Image[]             _cardBgList;                // BackGround
    [SerializeField]
    private GameObject[]        _cardTierBg;                // ī�� Ƽ�� bg
    [SerializeField]
    private GameObject[]        _cardTierText;              // ī�� Ƽ�� text
    [SerializeField]
    private GameObject[]        _cardIcon;                  // ī�� ������ 
    [SerializeField]
    private TextMeshProUGUI[]   _cardNameList;              // ī�� �̸� ����Ʈ
    [SerializeField]
    private Image[]             _cardImageList;             // ī�� �̹��� ����Ʈ
    [SerializeField]
    private GameObject[]        _cardStartCount;            // ī�� count�� Star
    [SerializeField]
    private TextMeshProUGUI[]   _cardToopTipList;           // ī�� ���� ����Ʈ 
    [SerializeField]
    private GameObject[]        _cardPointSprite;           // ���� �� point ��������Ʈ 

    [Header("===Random Select Card===")]
    [SerializeField]
    private List<Tuple<CardTier, SkillCard>> _finalSelectCard; // �������� ���� �� ī�� 

    // ī�� ui On  
    public void F_ShowCard()
    {
        // idx �ʱ�ȭ
        _currCardIndex = -1;

        // ui on 
        _cardSelectUi.SetActive(true);

        // ���� ���õ� card List �޾ƿ��� 
        _finalSelectCard = SkillCardManager.Instance.F_FinalSelectCard();

        // ī�� ǥ���ϱ� 
        for (int i = 0; i < _finalSelectCard.Count; i++)
        {
            // ##TODO : finalSelectCard�� Tuple�� �ʿ䰡 ���µ� ?
            // skillcard�ȿ� tier�� ���ڳ�

            //F_UpdateCard();

            /*
            CardTier _currTier = _finalSelectCard[i].Item1;
            SkillCard _currCard = _finalSelectCard[i].Item2;

            // tier�� ���� ī�� �̹��� �ٲٱ� 
            _cardBgList[i].sprite
                = _cardTierSprite[(int)_finalSelectCard[i].Item1];

            Debug.Log(_finalSelectCard[i].Item1);

            // ī���̸�
            _cardNameList[i].text = _currCard.skillCardName;

            // ī�� name�� �´� sprite
            _cardImageList[i].sprite = ResourceManager.Instance.F_ReturnSkillNameToSprite(_currCard.classSpriteName);

            // ī�� ����
            _cardToopTipList[i].text = _currCard.cardToolTip;
            */
        }

    }

    // ui card idx �޾ƿ��� (0���� 4����)
    public void F_SetCardIndex(int v_idx) 
    {

        // ���� �ε��� != Ŭ���ε��� => card ui ȿ���ֱ� 
        if(_currCardIndex != v_idx) 
        {
            // ���õ� ī�� ũ�� Ű��� 
            _cardBgList[v_idx].GetComponent<RectTransform>().sizeDelta += new Vector2(20,20);

            // ���� ī�� ũ�� �۰�
            if(_currCardIndex != -1)
                _cardBgList[_currCardIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(350,600);

            // card point ��������Ʈ on 
            _cardPointSprite[v_idx].gameObject.SetActive(true);

            // ���� idx ���� 
            _currCardIndex = v_idx;

        }
        // ���� �ε��� == Ŭ�����ؽ� => �ش� skillcard�� ȿ�� ���� 
        else 
        {
            // Ŀ���� ī�� �ǵ�������
            _cardBgList[_currCardIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(350, 600);

            // ui off
            _cardSelectUi.SetActive(false);

            // card point ��������Ʈ off
            _cardPointSprite[v_idx].gameObject.SetActive(true);

            // index�� �´� skillcard Ŭ������ return�� �� ���� 
            SkillCardManager.Instance.F_applyEffectBySkillcard(_finalSelectCard[_currCardIndex]);

            // pause �Ǿ��ִٸ� �ð� ���ư���
            if( Time.timeScale == 0 )
                Time.timeScale = 1;
        }
    }

    private void F_UpdateCard(SkillCard _card, int _i) 
    {
        CardTier _tier          = _card.cardTier;
        CardAbility _ability    = _card.cardAbility;

        // tier �� ī�� bg ���� 
        _cardBgList[_i].sprite = ResourceManager.Instance.cardBackGround(_tier);

        // tier �� tier �̹��� ���� 

        //##TODO : �����ؾ��� ! 
        
    }

}
