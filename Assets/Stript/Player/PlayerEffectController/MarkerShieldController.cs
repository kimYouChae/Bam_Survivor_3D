using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditorInternal;


public class MarkerShieldController : MonoBehaviour
{

    [Header("===basic Shield Object===")]
    [SerializeField]
    private GameObject _basicShieldObject;          // 기본 쉴드 오브젝트

    [Header("===Shield State===")]
    [SerializeField] private ShieldState _shieldState;

    [Header("===중복 검사 Dictionary===")]
    private Dictionary< Shield_Effect, int> _markerShieldDuplication;
    // Shield Enum (key)에 맞는 갯수 int (value) 

    public delegate void del_MarkerShield(Marker _unitTrs, float _size);

    // deligate 선언
    public del_MarkerShield del_markerShieldUse;

    private void Start()
    {
        // ##TODO : 임시 쉴드state 생성 , 나중에 쉴드 범위는 캐릭터 따라 달리지는 ?  
        _shieldState = new ShieldState(3f);
    }

    // ## Test : 쉴드 돌아가는 로직 
    private IEnumerator IE_Test( GameObject v_marker ) 
    {
        GameObject _shieldIns = Instantiate(_basicShieldObject, v_marker.transform);
        _shieldIns.transform.localPosition = Vector3.zero;

        while (true) 
        {
            // 크기 키우기 
            _shieldIns.transform.localScale += new Vector3(0.2f, 0.2f, 0);

            // 크기가 max가 되면 중지 
            if (_shieldIns.transform.localScale.x >= _shieldState.shieldSize
                && _shieldIns.transform.localScale.y >= _shieldState.shieldSize)
                break;

            // 쉴드 델리게이트 (효과 적용) 실행


            // 일정시간 기다리기 
            yield return new WaitForSeconds(0.5f);
        }


        // 일정시간후에 삭제 
        yield return new WaitForSeconds(0.2f);
        Destroy(_shieldIns);
    }

    private void F_AddToEffectDeligate(del_MarkerShield _effect) 
    {
        del_markerShieldUse += _effect;
    }

    private void F_RemoveFromEffectDeligate(del_MarkerShield _effect) 
    {
        del_markerShieldUse -= _effect;
    }

    private void F_UseEffectDeligate(Marker _marker, float _shieldSize) 
    {
        del_markerShieldUse.Invoke(_marker, _shieldSize);
    }

    public void F_ApplyShieldEffect( SkillCard v_card ) 
    {
        // 딕셔너리에 skillcard 검사 
        F_DictionaryInt(v_card);

        // ##TODO : 추가된 효과에 맞게 추가 효과 넣어야함
        // ex) dictionary[ 쉴드 enum.흡혈] == 1 && dictionary[쉴드 enum.ex] == 1
        if (_markerShieldDuplication[Shield_Effect.Epic_BloodSiphon] == 1) 
        {
            // Blood : 초기 1회만 델리게이트에 추가
            // 1~3회까지는 F_BloodShiponEffect()내에서 count로 데미지 추가
            // 4회에는 처형추가
            del_markerShieldUse += v_card.F_SkillcardEffect;
        }


    }

    // skill effect 중복 체크 
    public void F_DictionaryInt(SkillCard v_card)
    {
        // 초기화 안되어있으면 초기화
        if (_markerShieldDuplication == null)
            _markerShieldDuplication = new Dictionary<Shield_Effect, int>();

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
        if (!_markerShieldDuplication.ContainsKey(_myEffect))
        {
            _markerShieldDuplication.Add(_myEffect, 0);
        }
        // 포함이 되어있으면 ?
        else
        {
            _markerShieldDuplication[_myEffect] += 1;
        }

    }

    // Shield Sate 업데이트 
    public void F_UpdateShieldState( float ShieldSizePercent = 0 ) 
    { 
        _shieldState.shieldSize += _shieldState.shieldSize * ShieldSizePercent;
    }

    // Epic_BloodShiphon Effect : 범위내 unit 적 흡혈
    public void F_BloodShiponEffect(Marker v_marker , float v_size) 
    {
        // ##TODO : 여러개 먹으면 적 unit 일정 피 이하 처형 + 피 흡혈 (이러면 적 검사할 때 linq 쓰기 좋을듯?)
        // ##TODO : 쉴드가 유지되는 시간도 생각 ?

        Collider[] _unitColliderList = Physics.OverlapSphere( v_marker.gameObject.transform.position , v_size , UnitManager.Instance.unitLayerInt);

        // 범위안에 유닛이 x 
        if (_unitColliderList.Length < 0)
            return;

        // 범위안에 유닛이 감지되면
        else 
        { 
            foreach(Collider _coll in _unitColliderList) 
            {
                // marker State hp 증가 
                

                // Shield_Effect.Epic_BloodSiphon의 count만큼 흡혈 / 피해량 증가 


            }
        }
    }

}
