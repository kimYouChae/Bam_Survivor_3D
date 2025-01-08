using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBullet : MonoBehaviour
{
    [SerializeField]
    private UnitBulletType _unitBulletType;
    [SerializeField]
    private float _unitBulletDamage;

    public UnitBulletType BulletType { set { _unitBulletType = value; } }
    public float Damage { set { _unitBulletDamage = value;} }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer
            == LayerManager.Instance.markerLayerNum) 
        {
            // 2. marker랑 충돌
            collision.gameObject.GetComponent<Marker>().F_UpdateHP(_unitBulletDamage);
        }
        // 1. wall이랑 충돌
        // 3. building이랑 충돌        
        
        // addForce한거 초기화
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        // => pooling으로 되돌리기 
        UnitManager.Instance.UnitBulletPooling.F_UnitBulletSet(gameObject, _unitBulletType);
    }
}
