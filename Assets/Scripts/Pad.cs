using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour
{
    public Vector3 actualPos;
    public bool isGate = false;
    public bool isFree = true;
    private Pawn _pawnCapture;

    public string padName = "defaultPadName";

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

    private void capturedByPawn(Pawn pawn)
    {
        _pawnCapture = pawn;
        isFree = false;
    }

    private Pawn getPawnCaptured()
    {
        return _pawnCapture;
    }
    
}
