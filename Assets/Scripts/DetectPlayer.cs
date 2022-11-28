using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    private RotatingPlatform _rotatingPlatform;
    private CameraBehaviour _cameraBehaviour;
    private GameObject _player;
    private Collider _onPlatform;

    private void Start()
    {
        _rotatingPlatform = GetComponentInParent<RotatingPlatform>();
        _player = GameObject.Find("Player");
        _cameraBehaviour = GameObject.Find("Camera").GetComponent<CameraBehaviour>();
    }

    private void Update()
    {
        if (_onPlatform != null)
            RotateCameraAlong();
    }

    private void RotateCameraAlong()
    {
        if (_onPlatform.name.Equals("Player") && _rotatingPlatform.RotationStarts)
        {
            _cameraBehaviour.DoRotation = true;

            if (IsCameraAlongX())
            {
                _cameraBehaviour.RotationPoint = new Vector3(transform.position.x, _cameraBehaviour.transform.position.y, _cameraBehaviour.transform.position.z);
            }
            else
            {
                _cameraBehaviour.RotationPoint = new Vector3(_cameraBehaviour.transform.position.x, _cameraBehaviour.transform.position.y, transform.position.z);
            }
            _cameraBehaviour.StartDir = _cameraBehaviour.RotationPoint - _cameraBehaviour.transform.position;

            if (_cameraBehaviour.RotationSpeed < 0 && _rotatingPlatform.RotationDirection > 0)
            {
                _cameraBehaviour.RotationSpeed *= -1;
            }
            else if (_cameraBehaviour.RotationSpeed > 0 && _rotatingPlatform.RotationDirection < 0)
            {
                _cameraBehaviour.RotationSpeed *= -1;
            }

            _player.transform.parent = _rotatingPlatform.transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _onPlatform = other;
    }

    private void OnTriggerExit(Collider other)
    {
        _player.transform.parent = null;
        _onPlatform = null;
    }


    private bool IsCameraAlongX()
    {
        return _cameraBehaviour.transform.forward.Abs() == Vector3.right;
    }
}
