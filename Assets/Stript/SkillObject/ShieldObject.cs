using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public abstract class ShieldObject : MonoBehaviour
{
    [Header("State")]
    [SerializeField]
    protected Vector3 _minsize = new Vector3(0.5f,0.1f,0.5f);                 // ���� ũ�� (�ּ�ũ��)
    [SerializeField]
    protected Vector3 _maxsize = new Vector3(2f, 0.1f, 2f);                   // end ũ�� (�ִ�ũ��)

    [Header("Lerp")]
    private float currentTime;
    private float lerpTime = 1f;

    protected void F_SettingShiledObject(Vector3 _min, Vector3 _max)
    {
        this._maxsize = _min;
        this._minsize = _max;
    }

    // on �� �� minSize�� ����
    private void OnEnable()
    {
        gameObject.transform.localScale = _minsize;
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
            || gameObject.transform.localScale.z >= _maxsize.z)
        {
            // ���� end ���� 
            F_EndShiled();
        }
    }

    // ���� end�� ȿ�� �ۼ� �ʿ� 
    protected abstract void F_EndShiled();
    // ���� Ȯ���� �� ȿ�� �ۼ� �ʿ�
    protected abstract void F_ExpandingShield();

    protected Collider[] F_ReturnUnitCollider(GameObject _obj, float _size)
    {
        Collider[] _coll = Physics.OverlapSphere(_obj.transform.position, _size, LayerManager.instance.unitLayer);

        return _coll;
    }



}
