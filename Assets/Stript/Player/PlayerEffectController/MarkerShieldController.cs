using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Linq;

public class MarkerShieldController : MonoBehaviour
{

    [Header("===basic Shield Object===")]
    [SerializeField]
    private GameObject _basicShieldObject;                              // 기본 쉴드 오브젝트

    [Header("===Shield State===")]
    [SerializeField] private ShieldState _shieldState;

    [Header("===중복 검사 Dictionary===")]
    private Dictionary< Shield_Effect, int> DICT_ShieldTOCount;
    // Shield Enum (key)에 맞는 갯수 int (value) 

    [Header("===Skill Effect Ratio===")]
    [SerializeField] const  float   BLOOD_SHIPON_RATIO      = 0.7f;     // 피흡 비율 
    [SerializeField] const  float   BLOOD_EXCUTION_RATIO    = 5f;       // ##TODO 임시 : 적이 5f이하면 처형
    [SerializeField] const  int     BOMB_GENERATE_RATIO     = 2;        // 폭탄 생성 비율 

    public delegate void del_MarkerShield(Marker _unitTrs, float _size);

    // deligate 선언
    public del_MarkerShield del_markerShieldUse;

    private void Start()
    {
        // ##TODO : 임시 쉴드state 생성 , 나중에 쉴드 범위는 캐릭터 따라 달리지는 ?  
        _shieldState = new ShieldState(3f);

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

    // marker에서 실행
    public void F_StartShieldAction(Marker _marker) 
    {
        // 코루틴 멈춘 후 실행하면 마지막 marker만 코루틴 실행함  
        // StopAllCoroutines();
        StartCoroutine(IE_Test(_marker));
    }


    // ## Test : 쉴드 돌아가는 로직 
    private IEnumerator IE_Test( Marker _marker ) 
    {
        // ##TODO : 쉴드 pool에서 가져오기 
        GameObject _shieldIns               = Instantiate(_basicShieldObject, _marker.gameObject.transform);
        _shieldIns.transform.localPosition  = Vector3.zero;
        _shieldIns.transform.localScale     = new Vector3(0.1f, 0.1f, 0.1f);

        while (true) 
        {
            // 크기 키우기 
            _shieldIns.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
            _shieldIns.transform.position   = _marker.gameObject.transform.position;

            // 크기가 max가 되면 중지 
            if (_shieldIns.transform.localScale.x >= _shieldState.shieldSize
                && _shieldIns.transform.localScale.y >= _shieldState.shieldSize)
                break;


            // 일정시간 기다리기 
            yield return new WaitForSeconds(0.1f);
        }

        // 쉴드 델리게이트 (효과 적용) 실행
        try 
        {
            del_markerShieldUse(_marker , _shieldState.shieldSize );
        }
        catch (Exception ex) 
        {
            Debug.LogError(ex);
        }

        // 일정시간후에 삭제 
        yield return new WaitForSeconds(0.2f);

        // ##TODO : 쉴드 pool로 보내기 
        Destroy(_shieldIns);

        // 코루틴 종료
        yield break;
    }

    // 쉴드 카드 획득 시 실행
    public void F_ApplyShieldEffect( SkillCard v_card ) 
    {
        // 딕셔너리에 skillcard 검사 
        F_DictionaryInt(v_card);

        // 추가된 SkillCard에 맞게 델리게이트에 메서드 추가 
        // Blood Shipon : 초기 1회 획득
        if (DICT_ShieldTOCount[Shield_Effect.Epic_BloodSiphon] == 1) 
        {
            // Blood : 초기 1회만 델리게이트에 추가
            // 1~3회까지는 F_BloodShiponEffect()내에서 count로 데미지 추가
            // 4회에는 처형추가
            del_markerShieldUse += v_card.F_SkillcardEffect;
        }
        // Blood Shipon : 4회 획득
        if (DICT_ShieldTOCount[Shield_Effect.Epic_BloodSiphon] == 4) 
        {
            // TODO : blood 관련 코드 수정해야함 
            // 기존 삭제
            del_markerShieldUse -= F_BloodShiponEffect;
            // 처형효과 추가
            del_markerShieldUse += F_Test;
        }

    }

    // skill effect 중복 체크 
    public void F_DictionaryInt(SkillCard v_card)
    {
        // 초기화 안되어있으면 초기화
        if (DICT_ShieldTOCount == null)
            DICT_ShieldTOCount = new Dictionary<Shield_Effect, int>();

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
        { 
            Debug.LogError("Shield effect cannot be default");
            return;
        }

        // card가 포함이 안되어있으면 ?
        if (!DICT_ShieldTOCount.ContainsKey(_myEffect))
        {
            DICT_ShieldTOCount.Add(_myEffect, 0);
        }
        // 포함이 되어있으면 ?
        else
        {
            DICT_ShieldTOCount[_myEffect] += 1;
        }

    }

    // Shield Sate 업데이트 
    public void F_UpdateShieldState( float ShieldSizePercent = 0 ) 
    { 
        _shieldState.shieldSize += _shieldState.shieldSize * ShieldSizePercent;
    }

    // Epic_BloodShiphon Effect : 범위내 unit 적 흡혈
    public void F_BloodShiponEffect(Marker _marker , float _size) 
    {
        // Unit 검사 
        Collider[] _unitColliderList = Physics.OverlapSphere( _marker.gameObject.transform.position , _size , UnitManager.Instance.unitLayerInt);

        // 범위안에 유닛이 x 
        if (_unitColliderList.Length <= 0)
            return;

        // 범위안에 유닛이 감지되면
        foreach(Collider _coll in _unitColliderList) 
        {
            // 비울 * 획득 count 만큼 
            float _bloodAmount = BLOOD_SHIPON_RATIO * DICT_ShieldTOCount[Shield_Effect.Epic_BloodSiphon];
                
            // marker State hp 증가 
            PlayerManager.instance.F_UpdateIndiMarkerHP(_marker , _bloodAmount);

            // unit의 hp 감소 
            try 
            {
                _coll.GetComponent<Unit>().F_GetDamage(_bloodAmount);
            }
            catch (Exception e) 
            {
                Debug.LogError(e);
            }

        }
        
    }

    // ##TODO : 임시 Blood를 4개 이상 먹었을 때 적 처형 효과 추가 
    private void F_Test( Marker _marker , float _size ) 
    {
        // Unit 검사 
        Collider[] _unitColliderList = Physics.OverlapSphere(_marker.gameObject.transform.position, _size, UnitManager.Instance.unitLayerInt);

        // 범위안에 유닛이 x 
        if (_unitColliderList.Length <= 0)
            return;

        // 처형할 unit 추출 ( Linq :  적의 hp가 ratio 밑이면 temp에 저장 )
        var temp = from coll in _unitColliderList
                   where coll.GetComponent<Unit>() != null && coll.GetComponent<Unit>().unitHp <= BLOOD_EXCUTION_RATIO
                   select coll.GetComponent<Unit>();

        // 그 외 나머지 ?
        var temp2 = from coll in _unitColliderList
                    where coll.GetComponent<Unit>() != null && coll.GetComponent<Unit>().unitHp > BLOOD_EXCUTION_RATIO
                    select coll.GetComponent<Unit>();

        foreach (Unit unit in temp) 
        {
            // ##TODO : unit 의 적 처형
        }
        foreach (Unit unit in temp2) 
        {
            // ##TODO : 임시 코드 바꾸면 수정해야함 (데미지만 주기)
            unit.F_GetDamage(2f);
        }

    }

    public void F_SupernovaEffect( Marker _marker, float _size ) 
    {
    
    }
}
