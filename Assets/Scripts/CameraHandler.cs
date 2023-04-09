using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public bool isRotating = false;

    [SerializeField] private float rotationSpeed = 0.2f;
    private Vector3 _prevPosition;

    private float[] _anchors = { 0, 90, 180, -90 };
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            _prevPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 direction = Input.mousePosition - _prevPosition;
            
            transform.Rotate(0, direction.x * rotationSpeed, 0);
            
            _prevPosition = Input.mousePosition;
        }

        
    }


}
