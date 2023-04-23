using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    protected bool _active = false;
    void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        _active = state == GameManager.GameState.RollTheDice;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    public void OnMouseDown()
    {
        if (_active)
        {
            DiceManager.Instance.toss(GameManager.Instance.currentTeam);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
