using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectUI : MonoBehaviour
{
    [Header("===Animal Select===")]
    [SerializeField] List<GameObject> _animalProfileObj;            // ������ ����Ʈ
    [SerializeField] TextMeshProUGUI _selectAnimalNameText;         // ������ ���� �̸�

    [Header("===Arrow===")]
    [SerializeField] GameObject _leftArrow;
    [SerializeField] GameObject _rightArrow;

    [Header("===Button===")]
    [SerializeField] GameObject _acquireImage;              // ȹ�� �̹��� 
    [SerializeField] GameObject _buyButton;                 // buy ��ư
    [SerializeField] GameObject _selectButton;              // ���ù�ư
    [SerializeField] TextMeshProUGUI _priceText;            // ���� �ؽ�Ʈ

    [Header("===Ability===")]
    [SerializeField] TextMeshProUGUI _HpText;
    [SerializeField] TextMeshProUGUI _SpeedText;
    [SerializeField] TextMeshProUGUI _DamageText;
    [SerializeField] TextMeshProUGUI _DefenceText;
    [SerializeField] TextMeshProUGUI _RecoveryText;
    [SerializeField] TextMeshProUGUI _moreAbilityText;

    [Header("===Animal Unlock===")]
    [SerializeField] List<PlayerAnimalState> _playerAnimalStateList;        // enum ������� ����Ʈ���� ���� 
    [SerializeField] Dictionary<AnimalType, bool> DICT_obtainAnimal;        // type�� ȹ�� ����
    [SerializeField] AnimalType[] _animalTypeList;

    [Header("===Price===")]
    [SerializeField] AnimalPriceData[] _animalPriceData;
    // [0]beaver... enum ������� ������

    [Header("===Now Animal State===")]
    [SerializeField] PlayerAnimalState _currMarkerState;                    // ���� player Ŭ���� 
    [SerializeField] int _currIndex = 0;                                    // ���� index

    [Header("===Script===")]
    [SerializeField] private AnimalsOnOff _animalsOnOff;

    //[Header("===Delegate===")]
    [SerializeField] private delegate void Del_UiUpdate(int idx);
    private Del_UiUpdate del_UiUpdate;

    private void Awake()
    {
        // ���� 1ȸ , OnEnable���� ���� ���� �ؾ��Ѵ� 
        // AnimalType enum�� ����Ʈ�� ���� 
        _animalTypeList = (AnimalType[])Enum.GetValues(typeof(AnimalType));

        // ��ųʸ� �ʱ�ȭ 
        F_InitDictionary();

        // �������̼��� �Լ��߰� .
        del_UiUpdate += F_ChangeNameText;
        del_UiUpdate += F_ChangeButton;
        del_UiUpdate += F_UpdateAnimalState;
        del_UiUpdate += F_UpdatePrice;
        del_UiUpdate += _animalsOnOff.F_OnOFfAnimalsByIndex;

    }

    private void Start()
    {
        _currIndex = 0;

        // ��ư�� �̺�Ʈ �ʱ�ȭ
        _leftArrow.GetComponent<Button>().onClick.AddListener(F_ClickLeftArrow);
        _rightArrow.GetComponent<Button>().onClick.AddListener(F_ClickRightArrow); ;

        // buy ��ư ����ȭ 
        _buyButton.GetComponent<Button>().onClick.AddListener(F_BuyAnimal);
        // select ��ư �ʱ�ȭ
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
        // Ui �ʱ�ȭ 
        F_InitPlayerSelectUI();
    }

    public void F_InitDictionary() 
    {
        // 1. ������ �Ǻ��ϴ� ��ųʸ� �ʱ�ȭ
        DICT_obtainAnimal = new Dictionary<AnimalType, bool>();
        _playerAnimalStateList = new List<PlayerAnimalState>();

        // [0] ����� �⺻ ȹ������ ( true )
        try
        {
            DICT_obtainAnimal.Add(_animalTypeList[0] , true );
        }
        catch (Exception e) 
        {
            Debug.Log(e);
        }

        // [1] ~ ������ ȹ�� ( false )
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

        // 2. ����Ʈ �ʱ�ȭ
        for (int i = 0; i < _animalTypeList.Length; i++) 
        {
            _playerAnimalStateList.Add
                        (PlayerManager_Lobby.Instance.F_AnimaTypeToState(_animalTypeList[i]));
        }
    }

    // �� ui ���� ������ �� ó�� ȭ�� (����� ��������� ����) �ʱ�ȭ 
    // �ٸ� UI Panel���� �÷��̾ �� �� �� �����ϱ� 
    private void F_InitPlayerSelectUI() 
    {
        // panel ���鼭 ��� ǥ�� onoff
        for (int i = 0; i < _animalTypeList.Length; i++) 
        {
            // ȹ���� true�̸�  
            if (DICT_obtainAnimal[_animalTypeList[i]]) 
            {
                // �ڹ��� ���� 
                F_LockerOff(i);
                
            }
        }

        // ui ������Ʈ 
        del_UiUpdate(_currIndex);
    }

    private void F_ClickRightArrow() 
    {
        _currIndex++;
        if (_currIndex >= _playerAnimalStateList.Count - 1) 
        {
            // max������ 
            _currIndex = _playerAnimalStateList.Count - 1;

            // arrow ����� 
            _rightArrow.SetActive(false);
        }
        else
        {
            _rightArrow.SetActive(true);
            _leftArrow.SetActive(true);
        }

        // ui ������Ʈ 
        del_UiUpdate(_currIndex);

    }
    private void F_ClickLeftArrow() 
    {
        _currIndex--;
        if (_currIndex <= 0 ) 
        {
            // min ������ 
            _currIndex = 0;

            // arrow �����
            _leftArrow.SetActive(false);
        }
        else
        {
            _rightArrow.SetActive(true);
            _leftArrow.SetActive(true);
        }

        // ui ������Ʈ 
        del_UiUpdate(_currIndex);
    }

    // arrow ������ 
    // 1. �̸����� 
    private void F_ChangeNameText(int _idx) 
    {
        _selectAnimalNameText.text = _playerAnimalStateList[_idx].markerName;
    }

    // 2. buy, select ��ư on off 
    private void F_ChangeButton(int _idx) 
    {
        // ȹ���ߴ��� �ƴ��� �˻��ؾ��� 
        bool flag = DICT_obtainAnimal[(AnimalType)_idx];

        // ȹ�� �̹��� on
        _acquireImage.SetActive(flag);

        // buy ��ư off
        _buyButton.SetActive(!flag);
    }

    // 3. ������ ���� ���� 
    private void F_UpdateAnimalState(int _idx) 
    {
        PlayerAnimalState _state = _playerAnimalStateList[_idx];

        // �⺻ state ���� 
        _HpText.text        = _state.markerHp.ToString();
        _SpeedText.text     = _state.markerMoveSpeed.ToString();
        _DamageText.text    = _state.markerDamage.ToString();
        _DefenceText.text   = _state.markerDefence.ToString();
        _RecoveryText.text  = _state.markerNaturalRecoery.ToString();

        // �߰� state ���� 
        _moreAbilityText.text = "Ž������ : " + _state.magnetSearchRadious + "   "
                                + "����ġ ���� : " + _state.markerLuck + '\n'
                                + "���� ��Ÿ�� : " + _state.markerShieldCoolTime + " "
                                + "�� : " + _state.markerLuck;

    }

    // 4. ���� ������Ʈ 
    private void F_UpdatePrice(int _idx) 
    {
        _priceText.text = _animalPriceData[_idx].AnimalPrice.ToString();
    }
    
    // ���� ����
    public void F_BuyAnimal() 
    {
        // ��尡 ����� ������ return
        if (!GoodsManager.Instance.F_HaveEnoughMoney(GoodsType.Gold, _animalPriceData[_currIndex].AnimalPrice))
            return;

        // ��尡 ������ 1. ��� ���
        GoodsManager.Instance.F_UpdateGoods(GoodsType.Gold , -1 * (_animalPriceData[_currIndex].AnimalPrice));

        // 2. ȹ��
        F_GainAnimal(_animalTypeList[_currIndex]);
        // 2-1. �ڹ��� ������Ʈ
        F_LockerOff(_currIndex);

        // 3. Goods ui ������Ʈ 
        GoodsManager.Instance.goodsUi.F_UpdateGoodsText(GoodsType.Gold);
    }

    // ���� ȹ�� // ##TODO : ���� ���������� ����Ҽ��� 
    public void F_GainAnimal(AnimalType _type) 
    {
        DICT_obtainAnimal[_type] = true;
    }

    // �ڹ��� off
    private void F_LockerOff(int _idx) 
    {
        _animalProfileObj[_idx].transform.GetChild(2).gameObject.SetActive(false);
    }

    // ���� ���� 
    private void F_SelectAnimal() 
    {
        // ##TODO : ���Ӿ����� �Ѱܾ��� _currIndex�� �´� PlayerAnimalState
    }
}
