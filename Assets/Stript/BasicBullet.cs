using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Debug = UnityEngine.Debug;

public class BasicBullet : MonoBehaviour
{
    [Header("===Bullet Field===")]
    [SerializeField]
    private Vector3 _bulletDestination;   // bullet ������
    [SerializeField]
    private BulletSate _bulletState;        // bulletState
    [SerializeField]
    private int _currBounceCount;           // ���� this�� ƨ�� Ƚ�� 
    [SerializeField]
    private Vector2 _bulletStartPosition;         // �Ի�-�ݻ簢 ���� ���� ��ġ 
    [SerializeField]
    private bool _iscollisionToWall;            // ���� �浹�ߴ��� ?
    [SerializeField]
    private Vector3 lastVelocity;
    [SerializeField]
    private Vector3 _direction;
    
    [Header("===Component===")]
    [SerializeField]private Rigidbody _bulletRidigBody;

    // ������Ƽ
    public Vector3 bulletDestination { get => _bulletDestination; set { _bulletDestination = value; } }
    public BulletSate bulletState { get => _bulletState; set { _bulletState = value; } }
    public Vector2 bulletStartPosition { set { _bulletStartPosition = value; } }
    void Start()
    {
        _bulletRidigBody = gameObject.GetComponent<Rigidbody>();
        _currBounceCount = 0;
        _iscollisionToWall = true;

        // ���� �� velocity �ֱ�
        _direction = F_ReturnChangeYToZero((_bulletDestination - transform.position).normalized);           // ���⺤���� ����ȭ (0~1������ ���� ����ȭ��)

        //Debug.Log("����ȭ ���� : " + _direction );
    }

    void Update()
    {
        if (_bulletDestination != null && _iscollisionToWall)
        {
            _bulletRidigBody.velocity = _direction * _bulletState.bulletSpeed;                   // ���⺤�� * speed�� ������

            lastVelocity = _bulletRidigBody.velocity;

            //Debug.Log( this.name + "�� �ӵ� : " + lastVelocity);

            // bullet speed ��ŭ �������� �̵� 
            /*
            gameObject.transform.position = Vector3.Lerp
                (gameObject.transform.position,
                _bulletDestination.position,
                Time.deltaTime * _bulletState.bulletSpeed);
            */

            //Debug.Log(this.name + "�� �ӵ� : " + _bulletRidigBody.velocity);
            //Debug.Log(this.name + "�� �ӵ�2 : " + lastVelocity);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // unit�̶� �浹��
        if (collision.gameObject.CompareTag("Unit"))
        {
            //Debug.Log("Unit�̶� �浹��");

            // ##TODO : markerBulletExplosion�� �Լ����� 
            PlayerManager.instance.markerExplosionConteroller.F_BulletExplosionStart(gameObject.transform);

            Destroy(gameObject, 0.1f);
        }

        // wall�̶� �浹 �� 
        if (collision.gameObject.CompareTag("Wall"))
        {
            //Debug.LogError("���̶� �浹 ");

            _iscollisionToWall = false;

            // max�� Ƚ���� �������� -> destory
            if (_currBounceCount == _bulletState.bulletBounceCount)
            {
                // ##TODO : markerBulletExplosion�� �Լ����� 
                Destroy(gameObject, 0.1f);
                return;
            }

            //�Ի纤��
            Vector3 inDirection = lastVelocity;                             // ����(�ӵ�,���� ����) , ������ٵ��� velocity ���� �ӵ��� ������ ����
            Vector3 inNormal = collision.contacts[0].normal;                // �浹ü�� �븻����
            Vector3 newVelocity = Vector3.Reflect(inDirection, inNormal);   // �ݻ簢���ϱ�

            // ��� 0����
            _bulletRidigBody.velocity = Vector3.zero;

            // �� �� ���ο� �ݻ簢���� velocity �߰� 
            //_bulletRidigBody.velocity = newVelocity * _bulletState.bulletSpeed;

            _direction = F_ReturnChangeYToZero(newVelocity);

            //Debug.Log( "������ �ӵ� : " + lastVelocity + "�Ի簢 " + inDirection + " / �븻���� " + inNormal + " / �ݻ簢 " + newVelocity );
            //Debug.Log(_bulletRidigBody.velocity);

            // ƨ�� Ƚ�� +1
            _currBounceCount++;

            _iscollisionToWall = true;
        }

    }

    // ���� Vector3���� y���� 0���� ������� 
    private Vector3 F_ReturnChangeYToZero(Vector3 _input)  
    {
        return new Vector3( _input.x , 0 , _input.z );
    }

}
