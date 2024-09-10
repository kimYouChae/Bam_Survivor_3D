using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerMovement : MonoBehaviour
{
    [Header("===snake State===")]
    [SerializeField] private float _speed;                               // 머리 속도
    [SerializeField] private bool _isReadToMove;                         // 움직일 준비가 된

    [Header("===snake Move===")]
    private Vector2 _joystickVec;                       // 조이스틱의 vec 
    private List<Transform> _markerNowTransform;        // marker 움직임 위한 리스트 

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
        // y는 조이스틱이 위를 향할 때 (0보다 클 때) ,  아래를 향할 때 (0보다 작을 때) 로 나뉨
        Vector3 _joyVec = new Vector3(_joystickVec.x, 0 ,_joystickVec.y > 0 ? 1f : -1f);

        // head 움직이기 
        PlayerManager.instance.markers[0].gameObject.transform.Translate
            (_joyVec * _speed * Time.deltaTime);

    }

    private void F_SnakeBodyMovement()
    {
        // 배열 초기화 
        _markerNowTransform.Clear();

        // 현재 머리 + 몸통 위치 담아두기
        for (int i = 0; i < PlayerManager.instance.markers.Count; i++)
        {
            _markerNowTransform.Add(PlayerManager.instance.markers[i].transform);
        }

        // 이동 , 머리제외
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
