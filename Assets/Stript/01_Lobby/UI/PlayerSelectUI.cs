using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectUI : MonoBehaviour
{
    [Header("===Animal Select===")]
    [SerializeField] List<GameObject> _animalProfileObj;            // 프로필 리스트
    [SerializeField] TextMeshProUGUI _selectAnimalNameText;         // 선택한 동물 이름

    [Header("===Arrow===")]
    [SerializeField] GameObject _leftArrow;
    [SerializeField] GameObject _rightArrow;

    [Header("===Button===")]
    [SerializeField] GameObject _acquireImage;              // 획득 이미지 
    [SerializeField] GameObject _buyButton;                 // buy 버튼
    [SerializeField] GameObject _selectButton;              // 선택버튼
    [SerializeField] TextMeshProUGUI _priceText;            // 가격 텍스트

    [Header("===Ability===")]
    [SerializeField] TextMeshProUGUI _HpText;
    [SerializeField] TextMeshProUGUI _SpeedText;
    [SerializeField] TextMeshProUGUI _DamageText;
    [SerializeField] TextMeshProUGUI _DefenceText;
    [SerializeField] TextMeshProUGUI _RecoveryText;
    [SerializeField] TextMeshProUGUI _moreAbilityText;

    [Header("===Animal Unlock===")]
    [SerializeField] List<PlayerAnimalState> _playerAnimalStateList;        // enum 순서대로 리스트에서 저장 
    [SerializeField] Dictionary<AnimalType, bool> DICT_obtainAnimal;        // type별 획득 여부
    [SerializeField] AnimalType[] _animalTypeList;

    [Header("===Price===")]
    [SerializeField] AnimalPriceData[] _animalPriceData;
    // [0]beaver... enum 순서대로 들어가있음

    [Header("===Now Animal State===")]
    [SerializeField] PlayerAnimalState _currMarkerState;                    // 현재 player 클래스 
    [SerializeField] int _currIndex = 0;                                    // 현재 index

    [Header("===Script===")]
    [SerializeField] private AnimalsOnOff _animalsOnOff;

    //[Header("===Delegate===")]
    [SerializeField] private delegate void Del_UiUpdate(int idx);
    private Del_UiUpdate del_UiUpdate;

    private void Awake()
    {
        // 최초 1회 , OnEnable보다 일찍 시작 해야한다 
        // AnimalType enum을 리스트로 저장 
        _animalTypeList = (AnimalType[])Enum.GetValues(typeof(AnimalType));

        // 딕셔너리 초기화 
        F_InitDictionary();

        // 델리게이션이 함수추가 .
        del_UiUpdate += F_ChangeNameText;
        del_UiUpdate += F_ChangeButton;
        del_UiUpdate += F_UpdateAnimalState;
        del_UiUpdate += F_UpdatePrice;
        del_UiUpdate += _animalsOnOff.F_OnOFfAnimalsByIndex;

    }

    private void Start()
    {
        _currIndex = 0;

        // 버튼에 이벤트 초기화
        _leftArrow.GetComponent<Button>().onClick.AddListener(F_ClickLeftArrow);
        _rightArrow.GetComponent<Button>().onClick.AddListener(F_ClickRightArrow); ;

        // buy 버튼 조기화 
        _buyButton.GetComponent<Button>().onClick.AddListener(F_BuyAnimal);
        // select 버튼 초기화
        _selectButton.GetComponent<Button>().onClick.AddListener(F_SelectAnimal);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            DICT_obtainAnimal[AnimalType.SnappingTurtle] = true;
            F_InitPlayerSelectUI(); 

        }
    }

    private void OnEnable()
    {
        // Ui 초기화 
        F_InitPlayerSelectUI();
    }

    public void F_InitDictionary() 
    {
        // 1. 얻은지 판별하는 딕셔너리 초기화
        DICT_obtainAnimal = new Dictionary<AnimalType, bool>();
        _playerAnimalStateList = new List<PlayerAnimalState>();

        // [0] 비버는 기본 획득으로 ( true )
        try
        {
            DICT_obtainAnimal.Add(_animalTypeList[0] , true );
        }
        catch (Exception e) 
        {
            Debug.Log(e);
        }

        // [1] ~ 나머지 획득 ( false )
        for(int i = 1; i < _animalTypeList.Length; i++)
        {
            try
            {
                DICT_obtainAnimal.Add(_animalTypeList[i] , false);
            }
            catch (Exception e) 
            {
                Debug.Log(e);
            }
        }

        // 2. 리스트 초기화
        for (int i = 0; i < _animalTypeList.Length; i++) 
        {
            _playerAnimalStateList.Add
                        (PlayerManager_Lobby.Instance.F_AnimaTypeToState(_animalTypeList[i]));
        }
    }

    // 이 ui 들어올 때마다 맨 처음 화면 (비버는 잠금해제된 상태) 초기화 
    // 다른 UI Panel에서 플레이어를 살 수 도 있으니까 
    private void F_InitPlayerSelectUI() 
    {
        // panel 돌면서 잠금 표시 onoff
        for (int i = 0; i < _animalTypeList.Length; i++) 
        {
            // 획득이 true이면  
            if (DICT_obtainAnimal[_animalTypeList[i]]) 
            {
                // 자물쇠 끄기 
                F_LockerOff(i);
                
            }
        }

        // ui 업데이트 
        del_UiUpdate(_currIndex);
    }

    private void F_ClickRightArrow() 
    {
        _currIndex++;
        if (_currIndex >= _playerAnimalStateList.Count - 1) 
        {
            // max값으로 
            _currIndex = _playerAnimalStateList.Count - 1;

            // arrow 숨기기 
            _rightArrow.SetActive(false);
        }
        else
        {
            _rightArrow.SetActive(true);
            _leftArrow.SetActive(true);
        }

        // ui 업데이트 
        del_UiUpdate(_currIndex);

    }
    private void F_ClickLeftArrow() 
    {
        _currIndex--;
        if (_currIndex <= 0 ) 
        {
            // min 값으로 
            _currIndex = 0;

            // arrow 숨기기
            _leftArrow.SetActive(false);
        }
        else
        {
            _rightArrow.SetActive(true);
            _leftArrow.SetActive(true);
        }

        // ui 업데이트 
        del_UiUpdate(_currIndex);
    }

    // arrow 누를때 
    // 1. 이름변경 
    private void F_ChangeNameText(int _idx) 
    {
        _selectAnimalNameText.text = _playerAnimalStateList[_idx].markerName;
    }

    // 2. buy, select 버튼 on off 
    private void F_ChangeButton(int _idx) 
    {
        // 획득했는지 아닌지 검사해야함 
        bool flag = DICT_obtainAnimal[(AnimalType)_idx];

        // 획득 이미지 on
        _acquireImage.SetActive(flag);

        // buy 버튼 off
        _buyButton.SetActive(!flag);
    }

    // 3. 오른쪽 스탯 변경 
    private void F_UpdateAnimalState(int _idx) 
    {
        PlayerAnimalState _state = _playerAnimalStateList[_idx];

        // 기본 state 변경 
        _HpText.text        = _state.markerHp.ToString();
        _SpeedText.text     = _state.markerMoveSpeed.ToString();
        _DamageText.text    = _state.markerDamage.ToString();
        _DefenceText.text   = _state.markerDefence.ToString();
        _RecoveryText.text  = _state.markerNaturalRecoery.ToString();

        // 추가 state 변경 
        _moreAbilityText.text = "탐색범위 : " + _state.magnetSearchRadious + "   "
                                + "경험치 배율 : " + _state.markerLuck + '\n'
                                + "쉴드 쿨타임 : " + _state.markerShieldCoolTime + " "
                                + "운 : " + _state.markerLuck;

    }

    // 4. 가격 업데이트 
    private void F_UpdatePrice(int _idx) 
    {
        _priceText.text = _animalPriceData[_idx].AnimalPrice.ToString();
    }
    
    // 동물 구매
    public void F_BuyAnimal() 
    {
        // 골드가 충분히 없으면 return
        if (!GoodsManager.Instance.F_HaveEnoughMoney(GoodsType.Gold, _animalPriceData[_currIndex].AnimalPrice))
            return;

        // 골드가 있으면 1. 골드 사용
        GoodsManager.Instance.F_UpdateGoods(GoodsType.Gold , -1 * (_animalPriceData[_currIndex].AnimalPrice));

        // 2. 획득
        F_GainAnimal(_animalTypeList[_currIndex]);
        // 2-1. 자물쇠 업데이트
        F_LockerOff(_currIndex);

        // 3. Goods ui 업데이트 
        GoodsManager.Instance.goodsUi.F_UpdateGoodsText(GoodsType.Gold);
    }

    // 동물 획득 // ##TODO : 상점 페이지에서 사용할수도 
    public void F_GainAnimal(AnimalType _type) 
    {
        DICT_obtainAnimal[_type] = true;
    }

    // 자물쇠 off
    private void F_LockerOff(int _idx) 
    {
        _animalProfileObj[_idx].transform.GetChild(2).gameObject.SetActive(false);
    }

    // 동물 선택 
    private void F_SelectAnimal() 
    {
        // ##TODO : 게임씬으로 넘겨야함 _currIndex에 맞는 PlayerAnimalState
    }
}
