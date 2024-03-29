using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UndoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    float _holdDuration = 0.5f;
    float _holdTime = 0;
    public void OnPointerDown(PointerEventData eventData) => _holdTime = Time.time;


    public void OnPointerUp(PointerEventData eventData)
    {
        UIManager.Instance.Undo();
        _holdTime = 0;
    }

    private void Update()
    {
        if (_holdTime != 0 && Time.time - _holdTime > _holdDuration) UIManager.Instance.UndoAll();
    }
}
