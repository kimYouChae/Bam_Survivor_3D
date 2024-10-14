using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerExplosionConteroller : MonoBehaviour
{
    [Header("===중복 검사 Dictionary===")]
    private Dictionary<SkillCard, int> _bulletExplosionEffectDuplication; 
    // skillcard(key)에 맞는 갯수 int (value) 

    public delegate void del_BulletExplosion(GameObject obj );

    // deligate 선언
    public del_BulletExplosion del_bulletExplosion;

    private void Start()
    {
        // 델리게이트에 기본 
        del_bulletExplosion += F_BasicExplosionUse;    
    }

    // 충돌 시 시작
    public void F_BulletExplosionStart(GameObject v_object) 
    {
        // 델리게이트 실행
        del_bulletExplosion(v_object);
    }

    public void F_BasicExplosionUse(GameObject v_obj) 
    {
        // 대상 : unit 오브젝트
        if (v_obj.GetComponent<Unit>() == null)
            return;

        // unit의 hp 깎기 (bulletController의 bulletState의 damage 만큼) 
        v_obj.GetComponent<Unit>().
            F_GetDamage(PlayerManager.instance.markerBulletController.bulletSate.bulletDamage);
    }

    public void F_ApplyExplosionEffect(SkillCard v_card)
    {
        // ##TODO : 효과적용 코드 짜기 
        // 스위치문이던 if문이던 머든써서 dictionary 추가 후 매개변수skillcard랑 비교해서 ,,갯수가..어쩌고...
        // 처음이면 v_card의 효과 추가하고 
        // 아니면 이 스크립트에 함수추가하고 그거 델리게이트에 넣기 

        // card에 해당하는 value + 1 
        try
        {
            _bulletExplosionEffectDuplication[v_card] += 1;
            Debug.Log(v_card.cardName + "의 count는" + _bulletExplosionEffectDuplication[v_card]);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        // 이름으로 비교해야하나 ? 흠 
        if (v_card.cardName == "Rare_PoisionBullet")
        {
            del_bulletExplosion += F_PositionBullet;
        }
        else if (v_card.cardName == "Rare_IceBullet") 
        {
            del_bulletExplosion += F_IceBullet;
        }

    }

    // cvs 에서 데이터베이스 초기화 
    public void F_DictionaryInt(SkillCard v_card) 
    {
        // 초기화 안되어있으면 초기화
        if(_bulletExplosionEffectDuplication == null )
            _bulletExplosionEffectDuplication = new Dictionary<SkillCard, int>();

        // card가 포함이 안되어있으면 ?
        if (!_bulletExplosionEffectDuplication.ContainsKey(v_card))
        {
            _bulletExplosionEffectDuplication.Add(v_card, 0);
        }

    }

    // Rare Effect : Rare_PosionBullet
    public void F_PositionBullet(GameObject v_obj) 
    {
        // 총알 폭발 시 사거리 안에 있는 unit에게 독 효과 
        // 1. 사거리 내 모든 unit 콜라이더 검사 
        Collider2D[] _coll = F_ReturnColliser(v_obj.transform, 2f, UnitManager.Instance.unitLayer);

        // ## TODO : 나중에 unit 추가 하고나면 방어력 이 얼마 이상인 unit에겐 데미지 조금주고
        // < 이런거 linq로 할 수 있을듯? 지금은 basic 밖에 없음

        foreach (Collider2D coll in _coll)
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

    public void F_IceBullet(GameObject v_obj) 
    {
        // 1. 사거리 내 모든 unit 콜라이더 검사 
        Collider2D[] _coll = F_ReturnColliser(v_obj.transform, 2f, UnitManager.Instance.unitLayer);

        foreach (Collider2D coll in _coll)
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
    public Collider2D[] F_ReturnColliser(Transform v_trs , float v_Ra, LayerMask v_layer) 
    {
        Collider2D[] _coll = Physics2D.OverlapCircleAll
            (v_trs.position, v_Ra, v_layer);

        return _coll;
    }

}
