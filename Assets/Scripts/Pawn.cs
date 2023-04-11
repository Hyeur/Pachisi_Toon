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
    void Start()
    {

        _isOut = false;
        _isReady = false;
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
        if (currentPad)
        {
            updateReadyStatus();
        }
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
    private void updateReadyStatus()
    {
        if ( Vector3.Distance(currentPad.transform.position,this.transform.position) > 0.3f)
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
