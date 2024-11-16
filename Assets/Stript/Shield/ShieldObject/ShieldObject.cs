using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public abstract class ShieldObject : MonoBehaviour
{
    [Header("State")]
    [SerializeField]
    protected Shield_Effect _effect;
    [SerializeField]
    protected Vector3 _minsize;                // 시작 크기 (최소크기)
    [SerializeField]
    protected Vector3 _maxsize;                // end 크기 (최대크기)
    [SerializeField]
    protected Marker _parentMarker;            // 기준이 되는 marker 

    public Marker parentMarker { get => _parentMarker; set { _parentMarker = value; } }

    [Header("Lerp")]
    private float currentTime;
    private float lerpTime = 1f;

    public void F_SettingShiledObject(Shield_Effect _effect ,Vector3 _min, Vector3 _max)
    {
        this._effect    = _effect;
        this._minsize   = _min;
        this._maxsize   = _max;

        gameObject.transform.localScale = this._minsize;
    }


    private void OnDisable()
    {
        currentTime = 0;

        // 다 커지고 나서 off 될 때
        // 본인크기를 min size로
        gameObject.transform.localScale = this._minsize;
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
            && gameObject.transform.localScale.y >= _maxsize.y
            && gameObject.transform.localScale.z >= _maxsize.z)
        {
            // 쉴드 end 동작 
            F_EndShiled();
        }
    }

    protected void F_FllowMarker() 
    {
        gameObject.transform.position = _parentMarker.transform.position;
    }

    // 쉴드 end시 효과 작성 필요 
    protected abstract void F_EndShiled();
    // 쉴드 확장할 때 효과 작성 필요
    protected abstract void F_ExpandingShield();

    protected Collider[] F_ReturnUnitCollider(GameObject _obj, float _size , LayerMask _layer)
    {
        Collider[] _coll = Physics.OverlapSphere(_obj.transform.position, _size, _layer);

        return _coll;
    }



}
