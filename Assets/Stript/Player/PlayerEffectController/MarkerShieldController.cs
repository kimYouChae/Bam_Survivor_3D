using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MarkerShieldController : MonoBehaviour
{

    
    [Header("===basic Shield Object===")]
    [SerializeField]
    private GameObject _basicShieldObject;                              // 기본 쉴드 오브젝트
    

    [Header("===Contaier===")]
    private List<GameObject>        _instnaceToShieldObject;            // 쉴드 생성 시점에 생성될 오브젝트 컨테이너  
    private List<Shield_Effect>     _instanceShieldEffect;              // 생성한 쉴드 effect 컨테이너                               

    [Header("===Shield State===")]
    [SerializeField] private ShieldState _shieldState;

    [Header("===중복 검사 Dictionary===")]
    private Dictionary< Shield_Effect, int > DICT_ShieldTOCount;
    // Shield Enum (key)에 맞는 갯수 int (value) 

    [Header("===Skill Effect Ratio===")]
    [SerializeField] const  float   BLOOD_SHIPON_RATIO      = 0.7f;     // 피흡 비율 
    [SerializeField] const  float   BLOOD_EXCUTION_RATIO    = 5f;       // ##TODO 임시 : 적이 N이하면 처형
    [SerializeField] const  int     BOMB_GENERATE_RATIO     = 2;        // 폭탄 생성 비율 

    public delegate void del_ShieldCreate       (Marker _unitTrs);             

    // deligate 선언
    public del_ShieldCreate del_shieldCreate;

    private void Start()
    {
        // ##TODO : 임시 쉴드state 생성 , 나중에 쉴드 범위는 캐릭터 따라 달리지는 ?  
        _shieldState                = new ShieldState(3f);

        _instnaceToShieldObject     = new List<GameObject>();
        _instanceShieldEffect       = new List<Shield_Effect>();

        // 딕셔너리 초기화
        F_InitDictionary();

    }

    private void F_InitDictionary() 
    {
        DICT_ShieldTOCount = new Dictionary<Shield_Effect, int>();

        Shield_Effect[] _effect = (Shield_Effect[])System.Enum.GetValues(typeof(Shield_Effect));

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
    public void F_ApplyShieldEffect( SkillCard v_card ) 
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
            Enum.TryParse(v_card.classSpriteName , out _myEffect);
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
        GameObject _obj = ShieldPooling.instance.F_ShieldGet(_effect);
        _obj.transform.position = _marker.transform.position;   
    }

    // 쉴드 사이즈 수정 
    internal void F_UpdateShieldState(float ShieldSizePercent = 0f)
    {
        
    }
}
