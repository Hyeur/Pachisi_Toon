using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelController : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public bool isEnter = false;
    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (SessionManager.Instance.isOpened && !isEnter)
            {
                SessionManager.Instance.toggleSettingPanel();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
    }
}
