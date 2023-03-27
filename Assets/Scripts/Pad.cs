using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour
{
    public Vector3 actualPos;
    public bool isGate = false;
    public bool isSlot = false;
    public string padName = "defaultPadName";
    

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
    }

}
