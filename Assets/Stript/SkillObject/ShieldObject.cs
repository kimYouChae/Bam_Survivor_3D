using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public abstract class ShieldObject : MonoBehaviour
{
    [Header("State")]
    [SerializeField]
    protected Vector3 _minsize = new Vector3(0.5f,0.1f,0.5f);                 // 시작 크기 (최소크기)
    [SerializeField]
    protected Vector3 _maxsize = new Vector3(2f, 0.1f, 2f);                   // end 크기 (최대크기)

    [Header("Lerp")]
    private float currentTime;
    private float lerpTime = 1f;

    protected void F_SettingShiledObject(Vector3 _min, Vector3 _max)
    {
        this._maxsize = _min;
        this._minsize = _max;
    }

    // on 될 때 minSize로 지정
    private void OnEnable()
    {
        gameObject.transform.localScale = _minsize;
    }

    protected void F_ShieldUpdate()
    {
        // Lerp로 크기 커지게
        currentTime += Time.deltaTime;
        if (currentTime >= lerpTime)
        {
            currentTime = lerpTime;
        }

        float t = currentTime / lerpTime;
        //t = t*t*t*(t*(6f*t-15f) + 10f);
        t = Mathf.Sin(t * Mathf.PI * 0.5f);         // 처음엔 빠르고 도착할 땐 Smooth 하게 
        transform.localScale = Vector3.Lerp(_minsize, _maxsize, t);

        // 쉴드 expanding 효과 적용
        F_ExpandingShield(); 

        // max가 되면 ?
        if (gameObject.transform.localScale.x >= _maxsize.x
            || gameObject.transform.localScale.z >= _maxsize.z)
        {
            // 쉴드 end 동작 
            F_EndShiled();
        }
    }

    // 쉴드 end시 효과 작성 필요 
    protected abstract void F_EndShiled();
    // 쉴드 확장할 때 효과 작성 필요
    protected abstract void F_ExpandingShield();

    protected Collider[] F_ReturnUnitCollider(GameObject _obj, float _size)
    {
        Collider[] _coll = Physics.OverlapSphere(_obj.transform.position, _size, LayerManager.instance.unitLayer);

        return _coll;
    }



}
