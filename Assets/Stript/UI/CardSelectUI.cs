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
    private GameObject          _cardSelectUi;              // ī�� ����ui �θ� 
    [SerializeField]
    private int                 _currCardIndex;             // ���� ���� �� ī�� �ε���  

    [Header("===UI===")]
    [SerializeField]
    private Image[]             _cardBgList;                // BackGround
    [SerializeField]
    private Image[]             _cardTierBg;                // ī�� Ƽ�� bg
    [SerializeField]
    private TextMeshProUGUI[]        _cardTierText;              // ī�� Ƽ�� text
    [SerializeField]
    private Image[]        _cardIcon;                  // ī�� ������ 
    [SerializeField]
    private TextMeshProUGUI[]   _cardNameList;              // ī�� �̸� ����Ʈ
    [SerializeField]
    private Image[]             _cardImageList;             // ī�� �̹��� ����Ʈ
    [SerializeField]
    private GameObject[]         _cardStarParent;            // ī�� count�� Star�� �θ� 
    [SerializeField]
    private TextMeshProUGUI[]   _cardToopTipList;           // ī�� ���� ����Ʈ 
    [SerializeField]
    private GameObject[]        _cardPointSprite;           // ���� �� point ��������Ʈ 

    [Header("===Random Select Card===")]
    [SerializeField]
    private List<SkillCard> _finalSelectCard; // �������� ���� �� ī�� 

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

            F_UpdateCard(_finalSelectCard[i] , i );

            F_UpdateHighlightImage(i , false);

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
            F_UpdateHighlightImage(v_idx, true);

            // ������ �����ߴ� point�� off
            F_UpdateHighlightImage(_currCardIndex , false);

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
            F_UpdateHighlightImage(_currCardIndex, false);

            // �� ��ü off
            for (int i = 0; i < 5; i++)     // parent �� 5��
            {
                // ���� �ڽ� �� 5��
                for(int j = 0; j < 5; j++)  
                    _cardStarParent[i].transform.GetChild(j).transform.GetChild(0).gameObject.SetActive(false);
            }

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
        _cardTierBg[_i].sprite = ResourceManager.Instance.cardTierSprite(_tier);

        // tier �� �ؽ�Ʈ ���� 
        _cardTierText[_i].text = (_card.cardTier).ToString();

        // ability �� ������ ����
        _cardIcon[_i].sprite = ResourceManager.Instance.cardIconSprite(_ability);

        // card name ���� 
        _cardNameList[_i].text = _card.skillCardName;

        // card ��������Ʈ ���� 
        _cardImageList[_i].sprite = ResourceManager.Instance.F_NameToCardSprite(_card.classSpriteName);

        // count �� ���� 
        for (int i = 0; i < SkillCardManager.Instance.F_SkillAcquiCount(_card.skillCardName); i++) 
        {
            // �ڽ� (�����ִ� star)�� ON
            // �� Start�θ� (_index)
            //      �� Start �� ��� (i)
            //          �� Start Active (0)
            //      �� Start �� ��� 
            //          �� Start Active 
            _cardStarParent[_i].transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
        }

        // ���� ���� 
        _cardToopTipList[_i].text = _card.cardToolTip;
    }

    private void F_UpdateHighlightImage(int _idx, bool _flag) 
    {
        if (_idx < 0)
            return;

        _cardPointSprite[_idx].SetActive(_flag);
    }


}
