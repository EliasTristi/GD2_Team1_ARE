using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatablePlatform : MonoBehaviour, IInteractible
{
    private float _rotationSpeed = 90;
    private float _rotationAngle = 90;
    private int _rotationDirection;
    private float _rotationAngleDone;

    private Transform _cameraTransform;

    public GameObject Interaction
    {
        set
        {
            InitiateRotation(value);
        }
    }

    private void Start()
    {
        _cameraTransform = GameObject.Find("Camera").transform;
    }

    private void Update()
    {
        if (_rotationDirection != 0)
            RotatePlatform();
    }

    private void RotatePlatform()
    {
        float rotation = _rotationSpeed * Time.deltaTime;

        if (_rotationAngleDone + rotation <= _rotationAngle)
        {
            transform.Rotate(Vector3.up, rotation * _rotationDirection);

            _rotationAngleDone += rotation;
        }
        else
        {
            transform.Rotate(Vector3.up, (_rotationAngle - _rotationAngleDone) * _rotationDirection);
            _rotationDirection = 0;
            _rotationAngleDone = 0;
        }
    }

    private void InitiateRotation(GameObject user)
    {
        if (IsCameraAlongX())
        {
            if(user.transform.position.z > transform.position.z)
            {
                _rotationDirection = 1;
            }
            else
            {
                _rotationDirection = -1;
            }
        }
        else if(IsCameraAlongZ())
        {
            if (user.transform.position.x < transform.position.x)
            {
                _rotationDirection = 1;
            }
            else
            {
                _rotationDirection = -1;
            }
        }
        else if (IsCameraAgainstZ())
        {
            if (user.transform.position.x > transform.position.x)
            {
                _rotationDirection = 1;
            }
            else
            {
                _rotationDirection = -1;
            }
        }
        else
        {
            if (user.transform.position.z < transform.position.z)
            {
                _rotationDirection = 1;
            }
            else
            {
                _rotationDirection = -1;
            }
        }

    }

    private bool IsCameraAlongX()
       => _cameraTransform.forward == Vector3.right;
    private bool IsCameraAlongZ()
       => _cameraTransform.forward == Vector3.forward;
    private bool IsCameraAgainstZ()
       => _cameraTransform.forward == -Vector3.forward;
}
