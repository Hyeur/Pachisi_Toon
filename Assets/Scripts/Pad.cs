using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour
{
    public Vector3 actualPos;
    public bool isGate = false;
    public bool isFree = true;
    [SerializeField] private Pawn _pawnCapture;

    public string padName = "defaultPadName";
    public string padTeam;

    public Outline outline;
    
    

    public Pad(Vector3 actualPos, bool isGate, bool isFree, Pawn pawnCapture, string padName)
    {
        this.actualPos = actualPos;
        this.isGate = isGate;
        this.isFree = isFree;
        _pawnCapture = pawnCapture;
        this.padName = padName;
    }

    void Awake()
    {
        actualPos = transform.position;
        setPadName(gameObject.name);
        padTeam = transform.parent.name.Substring(0, 5);
    }

    private void Update()
    {
        updateFreeStatus();
    }

    private void updateFreeStatus()
    {
        if (_pawnCapture)
        {
            if (isFree)
            {
                isFree = false;
            }
        }
        else
        {
            if (!isFree)
            {
                isFree = true;
            }
        }
    }

    public void setPadName(string name)
    {
        if (name.Length != 0)
        {
            padName = name;
        }
        
        try
        {
            outline = GetComponent<Outline>();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    public void setPawnCaptured(Pawn pawn = null)
    {
        _pawnCapture = pawn;
    }

    public Pawn getPawnCaptured()
    {
        return _pawnCapture;
    }
    
    
}
