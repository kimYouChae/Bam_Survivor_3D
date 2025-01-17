using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSelectUI : MonoBehaviour
{
    [Header("===Animal Select===")]
    [SerializeField] List<GameObject> _animalProfileObj;
    [SerializeField] TextMeshProUGUI _selectAnimalNameText;

    [Header("===Arrow===")]
    [SerializeField] Image _leftArrow;
    [SerializeField] Image _rightArrow;

    [Header("===Ability===")]
    [SerializeField] TextMeshProUGUI _HpText;
    [SerializeField] TextMeshProUGUI _SpeedText;
    [SerializeField] TextMeshProUGUI _DamageText;
    [SerializeField] TextMeshProUGUI _DefenceText;
    [SerializeField] TextMeshProUGUI _RecoveryText;

    [Header("===Add Ability===")]
    [SerializeField] TextMeshProUGUI _moreAbilityText;

    [Header("===Animal Unlock===")]
    [SerializeField] List<PlayerAnimalState> _playerAnimalStateList;
    [SerializeField] Dictionary<AnimalType, bool> _obtainAnimal;

    [Header("===Price===")]
    [SerializeField] AnimalPriceData[] _animalPriceData;
    // [0]beaver... enum ������� ������

    [Header("===Now Animal State===")]
    [SerializeField] PlayerAnimalState _currMarkerState;

    private void Start()
    {
        // ��ųʸ� �ʱ�ȭ 
        F_InitDictionary();
    }

    public void F_InitDictionary() 
    {
        // 1. ������ �Ǻ��ϴ� ��ųʸ� �ʱ�ȭ
        _obtainAnimal = new Dictionary<AnimalType, bool>();
        _playerAnimalStateList = new List<PlayerAnimalState>();

        AnimalType[] _type = (AnimalType[])Enum.GetValues(typeof(AnimalType));

        // [0] ����� �⺻ ȹ������ 
        try
        {
            _obtainAnimal.Add(_type[0] , true );
           
        }
        catch (Exception e) 
        {
            Debug.Log(e);
        }

        // [1] ~ ������ ȹ�� 
        for(int i = 1; i < _type.Length; i++)
        {
            try
            {
                _obtainAnimal.Add(_type[i] , false);
            }
            catch (Exception e) 
            {
                Debug.Log(e);
            }
        }

        // 2. ����Ʈ �ʱ�ȭ
        for (int i = 0; i < _type.Length; i++) 
        {
            _playerAnimalStateList.Add
                        (PlayerManager_Lobby.Instance.F_AnimaTypeToState(_type[i]));
        }
    }


}
