using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour
{

    [Header("Base Button")]
    [SerializeField]
    protected Button _button;
    
    protected virtual void Start()
    {
        loadButton();
        AddOnClickEvent();
    }
    protected virtual void loadButton()
    {
        if (_button != null) return;
        _button = gameObject.GetComponent<Button>();
    }

    protected virtual void AddOnClickEvent()
    {
        _button.onClick.AddListener(OnClick);
        Debug.Log("Added listener");
    }

    protected abstract void OnClick();
}
