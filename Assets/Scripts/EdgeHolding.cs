using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeHolding : MonoBehaviour
{
    [SerializeField]
    private LayerMask _platformMask;

    private CharacterController _player;
    private PlayerMovement _playerMovement;
    private VinesClimbing _vinesClimbing;

    [SerializeField]
    private Transform _cameraTransform;

    private bool _edgeFound = false;
    private bool _HangingOnEdge = false;
    private bool _climbing = false;
    private float _climbInitialHeight;
    private const int _speed = 5;
    private const float _edgeJumpHeight = 1.5f;

    private void Start()
    {
        _player = GetComponent<CharacterController>();
        _playerMovement = GetComponent<PlayerMovement>();
        _vinesClimbing = GetComponent<VinesClimbing>();
    }

    private void Update()
    {
        if (!_climbing)
        {
            if (_HangingOnEdge)
            {
                EdgeMovement();
            }

            if (_edgeFound && Input.GetKey(KeyCode.UpArrow))
            {
                GrabEdge();
            }
        }
        else
        {
            ClimbOnTop();
        }
    }

    private void FixedUpdate()
    {
        FindEdge();
    }

    private void FindEdge()
    {
        Ray ray = new Ray(transform.position - (_cameraTransform.forward * 100), _cameraTransform.forward);

        if(Physics.Raycast(ray, out RaycastHit hit, 1000f, _platformMask) && hit.collider.isTrigger)
        {
            _edgeFound = true;
        }
        else
        {
            _edgeFound = false;
        }
    }

    private void GrabEdge()
    {
        _playerMovement.Velocity = Vector3.zero;
        _playerMovement.enabled = false;
        _vinesClimbing.enabled = false;
        _HangingOnEdge = true;

        Vector3 previousPosition;
        Vector3 currentPosition;
        int whileStopper = 0;
        int whileLimit = 100;
        do
        {
            previousPosition = transform.position;
            _player.Move(_cameraTransform.forward);
            currentPosition = transform.position;

            whileStopper++;

            if (whileStopper >= whileLimit)
            {
                do
                {
                    _player.Move(-_cameraTransform.forward);
                }
                while ((!Physics.Raycast(transform.position + (_cameraTransform.forward * 2) - _cameraTransform.right, _cameraTransform.right, out RaycastHit hit, 3, _platformMask) || !hit.collider.isTrigger) && (!Physics.Raycast(transform.position + (_cameraTransform.forward * 2) + _cameraTransform.right, -_cameraTransform.right, out RaycastHit hit2, 3, _platformMask) || !hit2.collider.isTrigger));

                break;
            }
        }
        while (previousPosition.x != currentPosition.x && previousPosition.z != currentPosition.z);
    }

    private void EdgeMovement()
    {
        Vector3 movementDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementDirection += _cameraTransform.right * _speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementDirection -= _cameraTransform.right * _speed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _climbing = true;
            _climbInitialHeight = transform.position.y;

            _HangingOnEdge = false;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            _playerMovement.enabled = true;
            _vinesClimbing.enabled = true;
            _HangingOnEdge = false;

            _playerMovement.Velocity = -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * _edgeJumpHeight);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || !_edgeFound)
        {
            _playerMovement.enabled = true;
            _HangingOnEdge = false;
        }

        _player.Move(movementDirection);
    }

    private void ClimbOnTop()
    {
        if (transform.position.y < _climbInitialHeight + 1.2f)
        {
            _player.Move(Vector3.up * _speed * Time.deltaTime);
        }
        else
        {
            if (Physics.Raycast(transform.position, _cameraTransform.forward + -Vector3.up, out RaycastHit hit1, 2, _platformMask) && !hit1.collider.isTrigger)
                _player.Move(_cameraTransform.forward);
            else if (Physics.Raycast(transform.position, _cameraTransform.right + -Vector3.up, out RaycastHit hit2, 2, _platformMask) && !hit2.collider.isTrigger)
                _player.Move(_cameraTransform.right);
            else
                _player.Move(-_cameraTransform.right);

            _climbing = false;
            _playerMovement.enabled = true;
            _vinesClimbing.enabled = true;
        }
    }
}
