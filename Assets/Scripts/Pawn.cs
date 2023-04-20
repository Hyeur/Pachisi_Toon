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
    private BoxCollider _boxCollider;
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
            _boxCollider = GetComponent<BoxCollider>();
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

        if (currentPad)
        {
            currentPad.setPawnCaptured(this);
        }
        
        updateOutStatus();

        updateReadyStatus(currentPad);
        
    }

    public Rigidbody getRb()
    {
        return _rigidbody;
    }

    public void setCurrentPad(Pad pad)
    {
        currentPad.setPawnCaptured(null);
        if (pad != null)
        {
            currentPad = pad;
        }
        else
        {
            currentPad = null;
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

    public async void recoveryToCurrentPad()
    {
        if (!_isReady)
        {
            _boxCollider.enabled = false;
            _rigidbody.useGravity = false;
            if (currentPad)
            {
                await transform.DORotate(new Vector3( 0, transform.rotation.y, 0), 1f).SetEase(Ease.InSine).AsyncWaitForCompletion();
                await transform.DOJump(currentPad.actualPos, 1,1,1f).SetEase(Ease.InSine).AsyncWaitForCompletion();
                _boxCollider.enabled = true;
                _rigidbody.useGravity = true;
                // Debug.Log($"Respawn {name} to {currentPad}");
            }
            else
            {
                transform.rotation = Quaternion.Euler(0,0,0);
                await transform.DOMove(Team.transform.position, 1f).SetEase(Ease.InSine).AsyncWaitForCompletion();
                _boxCollider.enabled = true;
                _rigidbody.useGravity = true;
                // Debug.Log($"Respawn {name} to {Team.name}");
            }
        }
    }

    public void sentToHome()
    {
        _boxCollider.enabled = false; 
        _rigidbody.useGravity = false;
        transform.rotation = Quaternion.Euler(0,0,0);
        transform.DOMove(Team.transform.position,.3f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            _boxCollider.enabled = true;
            _rigidbody.useGravity = true;
        });
    }
    
    
}
