using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

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
    private Image[]             _cardTierBg;                // 카드 티어 bg
    [SerializeField]
    private TextMeshProUGUI[]        _cardTierText;              // 카드 티어 text
    [SerializeField]
    private Image[]        _cardIcon;                  // 카드 아이콘 
    [SerializeField]
    private TextMeshProUGUI[]   _cardNameList;              // 카드 이름 리스트
    [SerializeField]
    private Image[]             _cardImageList;             // 카드 이미지 리스트
    [SerializeField]
    private GameObject[]         _cardStarParent;            // 카드 count별 Star의 부모 
    [SerializeField]
    private TextMeshProUGUI[]   _cardToopTipList;           // 카드 툴팁 리스트 
    [SerializeField]
    private GameObject[]        _cardPointSprite;           // 선택 시 point 스프라이트 

    [Header("===Random Select Card===")]
    [SerializeField]
    private List<SkillCard> _finalSelectCard; // 랜덤으로 선택 된 카드 

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

            F_UpdateCard(_finalSelectCard[i] , i );

            F_UpdateHighlightImage(i , false);

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
            F_UpdateHighlightImage(v_idx, true);

            // 이전에 선택했던 point는 off
            F_UpdateHighlightImage(_currCardIndex , false);

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
            F_UpdateHighlightImage(_currCardIndex, false);

            // 별 전체 off
            for (int i = 0; i < 5; i++)     // parent 별 5개
            {
                // 하위 자식 별 5개
                for(int j = 0; j < 5; j++)  
                    _cardStarParent[i].transform.GetChild(j).transform.GetChild(0).gameObject.SetActive(false);
            }

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
        _cardTierBg[_i].sprite = ResourceManager.Instance.cardTierSprite(_tier);

        // tier 별 텍스트 변경 
        _cardTierText[_i].text = (_card.cardTier).ToString();

        // ability 별 아이콘 변경
        _cardIcon[_i].sprite = ResourceManager.Instance.cardIconSprite(_ability);

        // card name 변경 
        _cardNameList[_i].text = _card.skillCardName;

        // card 스프라이트 변경 
        _cardImageList[_i].sprite = ResourceManager.Instance.F_NameToCardSprite(_card.classSpriteName);

        // count 별 갯수 
        for (int i = 0; i < SkillCardManager.Instance.F_SkillAcquiCount(_card.skillCardName); i++) 
        {
            // 자식 (켜저있는 star)을 ON
            // ㄴ Start부모 (_index)
            //      ㄴ Start 빈 배경 (i)
            //          ㄴ Start Active (0)
            //      ㄴ Start 빈 배경 
            //          ㄴ Start Active 
            _cardStarParent[_i].transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
        }

        // 툴팁 변경 
        _cardToopTipList[_i].text = _card.cardToolTip;
    }

    private void F_UpdateHighlightImage(int _idx, bool _flag) 
    {
        if (_idx < 0)
            return;

        _cardPointSprite[_idx].SetActive(_flag);
    }


}
