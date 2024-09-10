using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUi : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private int _cardIndex;

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.cardSelectUi.F_SetCardIndex(_cardIndex);
    }
}
