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

    private void Start()
    {
        // ##TODO : �ӽ� (3f) : explosion State �ʱ�ȭ
        _explosionState = new ExplosionState(3f);
        
        // ��������Ʈ�� �⺻ 
        del_bulletExplosion += F_BasicExplosionUse;    
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
        // �⺻ ���� particle
        ParticleManager.instance.F_PlayerParticle(ParticleState.BasicExposionVFX, _exposionTrs);

        // ##TODO : effect�� �´� ��ƼŬ ���� 
        /*
        if (DICT_ExplotionToCount[Explosion_Effect.Rare_PoisionBullet] == 1) 
        {
            ParticleManager.instance.F_PlayerParticle( ParticleState.BasicPoisonParticle , _exposionTrs);
        }
        */
    }

    // �⺻ �Ѿ� ����
    public void F_BasicExplosionUse(Transform _bulletTrs, float _size) 
    {
        // 1. ��Ÿ� �� ��� unit �ݶ��̴� �˻� 
        Collider[] _coll = F_ReturnColliser(_bulletTrs, _explosionState.explosionRadious, UnitManager.Instance.unitLayer);

        // 2. ����� coll�� ������ �ֱ�
        foreach(Collider _unitColl in _coll) 
        {
            try 
            {
                _unitColl.GetComponent<Unit>().F_GetDamage( PlayerManager.instance.markerBulletController.bulletSate.bulletDamage );
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
        // �ʱ�ȭ �ȵǾ������� �ʱ�ȭ
        if (DICT_ExplotionToCount == null)
            DICT_ExplotionToCount = new Dictionary<Explosion_Effect, int>();

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
        Collider[] _coll = F_ReturnColliser(_bulletTrs, _radious, UnitManager.Instance.unitLayer);

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
        Collider[] _coll = F_ReturnColliser(_bulletTrs, _radious, UnitManager.Instance.unitLayer);

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
