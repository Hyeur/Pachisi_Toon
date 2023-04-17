using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] public bool _isOut = false;
    
    [SerializeField] public bool _isReady;
    [SerializeField] public bool _isFinish = false;
    [SerializeField] public bool _isMoving = false;
    [SerializeField] public bool _isMoved = false;
    private Rigidbody _rigidbody;
    [SerializeField] protected Pad currentPad;

    public Team Team;

    public Outline outline;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        _rigidbody.freezeRotation = !(state == GameManager.GameState.RollTheDice);
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    void Start()
    {
        _isOut = false;
        _isReady = false;
        try
        {
            outline = GetComponent<Outline>();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
        try
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.freezeRotation = true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }

        Team = GetComponentInParent<Team>();

    }
    
    void FixedUpdate()
    {
        updateOutStatus();

        updateReadyStatus(currentPad);
        
    }

    public Rigidbody getRb()
    {
        return _rigidbody;
    }

    public void setCurrentPad(Pad pad)
    {
        if (pad != null)
        {
            currentPad = pad;
        }
    }
    
    public Pad getCurrentPad()
    {
        return currentPad;
    }
    
    public void setPawnStatus(bool isOut)
    {
        if (!_isOut.Equals(isOut))
        {
            _isOut = isOut;
        }
        
    }
    private void updateReadyStatus(Pad _currentPad)
    {
        if (!_currentPad)
        {
            _isReady = false;
            return;
        }
        if ( (_currentPad.actualPos - transform.position).sqrMagnitude > Mathf.Pow(0.3f,0.3f) || Mathf.Abs(transform.rotation.eulerAngles.x) > 10 || Mathf.Abs(transform.rotation.eulerAngles.z) > 10)
        {
            if (_isReady)
            {
                _isReady = false;
            }
        }
        else
        {
            if (!_isReady)
            {
                _isReady = true;
            }
        }
    }

    private void updateOutStatus()
    {
        _isOut = currentPad ? true : false;
    }

    public void recoveryToCurrentPad()
    {
        if (!_isReady)
        {
            if (currentPad)
            {
                transform.DORotate(new Vector3( 0, transform.rotation.y, 0), 1f).SetEase(Ease.InSine);
                transform.DOJump(currentPad.actualPos, 1,1,1f).SetEase(Ease.InSine);
                // Debug.Log($"Respawn {name} to {currentPad}");
            }
            else
            {
                transform.rotation = Quaternion.Euler(0,0,0);
                transform.DOMove(Team.transform.position, 1f).SetEase(Ease.InSine);
                // Debug.Log($"Respawn {name} to {Team.name}");
            }
        }
    }
    
    
}
