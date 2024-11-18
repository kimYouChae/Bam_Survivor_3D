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
    [SerializeField]
    private GameObject[]        _cardList;                  // ī�� ���� ui�� ��ųī�� 
    [SerializeField]
    private TextMeshProUGUI[]   _cardNameList;              // ī�� �̸� ����Ʈ
    [SerializeField]
    private Image[]             _cardImageList;             // ī�� �̹��� ����Ʈ
    [SerializeField]
    private TextMeshProUGUI[]   _cardToopTipList;           // ī�� ���� ����Ʈ 
    [SerializeField]
    private GameObject[]        _cardPointSprite;           // ���� �� point ��������Ʈ 

    [Header("===CardTierSprit===")]
    [SerializeField]
    private Sprite[] _cardTierSprite;           // tier�� ���� ��������Ʈ 

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
            CardTier _currTier = _finalSelectCard[i].Item1;
            SkillCard _currCard = _finalSelectCard[i].Item2;

            // tier�� ���� ī�� �̹��� �ٲٱ� 
            _cardList[i].GetComponent<Image>().sprite
                = _cardTierSprite[(int)_finalSelectCard[i].Item1];

            Debug.Log(_finalSelectCard[i].Item1);

            // ī���̸�
            _cardNameList[i].text = _currCard.skillCardName;

            // ī�� idx�� �´� sprite
            _cardImageList[i].sprite = ResourceManager.Instance.F_ReturnSkillNameToSprite(_currCard.classSpriteName);

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

            // card point ��������Ʈ on 
            _cardPointSprite[v_idx].gameObject.SetActive(true);

            // ���� idx ���� 
            _currCardIndex = v_idx;

        }
        // ���� �ε��� == Ŭ�����ؽ� => �ش� skillcard�� ȿ�� ���� 
        else 
        {
            // Ŀ���� ī�� �ǵ�������
            _cardList[_currCardIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(350, 600);

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

}
