using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerMovement : MonoBehaviour
{
    [Header("===snake State===")]
    [SerializeField] private float _speed;                               // �Ӹ� �ӵ�
    [SerializeField] private bool _isReadToMove;                         // ������ �غ� ��

    [Header("===snake Move===")]
    private Vector2 _joystickVec;                       // ���̽�ƽ�� vec 
    private List<Transform> _markerNowTransform;        // marker ������ ���� ����Ʈ 

    public Vector3 joystickVec { set { _joystickVec = value; } }

    void Start()
    {
        _speed = 3f;
        _joystickVec = new Vector3(0,0,1f);
        _isReadToMove = true;

        _markerNowTransform = new List<Transform>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isReadToMove)
        {
            // �Ӹ� ������ 
            F_HeadMoveControl();

            // ���� ������ 
            F_SnakeBodyMovement();
        }

    }

    // �Ӹ� ������ ��Ʈ��
    private void F_HeadMoveControl()
    {
        // ���̽�ƽ vector ����
        // y�� ���̽�ƽ�� ���� ���� �� (0���� Ŭ ��) ,  �Ʒ��� ���� �� (0���� ���� ��) �� ����
        Vector3 _joyVec = new Vector3(_joystickVec.x, 0 ,_joystickVec.y > 0 ? 1f : -1f);

        // head �����̱� 
        PlayerManager.instance.markers[0].gameObject.transform.Translate
            (_joyVec * _speed * Time.deltaTime);

    }

    private void F_SnakeBodyMovement()
    {
        // �迭 �ʱ�ȭ 
        _markerNowTransform.Clear();

        // ���� �Ӹ� + ���� ��ġ ��Ƶα�
        for (int i = 0; i < PlayerManager.instance.markers.Count; i++)
        {
            _markerNowTransform.Add(PlayerManager.instance.markers[i].transform);
        }

        // �̵� , �Ӹ�����
        for (int i = 1; i < PlayerManager.instance.markers.Count; i++)
        {
            Transform _nowMarker = PlayerManager.instance.markers[i].transform;
            PlayerManager.instance.markers[i].transform.position = Vector3.Lerp(
                PlayerManager.instance.markers[i].transform.position,
                _markerNowTransform[i - 1].transform.position,
                _speed * Time.deltaTime);
        }
    }
}
