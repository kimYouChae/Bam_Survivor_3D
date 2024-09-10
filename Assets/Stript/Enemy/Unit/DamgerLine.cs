using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamgerLine : MonoBehaviour
{
    [SerializeField]
    Vector3 _endPosition;

    public Vector3 EndPosition { get => _endPosition; set { _endPosition = value;} }

    void Start()
    {
         
    }

    void Update()
    {
        // ## TODO : �Ѿ� �ӵ��� �ٲٱ� 
        transform.position = Vector3.Lerp(transform.position, _endPosition, Time.deltaTime * 3.5f);
    }

    // ���� �浹�ϸ� ����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            Destroy(gameObject , 0.2f);
    }
}
