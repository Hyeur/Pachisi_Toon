using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public static Dice Instance;

    private Transform[] _faceList;
    
    private Rigidbody _rigidbody;

    private BoxCollider _boxCollider;

    private float _time = .0f;

    private bool _isIdle = false;

    private MeshRenderer _visible;

    public Outline outline;

    void Awake()
    {
        try
        {
            outline = GetComponent<Outline>();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
        _rigidbody = this.GetComponent<Rigidbody>();

        _boxCollider = this.GetComponent<BoxCollider>();

        _visible = this.GetComponent<MeshRenderer>();

        _faceList = GetComponentsInChildren<Transform>();

        Instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        controlStatus();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!SettingManager.Instance) return;
        
        var relativeVelocity = collision.relativeVelocity.magnitude;
        if (relativeVelocity > 20f)
        {
            SettingManager.Instance.playEffect(SettingManager.Instance.audioCatalog.diceImpact1);
        }
        if (relativeVelocity < 20f && relativeVelocity > 10f)
        {
            SettingManager.Instance.playEffect(SettingManager.Instance.audioCatalog.diceImpact2);
        }
    }

    private void rankTheFaces()
    {
        if (_faceList.Length <= 0)
        {
            Debug.Log("There no Face on List!");
            return;
        }
        _faceList = _faceList.OrderBy(o => o.transform.position.y).ToArray();
    }

    public int getTossResult()
    {
        rankTheFaces();
        
        Transform r = _faceList[6];
        int result = int.Parse(r.name);
        
        return result;
    }
    private void setIdle(bool status = false)
    {
        if (_isIdle == status)
            return;
        
        _isIdle = status;
    }

    private Vector3 getVelocity()
    {
        return _rigidbody.velocity;
    }

    private void controlStatus()
    {
        if (getVelocity().magnitude <= 0.2f)
        {
            _time += Time.deltaTime;
            if (_time >= 1.2f)
            {
                setIdle(true);
                _time = 0f;
            }
        }
        else
        {
            setIdle(false);
        }
    }

    public bool isIdle()
    {
        return _isIdle;
    }

    public Rigidbody getRb()
    {
        return _rigidbody;
    }    
    public BoxCollider GetCollider()
    {
        return _boxCollider;
    }

    public void setVisible(bool v)
    {
        _visible.enabled = v;
    }
    
}
