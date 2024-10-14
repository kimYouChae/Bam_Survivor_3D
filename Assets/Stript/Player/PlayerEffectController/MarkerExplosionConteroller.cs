using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerExplosionConteroller : MonoBehaviour
{
    [Header("===�ߺ� �˻� Dictionary===")]
    private Dictionary<SkillCard, int> _bulletExplosionEffectDuplication; 
    // skillcard(key)�� �´� ���� int (value) 

    public delegate void del_BulletExplosion(GameObject obj );

    // deligate ����
    public del_BulletExplosion del_bulletExplosion;

    private void Start()
    {
        // ��������Ʈ�� �⺻ 
        del_bulletExplosion += F_BasicExplosionUse;    
    }

    // �浹 �� ����
    public void F_BulletExplosionStart(GameObject v_object) 
    {
        // ��������Ʈ ����
        del_bulletExplosion(v_object);
    }

    public void F_BasicExplosionUse(GameObject v_obj) 
    {
        // ��� : unit ������Ʈ
        if (v_obj.GetComponent<Unit>() == null)
            return;

        // unit�� hp ��� (bulletController�� bulletState�� damage ��ŭ) 
        v_obj.GetComponent<Unit>().
            F_GetDamage(PlayerManager.instance.markerBulletController.bulletSate.bulletDamage);
    }

    public void F_ApplyExplosionEffect(SkillCard v_card)
    {
        // ##TODO : ȿ������ �ڵ� ¥�� 
        // ����ġ���̴� if���̴� �ӵ�Ἥ dictionary �߰� �� �Ű�����skillcard�� ���ؼ� ,,������..��¼��...
        // ó���̸� v_card�� ȿ�� �߰��ϰ� 
        // �ƴϸ� �� ��ũ��Ʈ�� �Լ��߰��ϰ� �װ� ��������Ʈ�� �ֱ� 

        // card�� �ش��ϴ� value + 1 
        try
        {
            _bulletExplosionEffectDuplication[v_card] += 1;
            Debug.Log(v_card.cardName + "�� count��" + _bulletExplosionEffectDuplication[v_card]);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        // �̸����� ���ؾ��ϳ� ? �� 
        if (v_card.cardName == "Rare_PoisionBullet")
        {
            del_bulletExplosion += F_PositionBullet;
        }
        else if (v_card.cardName == "Rare_IceBullet") 
        {
            del_bulletExplosion += F_IceBullet;
        }

    }

    // cvs ���� �����ͺ��̽� �ʱ�ȭ 
    public void F_DictionaryInt(SkillCard v_card) 
    {
        // �ʱ�ȭ �ȵǾ������� �ʱ�ȭ
        if(_bulletExplosionEffectDuplication == null )
            _bulletExplosionEffectDuplication = new Dictionary<SkillCard, int>();

        // card�� ������ �ȵǾ������� ?
        if (!_bulletExplosionEffectDuplication.ContainsKey(v_card))
        {
            _bulletExplosionEffectDuplication.Add(v_card, 0);
        }

    }

    // Rare Effect : Rare_PosionBullet
    public void F_PositionBullet(GameObject v_obj) 
    {
        // �Ѿ� ���� �� ��Ÿ� �ȿ� �ִ� unit���� �� ȿ�� 
        // 1. ��Ÿ� �� ��� unit �ݶ��̴� �˻� 
        Collider2D[] _coll = F_ReturnColliser(v_obj.transform, 2f, UnitManager.Instance.unitLayer);

        // ## TODO : ���߿� unit �߰� �ϰ��� ���� �� �� �̻��� unit���� ������ �����ְ�
        // < �̷��� linq�� �� �� ������? ������ basic �ۿ� ����

        foreach (Collider2D coll in _coll)
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

    public void F_IceBullet(GameObject v_obj) 
    {
        // 1. ��Ÿ� �� ��� unit �ݶ��̴� �˻� 
        Collider2D[] _coll = F_ReturnColliser(v_obj.transform, 2f, UnitManager.Instance.unitLayer);

        foreach (Collider2D coll in _coll)
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
    public Collider2D[] F_ReturnColliser(Transform v_trs , float v_Ra, LayerMask v_layer) 
    {
        Collider2D[] _coll = Physics2D.OverlapCircleAll
            (v_trs.position, v_Ra, v_layer);

        return _coll;
    }

}
