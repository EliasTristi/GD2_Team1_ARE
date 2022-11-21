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
    private Vector3 _startDir;
  
    [SerializeField]
    private float _cameraFollowSpeed = 2.0f;

    Camera _camera;

    private float _screenCenterX = Screen.width / 2;
    private float _screenCenterY = Screen.height / 2;

    [SerializeField]
    private float _followBoundaryX = 55.0f;
    private float _followBoundaryY = 55.0f;

    private float _cameraSpeed = 5.0f;


    void Start()
    {
        rotationPoint = gameObject.transform.position;
       // rotationPoint.z = _playerPos.transform.position.z;
        _startDir = rotationPoint - transform.position;
       _camera = GetComponent<Camera>();
    }
    void Update()
    {
        if (!_doRotation)
        {
            if (Input.GetButtonDown("RotateCameraLeft"))
            {
                //rotationPoint = _playerPos.transform.position;
                //rotationPoint.x = _screenCenterX;
                //rotationPoint.y = _screenCenterY;

                rotationPoint = transform.position;
                if (IsCameraAlongX())
                {
                    rotationPoint.x = _playerPos.transform.position.x;
                }
                else if (IsCameraAlongZ())
                {
                    rotationPoint.z = _playerPos.transform.position.z;
                }
                //old
                //rotationPoint = _playerPos.transform.position ;
      

                

                _startDir = rotationPoint - transform.position;
                _doRotation = true;
                if(_rotationSpeed < 0)
                    _rotationSpeed *= -1;
            }
            else if (Input.GetButtonDown("RotateCameraRight"))
            {

                rotationPoint = transform.position;
                if (IsCameraAlongX())
                {
                    rotationPoint.x = _playerPos.transform.position.x;
                }
                else if (IsCameraAlongZ())
                {
                    rotationPoint.z = _playerPos.transform.position.z;
                }


                //old 
                // rotationPoint = _playerPos.transform.position;

                //rotationPoint = transform.position;
                //rotationPoint.z = _playerPos.transform.position.z;


                _startDir = rotationPoint - transform.position;
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
        float currentAngle = Vector3.Angle(targetDir, _startDir);
  
        


        if (currentAngle >= 90)
        {
            _doRotation = false;
            _startDir = targetDir;

            //set to point
           // if(_rotationSpeed > 0)
            transform.RotateAround(rotationPoint, new Vector3(0, 1, 0), 90 - currentAngle );

            //if(_rotationSpeed < 0)
            //    transform.RotateAround(rotationPoint, new Vector3(0, 1, 0), -(90 - currentAngle));
            return;
        }
        transform.RotateAround(rotationPoint, new Vector3(0, 1, 0), _rotationSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        //if (!_doRotation)
        //{
        //    Vector3 cameraMovementDir = Vector3.zero;
        //    cameraMovementDir.x = Input.GetAxis("CameraFreeMovementHorizontal");
        //    cameraMovementDir.y = Input.GetAxis("CameraFreeMovementVertical");
        //    if (cameraMovementDir.x != 0 || cameraMovementDir.y != 0)
        //    {
        //        HandleFreeMovement(cameraMovementDir);
        //    }
        //    else
        //    {
        //        HandleCameraFollow();
        //    }


        //}

    }


    void HandleCameraFollow()
    {
        Vector2 playerInScreenSpace = _camera.WorldToScreenPoint(_playerPos.transform.position); //Convert playerpos to screenspace

        Vector3 moveDir = Vector3.zero;

        float dx = playerInScreenSpace.x - _screenCenterX;

        if (dx > _followBoundaryX || dx < -_followBoundaryX) //check if outside boundaries
        {
            if (_screenCenterX < playerInScreenSpace.x) //check on which side
            {
                moveDir.x = dx - _followBoundaryX;
            }
            else
            {
                moveDir.x = dx + _followBoundaryX;
            }

        }

        //same for y
        float dy = playerInScreenSpace.y - _screenCenterY;

        if (dy > _followBoundaryY || dy < -_followBoundaryY)
        {
            if (_screenCenterY < playerInScreenSpace.y)
            {
                moveDir.y = dy - _followBoundaryY;
            }
            else
            {
                moveDir.y = dy + _followBoundaryY;
            }

        }

        moveDir = transform.rotation * moveDir; //can't be *= because of quaternion and vector operands


        transform.position += moveDir * Time.deltaTime * _cameraFollowSpeed; //move the camera
    }

    void HandleFreeMovement(Vector3 cameraMovementDir)
    {
       

        print(cameraMovementDir.x);

        cameraMovementDir = transform.rotation * cameraMovementDir; //can't be *= because of quaternion and vector operands

        transform.position += cameraMovementDir * Time.deltaTime * _cameraSpeed; //move the camera



    }

    private bool IsCameraAlongX()
    { 
       return _camera.transform.forward.Abs() == Vector3.right;
    }
    private bool IsCameraAlongZ()
    {
        return _camera.transform.forward.Abs() == Vector3.forward;
    }

}

