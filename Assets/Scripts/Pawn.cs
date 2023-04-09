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
    [SerializeField] public bool _isFinish;

    [SerializeField] protected Pad currentPad;

    public GameObject Team;
    
    private Rigidbody _rigidbody;
    void Start()
    {

        _isOut = false;
        _isReady = false;
        try
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }
    
    void Update()
    {
        if (_isOut)
        {
            updateReadyStatus();
        }
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
                Debug.Log("_isReady = false");
            }
        }
        else
        {
            if (!_isReady)
            {
                _isReady = true;
                Debug.Log("_isReady = true");
            }
        }
    }

    public void recoveryToCurrentPad()
    {
        if (!_isReady)
        {
            this.transform.rotation = Quaternion.Euler(0,0,0);
            this.transform.position = currentPad.transform.position;
        }
    }

    public void respawn()
    {
        currentPad = null;
        transform.position = Team.transform.position;
    }
    
}
