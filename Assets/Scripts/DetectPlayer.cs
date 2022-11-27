using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    private RotatingPlatform _rotatingPlatform;
    private CameraBehaviour _cameraBehaviour;
    private GameObject _player;

    private void Start()
    {
        _rotatingPlatform = GetComponentInParent<RotatingPlatform>();
        _player = GameObject.Find("Player");
        _cameraBehaviour = GameObject.Find("Camera").GetComponent<CameraBehaviour>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Equals("Player") && _rotatingPlatform.RotationStarts)
        {
            _cameraBehaviour.DoRotation = true;
            if (_cameraBehaviour.RotationSpeed < 0 && _rotatingPlatform.RotationDirection > 0) 
            {
                _cameraBehaviour.RotationSpeed *= -1; 
            }
            else if(_cameraBehaviour.RotationSpeed > 0 && _rotatingPlatform.RotationDirection < 0)
            {
                _cameraBehaviour.RotationSpeed *= -1;
            }
            
            _player.transform.parent = _rotatingPlatform.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _player.transform.parent = null;
    }
}
