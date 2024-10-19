using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class MarkerBulletController : MonoBehaviour
{
    /// <summary>
    ///  �Ѿ��� ��������Ʈ ��� �� �ص� �� �� 
    ///  
    /// ** Ư�� �Ѿ��� ī�带 ȹ�� �� �� , Ư��Ÿ���̸�? 
    /// �Ѿ˿� ��ũ��ƮaddComponent�ؼ� �浹�� �󸮰�...���ְ�...��� �ϸ� ���� ������ ?
    /// => ���߿� pooling ����� �� ������ �̸� �������� ����ϸ� �ɵ� 
    /// 
    /// </summary>

    [Header("===Bullet Sate===")]
    [SerializeField] private BulletSate _bulletSate;

    [Header("===basic Bullet Object===")]
    [SerializeField]
    private GameObject _basicBulletObject;

    // ������Ƽ
    public BulletSate bulletSate => _bulletSate;

    private void Start()
    {
        // �Ѿ� state �ʱ�ȭ 
        _bulletSate = new BulletSate(
            _bulletCnt          : 1, 
            _bulletSpeed        : 3f, 
            _bulletDamage       : 1f, 
            _bulletSize         : 1f, 
            _bulletBounceCnt    : 1);

        // ���� , �ӵ�, ������, ũ�� , ƨ��� Ƚ�� 

    }

    public void F_BasicBulletShoot(Transform v_muzzleTrs)  // �ѱ� ��ġ 
    {
        Debug.Log("�⺻ �Ѿ� �߻�");

        // unit collider Ž��
        Transform _destination;

        //##TODO : ���⼭ ������ null 
        // unit�� �ݶ��̴� �˻� 
        Collider[] _coll = Physics.OverlapSphere
            (v_muzzleTrs.position, PlayerManager.instance.markers[0].markerState.markerSearchRadious, UnitManager.Instance.unitLayer);
        
        // ����Ȱ� ������ ����
        if (_coll.Length <= 0) 
            return;

        // �Ѿ� �߻� ���� ��ŭ 
        for (int i = 0; i < _bulletSate.bulletCount; i++)
        {
            // ����Ȱ� ���� ������ ����
            _destination = _coll[0].transform;

            // bullet ���� 
            GameObject _bullet = Instantiate(_basicBulletObject, v_muzzleTrs.position, Quaternion.identity);

            // bullet�� ������ �����ֱ�
            // ##TODO : bullet �Ѿ� ������� ���� �ٲٱ� ������ basic����  
            _bullet.GetComponent<BasicBullet>().bulletState.F_SetField(this.bulletSate);
            _bullet.GetComponent<BasicBullet>().bulletDestination = _destination.position;
            _bullet.GetComponent<BasicBullet>().bulletStartPosition = v_muzzleTrs.position;
        }

    }

    public void F_ApplyBulletEffect(SkillCard v_card) 
    {
        // skill card�� effect �߰� 
        v_card.F_SkillcardEffect();

        // ##TODO : Ui ���� 
    }

    // Bullet State ������Ʈ 
    public void F_UpdateBulletState(int BulletCnt = 0 , float BulletSpeedPercent = 0 , float BulletDamagePercent = 0 , float BulletSizePercent = 0 , int BulletBounceCount = 0 ) 
    {
        bulletSate.bulletCount  += BulletCnt;
        bulletSate.bulletSpeed  += bulletSate.bulletSpeed * BulletSpeedPercent;
        bulletSate.bulletDamage += bulletSate.bulletDamage * BulletDamagePercent;
        bulletSate.bulletSize   += bulletSate.bulletSize * BulletSizePercent;
        bulletSate.bulletCount  += BulletBounceCount;
    }

}