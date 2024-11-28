using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerExplosionConteroller : MonoBehaviour
{
    [Header("===Radious===")]
    [SerializeField] ExplosionState _explosionState;

    [Header("===중복 검사 Dictionary===")]
    private Dictionary<Explosion_Effect, int> DICT_ExplotionToCount; 
    // skillcard(key)에 맞는 갯수 int (value) 

    public delegate void del_BulletExplosion( Transform _bulletTrs, float _size);

    // deligate 선언
    public del_BulletExplosion del_bulletExplosion;

    [Header("===Skill Effect Ratio===")]
    [SerializeField] const int REIN_POISION_COUNT   = 4;
    [SerializeField] const int REIN_ICE_COUNT       = 0;

    private void Start()
    {
        // ##TODO : 임시 (3f) : explosion State 초기화
        _explosionState = new ExplosionState(1f);

        // 초기 1회 Dic 초기화
        F_InitDictionary();

        // 델리게이트에 기본 
        del_bulletExplosion += F_BasicExplosionUse;    
    }

    private void F_InitDictionary() 
    {
        DICT_ExplotionToCount = new Dictionary<Explosion_Effect, int>();

        Explosion_Effect[] _effect = (Explosion_Effect[])System.Enum.GetValues(typeof(Explosion_Effect));

        for (int i = 0; i < _effect.Length; i++) 
        {
            if (_effect[i] == Explosion_Effect.Default)
                continue;

            // key를 가지고 있지 않으면 
            if (!DICT_ExplotionToCount.ContainsKey(_effect[i]))
                DICT_ExplotionToCount.Add(_effect[i], 0);
        }
    }

    // 충돌 시 시작
    public void F_BulletExplosionStart(Transform _bulletTrs, float _size = 0) 
    {
        // 델리게이트 실행
        _size = _explosionState.explosionRadious;
        del_bulletExplosion(_bulletTrs , _size );

        // particle 실행
        F_ActiveExplisionEffect(_bulletTrs);
    }

    // explotionEffect에 맞는 파티클 실행
    private void F_ActiveExplisionEffect(Transform _exposionTrs)
    {
        if (_exposionTrs == null)
        { 
            Debug.LogError(" Bullet Exploison Transform cant be Null");
            return;
        }

        // 강화 독 (REIN_POISION_COUNT 이상 먹었을 때)
        if (DICT_ExplotionToCount[Explosion_Effect.Rare_PoisionBullet] >= REIN_POISION_COUNT)
        {
            ParticleManager.Instance.F_PlayerParticle(ParticleState.ReinPosionVFX, _exposionTrs.position);
            return;
        }

        // 기본 독 + 기본 폭발 particle 
        if (DICT_ExplotionToCount[Explosion_Effect.Rare_PoisionBullet] >= 1
            && DICT_ExplotionToCount[Explosion_Effect.Rare_IceBullet] < REIN_POISION_COUNT)
        {
            ParticleManager.Instance.F_PlayerParticle(ParticleState.BasicPoisonVFX, _exposionTrs.position);
        }

        // 기본 얼음 + 기본 폭발 particle
        if (DICT_ExplotionToCount[Explosion_Effect.Rare_IceBullet] >= 1)
        {
            ParticleManager.Instance.F_PlayerParticle(ParticleState.BasicIceVFX, _exposionTrs.position);
        }
        

        // 기본 폭발 particle 실행 
        ParticleManager.Instance.F_PlayerParticle(ParticleState.BasicExposionVFX, _exposionTrs.position);

    }

    // 기본 총알 폭발
    public void F_BasicExplosionUse(Transform _bulletTrs, float _size) 
    {
        // 1. 사거리 내 모든 unit 콜라이더 검사 
        Collider[] _coll = F_ReturnColliser(_bulletTrs, _explosionState.explosionRadious, LayerManager.Instance.unitLayer);

        // 2. 검출된 coll에 데미지 주기
        foreach(Collider _unitColl in _coll) 
        {
            try 
            {
                _unitColl.GetComponent<Unit>().F_GetDamage( PlayerManager.Instance.markerBulletController.bulletSate.bulletDamage );
            }
            catch(Exception ex) 
            {
                Debug.LogError(ex.ToString());
                continue;
            }
        }
    }


    // Explotion 카드 획득 시 실행

    public void F_ApplyExplosionEffect(SkillCard v_card)
    {
        // 딕셔너리에 skillcard  검사
        F_DictionaryInt(v_card);

        // 추가된 SkillCard에 맞게 델리게이트에 메서드 추가 
        // Ice Bullet : 초기 1회 획득
        if (DICT_ExplotionToCount[Explosion_Effect.Rare_IceBullet] == 1) 
        {
            del_bulletExplosion += v_card.F_SkillcardEffect;
        }

        // Poision Bullet : 초기 1회 획득
        if (DICT_ExplotionToCount[Explosion_Effect.Rare_PoisionBullet] == 1)
        {
            del_bulletExplosion += v_card.F_SkillcardEffect;
        }

    }

    // skill effect 중복 체크 
    public void F_DictionaryInt(SkillCard v_card)
    {

        // v_card의 _className변수와 같은 enum을 찾기 
        Explosion_Effect _myEffect = default;
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
        {
            Debug.LogError("Bullet Explotion effect cannot be default");
            return;
        }

        // card가 포함이 안되어있으면 ?
        if (!DICT_ExplotionToCount.ContainsKey(_myEffect))
        {
            DICT_ExplotionToCount.Add(_myEffect, 0);
        }
        // 포함이 되어있으면 ?
        else
        {
            DICT_ExplotionToCount[_myEffect] += 1;
        }

    }

    // Rare Effect : Rare_PosionBullet
    public void F_PositionBullet(Transform _bulletTrs, float _radious) 
    {
        // 총알 폭발 시 사거리 안에 있는 unit에게 독 효과 
        // 1. 사거리 내 모든 unit 콜라이더 검사 
        Collider[] _coll = F_ReturnColliser(_bulletTrs, _radious, LayerManager.Instance.unitLayer);

        // ## TODO : 나중에 unit 추가 하고나면 방어력 이 얼마 이상인 unit에겐 데미지 조금주고
        // < 이런거 linq로 할 수 있을듯? 지금은 basic 밖에 없음

        foreach (Collider coll in _coll)
        {
            if (coll.GetComponent<Unit>() == null)
                continue;

            // Unit이 있으면
            // ##TODO 독 데미지 얼마로 하지 ?
            StartCoroutine(IE_Poision(coll.GetComponent<Unit>()));

        }

        IEnumerator IE_Poision(Unit v_unit)
        {
            for (int i = 0; i < 3; i++)
            {
                v_unit.F_GetDamage(0.5f);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    public void F_IceBullet(Transform _bulletTrs, float _radious) 
    {
        // 1. 사거리 내 모든 unit 콜라이더 검사 
        Collider[] _coll = F_ReturnColliser(_bulletTrs, _radious, LayerManager.Instance.unitLayer);

        foreach (Collider coll in _coll)
        {
            if (coll.GetComponent<Unit>() == null)
                continue;

            float _oriSpeed = coll.GetComponent<Unit>().unitSpeed;
            // Unit이 있으면
            // ##TODO 독 데미지 얼마로 하지 ?
            StartCoroutine(IE_Ice(coll.GetComponent<Unit>() , _oriSpeed));

        }

        IEnumerator IE_Ice(Unit v_unit , float v_speed)
        {
            v_unit.F_ChageSpeed(0.5f);
            yield return new WaitForSeconds(1f);
            v_unit.F_ChageSpeed(v_speed);

            Debug.Log( v_unit.unitSpeed );
        }
    }

    // collider 검출 후 return
    public Collider[] F_ReturnColliser(Transform v_trs , float v_radious, LayerMask v_layer) 
    {
        Collider[] _coll = Physics.OverlapSphere(v_trs.position, v_radious, v_layer);

        return _coll;
    }

}
