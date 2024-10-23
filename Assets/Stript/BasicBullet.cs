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
    private Vector3 _bulletDestination;   // bullet 도착지
    [SerializeField]
    private BulletSate _bulletState;        // bulletState
    [SerializeField]
    private int _currBounceCount;           // 현재 this가 튕긴 횟수 
    [SerializeField]
    private Vector2 _bulletStartPosition;         // 입사-반사각 위한 시작 위치 
    [SerializeField]
    private bool _iscollisionToWall;            // 벽과 충돌했는지 ?
    [SerializeField]
    private Vector3 lastVelocity;
    [SerializeField]
    private Vector3 _direction;
    
    [Header("===Component===")]
    [SerializeField]private Rigidbody _bulletRidigBody;

    // 프로퍼티
    public Vector3 bulletDestination { get => _bulletDestination; set { _bulletDestination = value; } }
    public BulletSate bulletState { get => _bulletState; set { _bulletState = value; } }
    public Vector2 bulletStartPosition { set { _bulletStartPosition = value; } }
    void Start()
    {
        _bulletRidigBody = gameObject.GetComponent<Rigidbody>();
        _currBounceCount = 0;
        _iscollisionToWall = true;

        // 생성 시 velocity 주기
        _direction = F_ReturnChangeYToZero((_bulletDestination - transform.position).normalized);           // 방향벡터의 정규화 (0~1사이의 수로 정규화됨)

        //Debug.Log("정규화 벡터 : " + _direction );
    }

    void Update()
    {
        if (_bulletDestination != null && _iscollisionToWall)
        {
            _bulletRidigBody.velocity = _direction * _bulletState.bulletSpeed;                   // 방향벡터 * speed로 움직임

            lastVelocity = _bulletRidigBody.velocity;

            //Debug.Log( this.name + "의 속도 : " + lastVelocity);

            // bullet speed 만큼 도착지로 이동 
            /*
            gameObject.transform.position = Vector3.Lerp
                (gameObject.transform.position,
                _bulletDestination.position,
                Time.deltaTime * _bulletState.bulletSpeed);
            */

            //Debug.Log(this.name + "의 속도 : " + _bulletRidigBody.velocity);
            //Debug.Log(this.name + "의 속도2 : " + lastVelocity);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // unit이랑 충돌시
        if (collision.gameObject.CompareTag("Unit"))
        {
            //Debug.Log("Unit이랑 충돌함");

            // ##TODO : markerBulletExplosion의 함수실행 
            PlayerManager.instance.markerExplosionConteroller.F_BulletExplosionStart(gameObject.transform);

            Destroy(gameObject, 0.1f);
        }

        // wall이랑 충돌 시 
        if (collision.gameObject.CompareTag("Wall"))
        {
            //Debug.LogError("벽이랑 충돌 ");

            _iscollisionToWall = false;

            // max랑 횟수가 같아지면 -> destory
            if (_currBounceCount == _bulletState.bulletBounceCount)
            {
                // ##TODO : markerBulletExplosion의 함수실행 
                Destroy(gameObject, 0.1f);
                return;
            }

            //입사벡터
            Vector3 inDirection = lastVelocity;                             // 벡터(속도,방향 포함) , 리지드바디의 velocity 또한 속도와 방향을 포함
            Vector3 inNormal = collision.contacts[0].normal;                // 충돌체의 노말벡터
            Vector3 newVelocity = Vector3.Reflect(inDirection, inNormal);   // 반사각구하기

            // 잠시 0으로
            _bulletRidigBody.velocity = Vector3.zero;

            // 그 후 새로운 반사각으로 velocity 추가 
            //_bulletRidigBody.velocity = newVelocity * _bulletState.bulletSpeed;

            _direction = F_ReturnChangeYToZero(newVelocity);

            //Debug.Log( "마지막 속도 : " + lastVelocity + "입사각 " + inDirection + " / 노말벡터 " + inNormal + " / 반사각 " + newVelocity );
            //Debug.Log(_bulletRidigBody.velocity);

            // 튕김 횟수 +1
            _currBounceCount++;

            _iscollisionToWall = true;
        }

    }

    // 들어온 Vector3에서 y값을 0으로 만들어줌 
    private Vector3 F_ReturnChangeYToZero(Vector3 _input)  
    {
        return new Vector3( _input.x , 0 , _input.z );
    }

}
