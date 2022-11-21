using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UIElements;

public class MapCameraRotation : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 0.1f;

    private bool _doRotation = false;
    private float _targetRotation;
    private float _timer = 0;

    void Start()
    {

    }

    void Update()
    {
        if (_doRotation)
        {
            Rotate(_targetRotation);
        }
        else
        {
            if (Input.GetButtonDown("RotateCameraLeft"))
            {
                _targetRotation += 90;
                _doRotation = true;

            }
            else if (Input.GetButtonDown("RotateCameraRight"))
            {
                _targetRotation += -90;
                _doRotation = true;
            }
        }
    }

    private void Rotate(float degrees)
    {
        _timer += Time.deltaTime;

        Quaternion newRotation = Quaternion.AngleAxis(degrees, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, _timer);

        if (_timer >= _rotateSpeed)
        {
            _timer = 0;
            _doRotation = false;
        }
    }
}



