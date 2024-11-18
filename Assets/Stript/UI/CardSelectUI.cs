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
    private GameObject          _cardSelectUi;              // 카드 선택ui 부모 
    [SerializeField]
    private int                 _currCardIndex;             // 현재 선택 된 카드 인덱스  

    [Header("===UI===")]
    [SerializeField]
    private Image[]             _cardBgList;                // BackGround
    [SerializeField]
    private GameObject[]        _cardTierBg;                // 카드 티어 bg
    [SerializeField]
    private GameObject[]        _cardTierText;              // 카드 티어 text
    [SerializeField]
    private GameObject[]        _cardIcon;                  // 카드 아이콘 
    [SerializeField]
    private TextMeshProUGUI[]   _cardNameList;              // 카드 이름 리스트
    [SerializeField]
    private Image[]             _cardImageList;             // 카드 이미지 리스트
    [SerializeField]
    private GameObject[]        _cardStartCount;            // 카드 count별 Star
    [SerializeField]
    private TextMeshProUGUI[]   _cardToopTipList;           // 카드 툴팁 리스트 
    [SerializeField]
    private GameObject[]        _cardPointSprite;           // 선택 시 point 스프라이트 

    [Header("===Random Select Card===")]
    [SerializeField]
    private List<Tuple<CardTier, SkillCard>> _finalSelectCard; // 랜덤으로 선택 된 카드 

    // 카드 ui On  
    public void F_ShowCard()
    {
        // idx 초기화
        _currCardIndex = -1;

        // ui on 
        _cardSelectUi.SetActive(true);

        // 최종 선택된 card List 받아오기 
        _finalSelectCard = SkillCardManager.Instance.F_FinalSelectCard();

        // 카드 표시하기 
        for (int i = 0; i < _finalSelectCard.Count; i++)
        {
            // ##TODO : finalSelectCard는 Tuple일 필요가 없는듯 ?
            // skillcard안에 tier도 있자나

            //F_UpdateCard();

            /*
            CardTier _currTier = _finalSelectCard[i].Item1;
            SkillCard _currCard = _finalSelectCard[i].Item2;

            // tier에 따른 카드 이미지 바꾸기 
            _cardBgList[i].sprite
                = _cardTierSprite[(int)_finalSelectCard[i].Item1];

            Debug.Log(_finalSelectCard[i].Item1);

            // 카드이름
            _cardNameList[i].text = _currCard.skillCardName;

            // 카드 name에 맞는 sprite
            _cardImageList[i].sprite = ResourceManager.Instance.F_ReturnSkillNameToSprite(_currCard.classSpriteName);

            // 카드 툴팁
            _cardToopTipList[i].text = _currCard.cardToolTip;
            */
        }

    }

    // ui card idx 받아오기 (0부터 4까지)
    public void F_SetCardIndex(int v_idx) 
    {

        // 현재 인덱스 != 클릭인덱스 => card ui 효과주기 
        if(_currCardIndex != v_idx) 
        {
            // 선택된 카드 크기 키우기 
            _cardBgList[v_idx].GetComponent<RectTransform>().sizeDelta += new Vector2(20,20);

            // 현재 카드 크기 작게
            if(_currCardIndex != -1)
                _cardBgList[_currCardIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(350,600);

            // card point 스프라이트 on 
            _cardPointSprite[v_idx].gameObject.SetActive(true);

            // 현재 idx 저장 
            _currCardIndex = v_idx;

        }
        // 현재 인덱스 == 클릭인텍스 => 해당 skillcard의 효과 적용 
        else 
        {
            // 커졌던 카드 되돌려놓기
            _cardBgList[_currCardIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(350, 600);

            // ui off
            _cardSelectUi.SetActive(false);

            // card point 스프라이트 off
            _cardPointSprite[v_idx].gameObject.SetActive(true);

            // index에 맞는 skillcard 클래스를 return할 수 있음 
            SkillCardManager.Instance.F_applyEffectBySkillcard(_finalSelectCard[_currCardIndex]);

            // pause 되어있다면 시간 돌아가게
            if( Time.timeScale == 0 )
                Time.timeScale = 1;
        }
    }

    private void F_UpdateCard(SkillCard _card, int _i) 
    {
        CardTier _tier          = _card.cardTier;
        CardAbility _ability    = _card.cardAbility;

        // tier 별 카드 bg 변경 
        _cardBgList[_i].sprite = ResourceManager.Instance.cardBackGround(_tier);

        // tier 별 tier 이미지 변경 

        //##TODO : 수정해야함 ! 
        
    }

}
