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

    Camera _camera;

    void Start()
    {
        rotationPoint = gameObject.transform.position;
       // rotationPoint.z = _playerPos.transform.position.z;
        startDir = rotationPoint - transform.position;
       _camera = GetComponent<Camera>();
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

    private float _screenCenterX = Screen.width / 2;
    private float _screenCenterY = Screen.height / 2;

    [SerializeField]
    private float _followBoundaryX = 55.0f;
    private float _followBoundaryY = 55.0f;

    void HandleMovement()
    {
        Vector2 playerInScreenSpace = _camera.WorldToScreenPoint(_playerPos.transform.position); //Convert playerpos to screenspace

        Vector3 moveDirection = Vector3.zero;

        float dx = playerInScreenSpace.x - _screenCenterX;

        if (dx > _followBoundaryX || dx < -_followBoundaryX) //check if outside boundaries
        {
            if (_screenCenterX < playerInScreenSpace.x) //check on which side
            {
                moveDirection.x = dx - _followBoundaryX;
            }
            else
            {
                moveDirection.x = dx + _followBoundaryX;
            }

        }

        //same for y
        float dy = playerInScreenSpace.y - _screenCenterY;

        if (dy > _followBoundaryY || dy < -_followBoundaryY)
        {
            if (_screenCenterY < playerInScreenSpace.y)
            {
                moveDirection.y = dy - _followBoundaryY;
            }
            else
            {
                moveDirection.y = dy + _followBoundaryY;
            }

        }

        moveDirection = transform.rotation * moveDirection ;


        transform.position += moveDirection * Time.deltaTime * _cameraFollowSpeed;
    }


}

