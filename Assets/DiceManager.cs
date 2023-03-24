using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private float _time = .0f;

    private bool _isIdle = false;

    private int _result = 0;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        controlStatus();
    }

    private void setIdle(bool status = false)
    {
        if (_isIdle == status)
            return;
        
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
}
