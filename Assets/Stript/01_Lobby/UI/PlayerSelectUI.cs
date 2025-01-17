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

    private void Start()
    {
        
    }

    public void F_InitAnimalProfile() 
    {
        // dicrionary에 있는 markerState를 가져와서 값 넣어야함

    }
}
