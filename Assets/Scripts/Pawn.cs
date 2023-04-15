using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] public bool _isOut = false;
    
    [SerializeField] public bool _isReady;
    [SerializeField] public bool _isFinish = false;
    [SerializeField] public bool _isMoving = false;
    private Rigidbody _rigidbody;
    [SerializeField] protected Pad currentPad;

    public Team Team;

    public Outline outline;
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
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }
    
    void Update()
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
    public int getCurrentPadIndex()
    {
        return PadManager.Instance.map.FirstOrDefault(key => key.Value == currentPad).Key;
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
        if (!_currentPad) return;
        if ( (_currentPad.actualPos - transform.position).sqrMagnitude > Mathf.Pow(0.3f,0.3f))
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
                transform.rotation = Quaternion.Euler(0,0,0);
                transform.position = currentPad.transform.position;
                Debug.Log($"Respawn {name} to {currentPad}");
            }
            else
            {
                transform.rotation = Quaternion.Euler(0,0,0);
                transform.position = Team.transform.position;
                Debug.Log($"Respawn {name} to {Team.name}");
            }
        }
    }
    
    
}
