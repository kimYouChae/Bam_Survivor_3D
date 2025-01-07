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
            // 2. marker�� �浹
            collision.gameObject.GetComponent<Marker>().F_UpdateHP(_unitBulletDamage);
        }
        // 1. wall�̶� �浹
        // 3. building�̶� �浹        
        // => pooling���� �ǵ����� 
        UnitManager.Instance.UnitBulletPooling.F_UnitBulletSet(gameObject, _unitBulletType);
    }
}
