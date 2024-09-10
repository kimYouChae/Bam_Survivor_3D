using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform _lever;
    private RectTransform _rectTransform;

    [SerializeField, Range(10f, 150f)]
    private float _levelRange;

    private Vector2 _inputVector;
    private bool _isInput;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        
    }

    private void Update()
    {
        if (_isInput) 
        {
            PlayerManager.instance.markerMovement.joystickVec = _inputVector;

        }
    }

    // 드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        F_ControlJoystickLevel(eventData);

         _isInput = true;
    }

    // 드래그
    public void OnDrag(PointerEventData eventData)
    {
        F_ControlJoystickLevel(eventData);

    }

    // 드래그 끝
    public void OnEndDrag(PointerEventData eventData)
    {

        // 드래그 때면 level은 0,0,0으로
        _lever.anchoredPosition = Vector2.zero;
        
        _isInput = false;
    }

    public void F_ControlJoystickLevel(PointerEventData v_Data ) 
    {
        // 드래그 시작한 그 위치로 level 이동
        Vector2 inputDir = v_Data.position - _rectTransform.anchoredPosition;

        // level이 joystick을 안 넘어가게 
        Vector2 clampedDir = inputDir.magnitude < _levelRange ?
            inputDir : inputDir.normalized * _levelRange;

        _lever.anchoredPosition = clampedDir;

        // levelRange를 곱해놔서 clamp는 너무큼 
        _inputVector = clampedDir / _levelRange;
    }
}
