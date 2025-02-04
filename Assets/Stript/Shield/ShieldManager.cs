using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : Singleton<ShieldManager>
{
    [Header("===Script===")]
    [SerializeField] private ShieldPooling _shieldPooling;
    [SerializeField] private ShieldCSVImporter _shieldCSVImporter;

    [Header("===중복 검사 Dictionary===")]
    private Dictionary<Shield_Effect, int> DICT_ShieldTOCount;
    // Shield Enum (key)에 맞는 갯수 int (value) 

    [Header("===Skill Effect Ratio===")]
    private const float BLOOD_SHIPON_RATIO = 0.7f;
    private const float BLOOD_EXCUTION_LIMIT = 15f;
    private const int BLOOD_EXUTION_CNT = 4;
    private const float SUPERNOVA_DAMAGE = 10f;

    public delegate void del_ShieldCreate(Marker _unitTrs);

    // deligate 선언
    public del_ShieldCreate del_shieldCreate;

    // 프로퍼티
    public ShieldCSVImporter shieldCSVImporter => _shieldCSVImporter;

    // effect에 해당하는 count를 return
    public int F_ReturnCountToDic(Shield_Effect _effect)
    {
        if (!DICT_ShieldTOCount.ContainsKey(_effect))
            return 0;

        return DICT_ShieldTOCount[_effect];
    }

    public bool F_IsBloodExution()
    {
        // count가 넘으면 true, 아니면 false
        return DICT_ShieldTOCount[Shield_Effect.Epic_BloodSiphon] >= BLOOD_EXUTION_CNT;
    }

    public float supernovaDamage => SUPERNOVA_DAMAGE;
    public float bloodShiponRatio => BLOOD_SHIPON_RATIO;
    public int bloodExcutionCnt => BLOOD_EXUTION_CNT;
    public float bloodExcutionLimit => BLOOD_EXCUTION_LIMIT;
    public ShieldPooling shieldPooling => _shieldPooling;

    protected override void Singleton_Awake()
    {

    }

    private void Start()
    {
        // effect To Count 딕셔너리 초기화
        F_InitShieldEffectToCountDIC();

        // 델리게이트에 기본 쉴드 use 추가
        del_shieldCreate += F_BasicShieldUse;

    }


    private void F_InitShieldEffectToCountDIC()
    {
        // Effect To Count 딕셔너리 초기화
        DICT_ShieldTOCount = new Dictionary<Shield_Effect, int>();

        Shield_Effect[] _effect = (Shield_Effect[])System.Enum.GetValues(typeof(Shield_Effect));

        // EffEct To Count 딕셔너리 값 넣기 
        for (int i = 0; i < _effect.Length; i++)
        {
            if (_effect[i] == Shield_Effect.Default)
                continue;

            // key를 가지고 있지 않으면 
            if (!DICT_ShieldTOCount.ContainsKey(_effect[i]))
                DICT_ShieldTOCount.Add(_effect[i], 0);
        }

    }


    // 쉴드 카드 획득 시 실행
    public void F_ApplyShieldEffect(SkillCard v_card)
    {
        // 딕셔너리에 skillcard 검사 
        Shield_Effect _effect = F_cardEqualShieldEffect(v_card);

        // default면 return
        if (_effect == Shield_Effect.Default)
            return;

        // 딕셔너리 + 1
        DICT_ShieldTOCount[_effect] += 1;

        // 처음 획득시 delegate에 추가
        if (DICT_ShieldTOCount[_effect] == 1)
        {
            del_shieldCreate += v_card.F_SkillcardEffect;
        }

    }

    // skill effect 중복 체크 
    internal Shield_Effect F_cardEqualShieldEffect(SkillCard v_card)
    {
        // v_card의 _className변수와 같은 enum을 찾기 
        Shield_Effect _myEffect = default;

        try
        {
            // _myEffect에 _className과 같은 enum이 담김
            Enum.TryParse(v_card.classSpriteName, out _myEffect);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }

        // 만약 설정이 안되고 default로 남아있으면 ? -> return 
        if (_myEffect == default)
            return Shield_Effect.Default;

        return _myEffect;

    }

    // effect에 따라 pool에서 가져오기
    internal void F_GetBloodShieldToPool(Shield_Effect _effect, Marker _marker)
    {
        // 쉴드 오브젝트 풀링에서 가져오기
        GameObject _obj = shieldPooling.F_ShieldGet(_effect);

        // 위치설정 
        //_obj.transform.position = _marker.transform.position;

        // marker 스크립트에 marker 넣어주기
        try
        {
            _obj.GetComponent<ShieldObject>().parentMarker = _marker;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    // 쉴드 사이즈 수정 
    internal void F_UpdateShieldState(float ShieldSizePercent = 0f)
    {

    }

    internal void F_BasicShieldUse(Marker _marker)
    {
        Debug.Log("BasocShieldUse 함수 실행");
        F_GetBloodShieldToPool(Shield_Effect.Default, _marker);
    }


}
