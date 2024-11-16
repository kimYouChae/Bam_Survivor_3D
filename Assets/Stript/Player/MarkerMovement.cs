using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerMovement : MonoBehaviour
{
    [Header("===snake State===")]
    [SerializeField] private float _speed;                               // 머리 속도
    [SerializeField] private bool _isReadToMove;                         // 움직일 준비가 된
    [SerializeField] private float _betweenMarkerDistnace;                   // marker 사이에 

    [Header("===snake Move===")]
    private Vector2 _joystickVec;                       // 조이스틱의 vec 
    private List<Transform> _markerNowTransform;        // marker 움직임 위한 리스트 
    private List<Quaternion> _markerNowQuaternion;      // marker 회전 위한 리스트

    public Vector3 joystickVec { set { _joystickVec = value; } }

    void Start()
    {
        _speed              = 3f;
        _joystickVec        = new Vector3(1f,0 , 0);
        _isReadToMove       = true;
        _betweenMarkerDistnace  = 1f;

        _markerNowTransform = new List<Transform>();
        _markerNowQuaternion = new List<Quaternion>();

        for (int i = 0; i < PlayerManager.Instance.markers.Count; i++)
        {
            // 위치 저장
            _markerNowTransform.Add(PlayerManager.Instance.markers[i].transform);

            // 회전 저장 
            _markerNowQuaternion.Add(PlayerManager.Instance.markers[i].transform.rotation);

        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isReadToMove)
        {
            // 머리 움직임 
            F_HeadMoveControl();

            // 몸통 움직임 
            F_SnakeBodyMovement();
        }

    }

    // 머리 움직임 컨트롤
    private void F_HeadMoveControl()
    {
        // 조이스틱 vector 수정
        Vector3 joyVec = new Vector3(_joystickVec.x, 0, _joystickVec.y);

        // 마커의 현재 위치를 저장
        Vector3 currentPosition = PlayerManager.Instance.markers[0].transform.position;

        // 새로운 위치 계산
        Vector3 newPosition = currentPosition + joyVec * _speed * Time.deltaTime;

        // 마커 이동
        PlayerManager.Instance.markers[0].transform.position = newPosition;

        // 이동 방향이 0이 아닐 때만 회전 적용
        if (joyVec != Vector3.zero)
        {
            // vec를 바라보는 회전 값 구하기 : LookRotation( 방향벡터 , Vector3.up ) 
            Quaternion targetRotation = Quaternion.LookRotation(joyVec);

            // 부드러운 회전 적용
            PlayerManager.Instance.markers[0].transform.rotation = Quaternion.Slerp( PlayerManager.Instance.markers[0].transform.rotation,
                targetRotation, _speed * Time.deltaTime); 
        }

    }

    private void F_SnakeBodyMovement()
    {
        for (int i = 1; i < PlayerManager.Instance.markers.Count; i++)
        {
            Transform _currentMarker = PlayerManager.Instance.markers[i].transform;
            Transform _previousMarker = PlayerManager.Instance.markers[i - 1].transform;

            // 방향벡터
            Vector3 _direction = _previousMarker.position - _currentMarker.position;

            // 방향벡터 길이 = 벡터의 거리 ( 현재marker ,  pre marker 사이의 거리 )
            float _distance = _direction.magnitude;

            // 거리가 내가 지정한 between 거리보다 크면 -> 이동 ( 작으면 이동하지 않는다. 이런방법이 !! )
            if (_distance > _betweenMarkerDistnace)
            {
                Vector3 newPosition = _previousMarker.position - _direction.normalized * _betweenMarkerDistnace;
                _currentMarker.position = Vector3.Lerp(_currentMarker.position, newPosition, _speed * Time.deltaTime);
            }

            // 회전
            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            _currentMarker.rotation = Quaternion.Slerp(_currentMarker.rotation, targetRotation, _speed * Time.deltaTime);
        }

        /*
        // 이동 + 회전 , 머리제외
        for (int i = 1; i < PlayerManager.instance.markers.Count; i++)
        {
            // 현재 marker
            Transform _nowMarker = PlayerManager.instance.markers[i].transform;

            // 움직임
            PlayerManager.instance.markers[i].transform.position = Vector3.Lerp
                (
                    PlayerManager.instance.markers[i].transform.position,
                    _markerNowTransform[i - 1].transform.position,
                    _speed * Time.deltaTime
                );

            // 회전
            PlayerManager.instance.markers[i].transform.rotation = Quaternion.Slerp
                (
                    PlayerManager.instance.markers[i].transform.rotation,
                    _markerNowQuaternion[i-1],
                    _speed * Time.deltaTime
                );

        }

        // 배열 초기화 
        _markerNowTransform.Clear();
        _markerNowQuaternion.Clear();

        // 현재 머리 + 몸통 위치 담아두기
        for (int i = 0; i < PlayerManager.instance.markers.Count; i++)
        {
            // 위치 저장
            _markerNowTransform.Add(PlayerManager.instance.markers[i].transform);

            // 회전 저장 
            _markerNowQuaternion.Add(PlayerManager.instance.markers[i].transform.rotation);

        }
        */
    }
}
