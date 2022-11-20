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
  
    [SerializeField]
    private float _cameraFollowSpeed = 2.0f;

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

    private void FixedUpdate()
    {
        if(!_doRotation)
        HandleMovement();
    }

    void HandleMovement() 
    {
        //source: https://www.youtube.com/watch?v=Gwc4VCGEuBM

        float boundX = 2.0f;
        float boundY = 2.0f;
        float boundZ = 2.0f;

        Vector3 moveDirection = Vector3.zero;

        float dx = _playerPos.transform.position.x - transform.position.x;

        if (dx > boundX || dx < -boundX)
        {
            //if (transform.position.x < _playerPos.transform.position.x)
            //{
            //    moveDirection.x = dx - boundX;
            //}
            //else
            //{
            //    moveDirection.x = dx + boundX;
            //}

        }



        float dy = _playerPos.transform.position.y - transform.position.y;

        if (dy > boundY || dy < -boundY)
        {
          



        }

        float dz = _playerPos.transform.position.z - transform.position.z;

        if (dz > boundZ || dz < -boundZ) //check if outside of the boundary
        {
            print("OK");
            if (transform.position.z < _playerPos.transform.position.z)
            {
                moveDirection.z = dz - boundZ;
            }
            else
            {
                moveDirection.z = dz + boundZ;
            }
        }

        transform.position += moveDirection * Time.deltaTime * _cameraFollowSpeed;
    }
}

