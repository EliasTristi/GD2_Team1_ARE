using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MapCameraRotation : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 0.5f;
    [SerializeField]
    private GameObject _lookObject;
    private MapCameraLook _look;

    [HideInInspector]
    public bool _doRotation = false;

    private float _targetRotation = 45;
    private float _timer = 0;

    void Start()
    {
        _look = _lookObject.GetComponent<MapCameraLook>();
        _look.parentRotation = _targetRotation;
    }

    void Update()
    {
        if (_doRotation)
        {
            Rotate(_targetRotation, Vector3.up);
        }
        else
        {
            if (Input.GetButtonDown("RotateCameraLeft"))
            {
                _targetRotation += 90;
                //_look.enabled = false;
                _look.parentRotation = _targetRotation;
                _doRotation = true;

            }
            else if (Input.GetButtonDown("RotateCameraRight"))
            {
                _targetRotation -= 90;
                //_look.enabled = false;
                _look.parentRotation = _targetRotation;
                _doRotation = true;
            }
        }
    }

    private void Rotate(float degrees, Vector3 axis)
    {
        _timer += Time.deltaTime;

        Quaternion newRotation = Quaternion.AngleAxis(degrees, axis);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, _timer);

        if (_timer >= _rotateSpeed)
        {
            _timer = 0;
            _doRotation = false;
            //_look.enabled = true;
        }
    }
}



