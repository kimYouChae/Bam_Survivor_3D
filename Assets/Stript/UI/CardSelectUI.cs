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
    private GameObject _cardSelectUi;       // 카드 선택ui 부모 
    [SerializeField]
    private int _currCardIndex;             // 현재 선택 된 카드 인덱스  
    [SerializeField]
    private GameObject[] _cardList;        // 카드 선택 ui의 스킬카드 
    [SerializeField]
    private TextMeshProUGUI[] _cardNameList;    // 카드 이름 리스트
    [SerializeField]
    private Image[] _cardImageList;             // 카드 이미지 리스트
    [SerializeField]
    private TextMeshProUGUI[] _cardToopTipList; // 카드 툴팁 리스트 

    [Header("===CardTierSprit===")]
    [SerializeField]
    private Sprite[] _cardTierSprite;           // tier에 따른 스프라이트 

    [SerializeField]
    private List<Tuple<CardTier, SkillCard>> _finalSelectCard; // 랜덤으로 선택 된 카드 

    void Update()
    {
        //  ##TODO : 임시로 L 누르면 
        if (Input.GetKeyDown(KeyCode.L))
        {
            F_ShowCard();
        }
    }

    // 카드 ui On  
    private void F_ShowCard()
    {
        // idx 초기화
        _currCardIndex = -1;

        // ui on 
        _cardSelectUi.SetActive(true);

        // 최종 선택된 card List 받아오기 
        _finalSelectCard = SkillCardManager.instance.F_FinalSelectCard();

        // 카드 표시하기 
        for (int i = 0; i < _finalSelectCard.Count; i++)
        {
            CardTier _currTier = _finalSelectCard[i].Item1;
            SkillCard _currCard = _finalSelectCard[i].Item2;

            // tier에 따른 카드 이미지 바꾸기 
            _cardList[i].GetComponent<Image>().sprite
                = _cardTierSprite[(int)_finalSelectCard[i].Item1];

            Debug.Log(_finalSelectCard[i].Item1);

            // 카드이름
            _cardNameList[i].text = _currCard.cardName;

            // 카드 idx에 맞는 sprite
            _cardImageList[i].sprite = ResourceManager.instance.skillCardSprites[_currCard.cardIndex];

            // 카드 툴팁
            _cardToopTipList[i].text = _currCard.cardToolTip;
        }

    }

    // ui card idx 받아오기 (0부터 4까지)
    public void F_SetCardIndex(int v_idx) 
    {

        // 현재 인덱스 != 클릭인덱스 => card ui 효과주기 
        if(_currCardIndex != v_idx) 
        {
            // 선택된 카드 크기 키우기 
            _cardList[v_idx].GetComponent<RectTransform>().sizeDelta += new Vector2(20,20);

            // 현재 카드 크기 작게
            if(_currCardIndex != -1)
                _cardList[_currCardIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(350,600);

            // 현재 idx 저장 
            _currCardIndex = v_idx;

        }
        // 현재 인덱스 == 클릭인텍스 => 해당 skillcard의 효과 적용 
        else 
        {
            // 커졌던 카드 되돌려놓기
            _cardList[_currCardIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(350, 600);

            // ui on 
            _cardSelectUi.SetActive(false);

            // index에 맞는 skillcard 클래스를 return할 수 있음 
            SkillCardManager.instance.F_applyEffectBySkillcard(_finalSelectCard[_currCardIndex]);
        }
    }

}
