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

    // �巡�� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        F_ControlJoystickLevel(eventData);

         _isInput = true;
    }

    // �巡��
    public void OnDrag(PointerEventData eventData)
    {
        F_ControlJoystickLevel(eventData);

    }

    // �巡�� ��
    public void OnEndDrag(PointerEventData eventData)
    {

        // �巡�� ���� level�� 0,0,0����
        _lever.anchoredPosition = Vector2.zero;
        
        _isInput = false;
    }

    public void F_ControlJoystickLevel(PointerEventData v_Data ) 
    {
        // �巡�� ������ �� ��ġ�� level �̵�
        Vector2 inputDir = v_Data.position - _rectTransform.anchoredPosition;

        // level�� joystick�� �� �Ѿ�� 
        Vector2 clampedDir = inputDir.magnitude < _levelRange ?
            inputDir : inputDir.normalized * _levelRange;

        _lever.anchoredPosition = clampedDir;

        // levelRange�� ���س��� clamp�� �ʹ�ŭ 
        _inputVector = clampedDir / _levelRange;
    }
}
