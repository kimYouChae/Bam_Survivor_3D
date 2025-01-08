using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerMovement : MonoBehaviour
{
    [Header("===snake State===")]
    [SerializeField] private bool _isReadToMove;                         // ������ �غ� ��

    [Header("===snake Move===")]
    private Vector2 _joystickVec;                       // ���̽�ƽ�� vec 
    /*
    private List<Transform> _markerNowTransform;        // marker ������ ���� ����Ʈ 
    private List<Quaternion> _markerNowQuaternion;      // marker ȸ�� ���� ����Ʈ
    */

    public Vector3 joystickVec { set { _joystickVec = value; } }

    void Start()
    {
        _joystickVec        = new Vector3(1f,0 , 0);
        _isReadToMove       = true;

        // snake ������ (X)
        /*
        _markerNowTransform = new List<Transform>();
        _markerNowQuaternion = new List<Quaternion>();

        for (int i = 0; i < PlayerManager.Instance.markers.Count; i++)
        {
            // ��ġ ����
            _markerNowTransform.Add(PlayerManager.Instance.markers[i].transform);

            // ȸ�� ���� 
            _markerNowQuaternion.Add(PlayerManager.Instance.markers[i].transform.rotation);

        }
        */
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isReadToMove)
        {
            // �Ӹ� ������ 
            F_HeadMoveControl();
        }

    }

    // �Ӹ� ������ ��Ʈ��
    private void F_HeadMoveControl()
    {
        // ���̽�ƽ vector ����
        Vector3 joyVec = new Vector3(_joystickVec.x, 0, _joystickVec.y);

        // ��Ŀ�� ���� ��ġ�� ����
        Vector3 currentPosition = PlayerManager.Instance.markers.transform.position;

        // ���ο� ��ġ ���
        Vector3 newPosition = currentPosition + joyVec * PlayerManager.Instance.markerSpeed * Time.deltaTime;

        // ��Ŀ �̵�
        PlayerManager.Instance.markers.transform.position = newPosition;

        // �̵� ������ 0�� �ƴ� ���� ȸ�� ����
        if (joyVec != Vector3.zero)
        {
            // vec�� �ٶ󺸴� ȸ�� �� ���ϱ� : LookRotation( ���⺤�� , Vector3.up ) 
            Quaternion targetRotation = Quaternion.LookRotation(joyVec);

            // �ε巯�� ȸ�� ����
            PlayerManager.Instance.markers.transform.rotation = Quaternion.Slerp( PlayerManager.Instance.markers.transform.rotation,
                targetRotation, PlayerManager.Instance.markerSpeed * Time.deltaTime); 
        }

    }

    // snake������ 
    /*
    private void F_SnakeBodyMovement()
    {
        for (int i = 1; i < PlayerManager.Instance.markers.Count; i++)
        {
            Transform _currentMarker = PlayerManager.Instance.markers[i].transform;
            Transform _previousMarker = PlayerManager.Instance.markers[i - 1].transform;

            // ���⺤��
            Vector3 _direction = _previousMarker.position - _currentMarker.position;

            // ���⺤�� ���� = ������ �Ÿ� ( ����marker ,  pre marker ������ �Ÿ� )
            float _distance = _direction.magnitude;

            // �Ÿ��� ���� ������ between �Ÿ����� ũ�� -> �̵� ( ������ �̵����� �ʴ´�. �̷������ !! )
            if (_distance > 3f)
            {
                Vector3 newPosition = _previousMarker.position - _direction.normalized * 3f;
                _currentMarker.position = Vector3.Lerp(_currentMarker.position, newPosition, _speed * Time.deltaTime);
            }

            // ȸ��
            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            _currentMarker.rotation = Quaternion.Slerp(_currentMarker.rotation, targetRotation, _speed * Time.deltaTime);
        }

        
        // �̵� + ȸ�� , �Ӹ�����
        for (int i = 1; i < PlayerManager.instance.markers.Count; i++)
        {
            // ���� marker
            Transform _nowMarker = PlayerManager.instance.markers[i].transform;

            // ������
            PlayerManager.instance.markers[i].transform.position = Vector3.Lerp
                (
                    PlayerManager.instance.markers[i].transform.position,
                    _markerNowTransform[i - 1].transform.position,
                    _speed * Time.deltaTime
                );

            // ȸ��
            PlayerManager.instance.markers[i].transform.rotation = Quaternion.Slerp
                (
                    PlayerManager.instance.markers[i].transform.rotation,
                    _markerNowQuaternion[i-1],
                    _speed * Time.deltaTime
                );

        }

        // �迭 �ʱ�ȭ 
        _markerNowTransform.Clear();
        _markerNowQuaternion.Clear();

        // ���� �Ӹ� + ���� ��ġ ��Ƶα�
        for (int i = 0; i < PlayerManager.instance.markers.Count; i++)
        {
            // ��ġ ����
            _markerNowTransform.Add(PlayerManager.instance.markers[i].transform);

            // ȸ�� ���� 
            _markerNowQuaternion.Add(PlayerManager.instance.markers[i].transform.rotation);

        }
        
    }
    */

}
