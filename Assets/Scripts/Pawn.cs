using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] protected bool _isOut = false;
    
    [SerializeField] protected bool _isReady;

    [SerializeField] protected Pad currentPad;
    
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

    // Update is called once per frame
    void Update()
    {
        if (_isOut)
        {
            updateReadyStatus();
        }
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

    public void recovery()
    {
        if (!_isReady)
        {
            this.transform.rotation = Quaternion.Euler(0,0,0);
            this.transform.position = currentPad.transform.position;
        }
    }
    
    
    
}
