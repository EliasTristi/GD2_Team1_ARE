using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    [SerializeField]
    private GameObject _playerPos;
    [SerializeField]
    private float _rotationSpeed = 40.0f;

    private Vector3 rotationPoint;
    private bool _doRotation = false;
    private Vector3 startDir;
  

    void Start()
    {
        rotationPoint = gameObject.transform.position;
       // rotationPoint.z = _playerPos.transform.position.z;
        startDir = rotationPoint - transform.position;
       
    }
    void Update()
    {
        if (!_doRotation)
        {
            if (Input.GetButtonDown("RotateCameraLeft"))
            {
                
                rotationPoint = _playerPos.transform.position ;
                //rotationPoint.z = _playerPos.transform.position.z;
                startDir = rotationPoint - transform.position;
                _doRotation = true;
                if(_rotationSpeed < 0)
                    _rotationSpeed *= -1;
            }
            else if (Input.GetButtonDown("RotateCameraRight"))
            {
                rotationPoint = _playerPos.transform.position;
                //rotationPoint = transform.position;
                //rotationPoint.z = _playerPos.transform.position.z;
                startDir = rotationPoint - transform.position;
                _doRotation = true;
                if (_rotationSpeed > 0)
                    _rotationSpeed *= -1;
            }

        }

        if (_doRotation)
            Rotate();

    }

    void Rotate()
    {
        Vector3 targetDir = rotationPoint - transform.position;
        float currentAngle = Vector3.Angle(targetDir, startDir);
  
        if (currentAngle >= 90)
        {
            _doRotation = false;
            startDir = targetDir;
            return;
        }
        transform.RotateAround(rotationPoint, new Vector3(0, 1, 0), _rotationSpeed * Time.deltaTime);
    }
}

