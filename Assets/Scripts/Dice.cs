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

    private float _time = .0f;

    private bool _isIdle = false;

    private MeshRenderer _visible;

    void Awake()
    {
        _rigidbody = this.GetComponent<Rigidbody>();

        _visible = this.GetComponent<MeshRenderer>();

        _faceList = GetComponentsInChildren<Transform>();

        Dice.Instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        controlStatus();
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

    public float getTossResult()
    {
        rankTheFaces();
        
        Transform r = _faceList[6];
        float result = float.Parse(r.name);
        
        return result;
    }
    private void setIdle(bool status = false)
    {
        if (_isIdle == status)
            return;
        
        if (status == true)
            Debug.Log("Result " + getTossResult());
        
        _isIdle = status;
        
        Debug.Log(_isIdle);
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

    public void setVisible(bool v)
    {
        _visible.enabled = v;
    }
    
}
