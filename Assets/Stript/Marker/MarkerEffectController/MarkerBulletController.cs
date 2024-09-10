using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

// bullet state
[System.Serializable]
public class BulletSate
{
    [SerializeField] int _bulletCount;    // �ѹ��� �����ϴ� �Ѿ� ����
    [SerializeField] float _bulletSpeed;    // �Ѿ� �ӵ�
    [SerializeField] float _bulletDamage;   // �Ѿ� ������ 
    [SerializeField] float _bulletSize;     // �Ѿ� ũ�� 
    [SerializeField] int _bulletBounceCount;  // �Ѿ� ƨ��� Ƚ�� 

    // ������Ƽ 
    public int bulletCount { get => _bulletCount; set { _bulletCount = value; } }
    public float bulletSpeed { get => _bulletSpeed; set { _bulletSpeed = value; } }
    public float bulletDamage { get => _bulletDamage; set { _bulletDamage = value; } }
    public float bulletSize { get => _bulletSize; set { _bulletSize = value; } }
    public int bulletBounceCount { get => _bulletBounceCount; set { _bulletBounceCount = value; } }

    // ������
    public BulletSate(int v_cnt, float v_speed, float v_damage, float v_size, int _cnt)
    {
        this._bulletCount = v_cnt;
        this._bulletSpeed = v_speed;
        this._bulletDamage = v_damage;
        this._bulletSize = v_size;
        this._bulletBounceCount = _cnt;
    }

    public void F_SetField(BulletSate v_state) 
    {
        this._bulletCount = v_state._bulletCount;
        this._bulletSpeed = v_state._bulletSpeed;
        this._bulletDamage = v_state._bulletDamage;
        this._bulletSize = v_state._bulletSize;
        this._bulletBounceCount = v_state._bulletBounceCount;
    }
}

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
        _bulletSate = new BulletSate(1, 3f, 1f, 1f, 1);

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
        // ##TODO : ȿ������ �ڵ� ¥�� 
    }
}