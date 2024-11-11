using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShieldObject : MonoBehaviour
{
    [Header("State")]
    [SerializeField]
    protected Vector3 _minsize;                 // ���� ũ�� (�ּ�ũ��)
    [SerializeField]
    protected Vector3 _maxsize;                 // end ũ�� (�ִ�ũ��)
    [SerializeField]
    protected float _grothSpeed = 0.2f;     // �����Ӵ� size ũ�� ����

    protected void F_SettingShiledObject(Vector3 _min, Vector3 _max)
    {
        this._maxsize = _min;
        this._minsize = _max;
    }

    protected void F_ShieldUpdate()
    {
        // �� ������ grothSpeed��ŭ Ŀ�� 
        gameObject.transform.position += new Vector3(_grothSpeed, _grothSpeed, _grothSpeed);

        // max�� �Ǹ� ?
        if (gameObject.transform.position.x == _maxsize.x
            && gameObject.transform.position.y == _maxsize.y
            && gameObject.transform.position.z == _maxsize.z)
        {
            F_EndShiled();
            return;
        }
    }

    // ���� end�� ȿ�� �ۼ� �ʿ� 
    protected abstract void F_EndShiled();
    protected Collider[] F_ReturnColliser(Transform v_trs, float v_radious, LayerMask v_layer)
    {
        Collider[] _coll = Physics.OverlapSphere(v_trs.position, v_radious, v_layer);

        return _coll;
    }

}
