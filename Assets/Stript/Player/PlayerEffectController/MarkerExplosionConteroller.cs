using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerExplosionConteroller : MonoBehaviour
{
    [Header("===Radious===")]
    [SerializeField] ExplosionState _explosionState;

    [Header("===�ߺ� �˻� Dictionary===")]
    private Dictionary<Explosion_Effect, int> DICT_ExplotionToCount; 
    // skillcard(key)�� �´� ���� int (value) 

    public delegate void del_BulletExplosion( Transform _bulletTrs, float _size);

    // deligate ����
    public del_BulletExplosion del_bulletExplosion;

    [Header("===Skill Effect Ratio===")]
    [SerializeField] const int REIN_POISION_COUNT   = 4;
    [SerializeField] const int REIN_ICE_COUNT       = 0;

    private void Start()
    {
        // ##TODO : �ӽ� (3f) : explosion State �ʱ�ȭ
        _explosionState = new ExplosionState(1f);

        // �ʱ� 1ȸ Dic �ʱ�ȭ
        F_InitDictionary();

        // ��������Ʈ�� �⺻ 
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

            // key�� ������ ���� ������ 
            if (!DICT_ExplotionToCount.ContainsKey(_effect[i]))
                DICT_ExplotionToCount.Add(_effect[i], 0);
        }
    }

    // �浹 �� ����
    public void F_BulletExplosionStart(Transform _bulletTrs, float _size = 0) 
    {
        // ��������Ʈ ����
        _size = _explosionState.explosionRadious;
        del_bulletExplosion(_bulletTrs , _size );

        // particle ����
        F_ActiveExplisionEffect(_bulletTrs);
    }

    // explotionEffect�� �´� ��ƼŬ ����
    private void F_ActiveExplisionEffect(Transform _exposionTrs)
    {
        if (_exposionTrs == null)
        { 
            Debug.LogError(" Bullet Exploison Transform cant be Null");
            return;
        }

        // ��ȭ �� (REIN_POISION_COUNT �̻� �Ծ��� ��)
        if (DICT_ExplotionToCount[Explosion_Effect.Rare_PoisionBullet] >= REIN_POISION_COUNT)
        {
            ParticleManager.Instance.F_PlayerParticle(ParticleState.ReinPosionVFX, _exposionTrs.position);
            return;
        }

        // �⺻ �� + �⺻ ���� particle 
        if (DICT_ExplotionToCount[Explosion_Effect.Rare_PoisionBullet] >= 1
            && DICT_ExplotionToCount[Explosion_Effect.Rare_IceBullet] < REIN_POISION_COUNT)
        {
            ParticleManager.Instance.F_PlayerParticle(ParticleState.BasicPoisonVFX, _exposionTrs.position);
        }

        // �⺻ ���� + �⺻ ���� particle
        if (DICT_ExplotionToCount[Explosion_Effect.Rare_IceBullet] >= 1)
        {
            ParticleManager.Instance.F_PlayerParticle(ParticleState.BasicIceVFX, _exposionTrs.position);
        }
        

        // �⺻ ���� particle ���� 
        ParticleManager.Instance.F_PlayerParticle(ParticleState.BasicExposionVFX, _exposionTrs.position);

    }

    // �⺻ �Ѿ� ����
    public void F_BasicExplosionUse(Transform _bulletTrs, float _size) 
    {
        // 1. ��Ÿ� �� ��� unit �ݶ��̴� �˻� 
        Collider[] _coll = F_ReturnColliser(_bulletTrs, _explosionState.explosionRadious, LayerManager.Instance.unitLayer);

        // 2. ����� coll�� ������ �ֱ�
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


    // Explotion ī�� ȹ�� �� ����

    public void F_ApplyExplosionEffect(SkillCard v_card)
    {
        // ��ųʸ��� skillcard  �˻�
        F_DictionaryInt(v_card);

        // �߰��� SkillCard�� �°� ��������Ʈ�� �޼��� �߰� 
        // Ice Bullet : �ʱ� 1ȸ ȹ��
        if (DICT_ExplotionToCount[Explosion_Effect.Rare_IceBullet] == 1) 
        {
            del_bulletExplosion += v_card.F_SkillcardEffect;
        }

        // Poision Bullet : �ʱ� 1ȸ ȹ��
        if (DICT_ExplotionToCount[Explosion_Effect.Rare_PoisionBullet] == 1)
        {
            del_bulletExplosion += v_card.F_SkillcardEffect;
        }

    }

    // skill effect �ߺ� üũ 
    public void F_DictionaryInt(SkillCard v_card)
    {

        // v_card�� _className������ ���� enum�� ã�� 
        Explosion_Effect _myEffect = default;
        try
        {
            // _myEffect�� _className�� ���� enum�� ���
            Enum.TryParse(v_card.classSpriteName, out _myEffect);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }

        // ���� ������ �ȵǰ� default�� ���������� ? -> return 
        if (_myEffect == default)
        {
            Debug.LogError("Bullet Explotion effect cannot be default");
            return;
        }

        // card�� ������ �ȵǾ������� ?
        if (!DICT_ExplotionToCount.ContainsKey(_myEffect))
        {
            DICT_ExplotionToCount.Add(_myEffect, 0);
        }
        // ������ �Ǿ������� ?
        else
        {
            DICT_ExplotionToCount[_myEffect] += 1;
        }

    }

    // Rare Effect : Rare_PosionBullet
    public void F_PositionBullet(Transform _bulletTrs, float _radious) 
    {
        // �Ѿ� ���� �� ��Ÿ� �ȿ� �ִ� unit���� �� ȿ�� 
        // 1. ��Ÿ� �� ��� unit �ݶ��̴� �˻� 
        Collider[] _coll = F_ReturnColliser(_bulletTrs, _radious, LayerManager.Instance.unitLayer);

        // ## TODO : ���߿� unit �߰� �ϰ��� ���� �� �� �̻��� unit���� ������ �����ְ�
        // < �̷��� linq�� �� �� ������? ������ basic �ۿ� ����

        foreach (Collider coll in _coll)
        {
            if (coll.GetComponent<Unit>() == null)
                continue;

            // Unit�� ������
            // ##TODO �� ������ �󸶷� ���� ?
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
        // 1. ��Ÿ� �� ��� unit �ݶ��̴� �˻� 
        Collider[] _coll = F_ReturnColliser(_bulletTrs, _radious, LayerManager.Instance.unitLayer);

        foreach (Collider coll in _coll)
        {
            if (coll.GetComponent<Unit>() == null)
                continue;

            float _oriSpeed = coll.GetComponent<Unit>().unitSpeed;
            // Unit�� ������
            // ##TODO �� ������ �󸶷� ���� ?
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

    // collider ���� �� return
    public Collider[] F_ReturnColliser(Transform v_trs , float v_radious, LayerMask v_layer) 
    {
        Collider[] _coll = Physics.OverlapSphere(v_trs.position, v_radious, v_layer);

        return _coll;
    }

}
