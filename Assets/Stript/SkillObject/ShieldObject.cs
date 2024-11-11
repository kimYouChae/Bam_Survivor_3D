using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShieldObject : MonoBehaviour
{
    [Header("State")]
    [SerializeField]
    protected Vector3 _minsize;                 // 시작 크기 (최소크기)
    [SerializeField]
    protected Vector3 _maxsize;                 // end 크기 (최대크기)
    [SerializeField]
    protected float _grothSpeed = 0.2f;     // 프레임당 size 크기 증가

    protected void F_SettingShiledObject(Vector3 _min, Vector3 _max)
    {
        this._maxsize = _min;
        this._minsize = _max;
    }

    protected void F_ShieldUpdate()
    {
        // 매 프레임 grothSpeed만큼 커짐 
        gameObject.transform.position += new Vector3(_grothSpeed, _grothSpeed, _grothSpeed);

        // max가 되면 ?
        if (gameObject.transform.position.x == _maxsize.x
            && gameObject.transform.position.y == _maxsize.y
            && gameObject.transform.position.z == _maxsize.z)
        {
            F_EndShiled();
            return;
        }
    }

    // 쉴드 end시 효과 작성 필요 
    protected abstract void F_EndShiled();
    protected Collider[] F_ReturnColliser(Transform v_trs, float v_radious, LayerMask v_layer)
    {
        Collider[] _coll = Physics.OverlapSphere(v_trs.position, v_radious, v_layer);

        return _coll;
    }

}
