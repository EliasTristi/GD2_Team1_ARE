using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public EventHandler<EventArgs> StartRotation;
    public EventHandler<EventArgs> StopRotation;

    [SerializeField]
    private GameObject _playerPos;
    [SerializeField]
    public float RotationSpeed = 180.0f;

    private Vector3 rotationPoint;
    private bool _doRotation = false;

    public bool DoRotation
    {
        get { return _doRotation; }
        set
        {
            _doRotation = value;
            if (value) OnStartRotation(EventArgs.Empty);
            else OnStopRotation(EventArgs.Empty);
        }
    }
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
        if (!DoRotation)
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
                
      

                

                _startDir = rotationPoint - transform.position;
                DoRotation = true;
                if(RotationSpeed < 0)
                    RotationSpeed *= -1;

                //CalculateEndPos();
               // CalcEndPos();
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
                DoRotation = true;
                if (RotationSpeed > 0)
                    RotationSpeed *= -1;
                //CalculateEndPos();
               // CalcEndPos();
            }

        }

        if (DoRotation)
            Rotate();




    }

    Vector3 _endPos;

   bool _hasFreeMoved = false;

    void Rotate()
    {
        Vector3 targetDir = rotationPoint - transform.position;
        float currentAngle = Vector3.Angle(targetDir, _startDir);
  
        


        if (currentAngle >= 90)
        {
         
            print(currentAngle);

            float deltaAngle = currentAngle - 90;

            if (RotationSpeed > 0)
                transform.RotateAround(rotationPoint, new Vector3(0, 1, 0), -deltaAngle);

            if (RotationSpeed < 0)
                transform.RotateAround(rotationPoint, new Vector3(0, 1, 0), deltaAngle);

            // transform.position = _endPos;

            if (IsCameraAlongX())
            {
         
                _startDir = targetDir;
                DoRotation = false; // this triggers the StopRotation event, so it must happen at the laxt step
                return;
            }
            else if (IsCameraAlongZ())
            {
              
                _startDir = targetDir;
                DoRotation = false; // this triggers the StopRotation event, so it must happen at the laxt step
                return;
            }

      
        }
        transform.RotateAround(rotationPoint, new Vector3(0, 1, 0), RotationSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (!DoRotation)
        {
            Vector3 cameraMovementDir = Vector3.zero;
            cameraMovementDir.x = Input.GetAxis("CameraFreeMovementHorizontal");
            cameraMovementDir.y = Input.GetAxis("CameraFreeMovementVertical");
            if (cameraMovementDir.x != 0 || cameraMovementDir.y != 0)
            {
               _hasFreeMoved = true;
            }

            if(_hasFreeMoved)
            {
                HandleFreeMovement(cameraMovementDir);
            }

            //handlefreemovement uses camerafollow to snap back to the player center
            if (!_hasFreeMoved) //allows it to instantly snap back after it goes out of bound
            {
                HandleCameraFollow();
            }
           
            


        }

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

    [SerializeField]
    float _freeMoveBoundaryX = 50;

    [SerializeField]
    float _freeMoveBoundaryY = 50;

    void HandleFreeMovement(Vector3 cameraMovementDir)
    {

        if (cameraMovementDir != Vector3.zero)
        {
            print(cameraMovementDir.x);

            cameraMovementDir = transform.rotation * cameraMovementDir; //can't be *= because of quaternion and vector operands

            transform.position += cameraMovementDir * Time.deltaTime * _cameraSpeed; //move the camera


        }


        Vector2 playerInScreenSpace = _camera.WorldToScreenPoint(_playerPos.transform.position); //Convert playerpos to screenspace

        float boundaryPosLeft = 0 + _freeMoveBoundaryX;
        float boundaryPosRight = Screen.width - _freeMoveBoundaryX;
        float boundaryPosBot = 0 + _freeMoveBoundaryY;
        float boundaryPosTop = Screen.height - _freeMoveBoundaryY;


        if (playerInScreenSpace.x > boundaryPosRight) 
        {

            _hasFreeMoved = false;
            return;

        }
        else if (playerInScreenSpace.x < boundaryPosLeft)
        {
            _hasFreeMoved = false;
            return;
        }

        if (playerInScreenSpace.y > boundaryPosTop)
        {
            _hasFreeMoved = false;
            return;
        }
        else if (playerInScreenSpace.y < boundaryPosBot)
        {
            _hasFreeMoved = false;
            return;
        }





    }





    private bool IsCameraAlongX()
    { 
       return _camera.transform.forward.Abs() == Vector3.right;
    }
    private bool IsCameraAlongZ()
    {
        return _camera.transform.forward.Abs() == Vector3.forward;
    }


    protected virtual void OnStartRotation(EventArgs e)
    {
        var handler = StartRotation;
        handler?.Invoke(this, e);
    }
    protected virtual void OnStopRotation(EventArgs e)
    {
        var handler = StopRotation;
        handler?.Invoke(this, e);
    }
}

