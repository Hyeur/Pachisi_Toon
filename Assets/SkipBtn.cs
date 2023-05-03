using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipBtn : BaseButton
{
    private void Awake()
    {
        GameManager.OnBeforeGameStateChanged += beforeGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnBeforeGameStateChanged -= beforeGameStateChanged;
    }

    private void beforeGameStateChanged(GameManager.GameState state)
    {
        this._button.interactable = (state == GameManager.GameState.PickAPawn);
    }

    protected override void OnClick()
    {
        GameManager.Instance.skipTurn();
    }
}
