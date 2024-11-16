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
    protected Vector3 _minsize;                // ���� ũ�� (�ּ�ũ��)
    [SerializeField]
    protected Vector3 _maxsize;                // end ũ�� (�ִ�ũ��)
    [SerializeField]
    protected Marker _parentMarker;            // ������ �Ǵ� marker 

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

        // �� Ŀ���� ���� off �� ��
        // ����ũ�⸦ min size��
        gameObject.transform.localScale = this._minsize;
    }

    protected void F_ShieldUpdate()
    {
        // Lerp�� ũ�� Ŀ����
        currentTime += Time.deltaTime;
        if (currentTime >= lerpTime)
        {
            currentTime = lerpTime;
        }

        float t = currentTime / lerpTime;
        //t = t*t*t*(t*(6f*t-15f) + 10f);
        t = Mathf.Sin(t * Mathf.PI * 0.5f);         // ó���� ������ ������ �� Smooth �ϰ� 
        transform.localScale = Vector3.Lerp(_minsize, _maxsize, t);

        // ���� expanding ȿ�� ����
        F_ExpandingShield(); 

        // max�� �Ǹ� ?
        if (gameObject.transform.localScale.x >= _maxsize.x
            && gameObject.transform.localScale.y >= _maxsize.y
            && gameObject.transform.localScale.z >= _maxsize.z)
        {
            // ���� end ���� 
            F_EndShiled();
        }
    }

    protected void F_FllowMarker() 
    {
        gameObject.transform.position = _parentMarker.transform.position;
    }

    // ���� end�� ȿ�� �ۼ� �ʿ� 
    protected abstract void F_EndShiled();
    // ���� Ȯ���� �� ȿ�� �ۼ� �ʿ�
    protected abstract void F_ExpandingShield();

    protected Collider[] F_ReturnUnitCollider(GameObject _obj, float _size , LayerMask _layer)
    {
        Collider[] _coll = Physics.OverlapSphere(_obj.transform.position, _size, _layer);

        return _coll;
    }



}
