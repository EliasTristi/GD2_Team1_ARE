using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinesClimbing : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraTransform;

    [SerializeField]
    private LayerMask _allExceptZTriggerAndDefault;

    private CharacterController _player;
    private PlayerMovement _playerMovement;
    private EdgeHolding _edgeHolding;

    private Transform _vineFound = null;
    private bool _isClimbing;

    private const int _speed = 5;
    private const float _vineJumpHeight = 1.5f;

    private void Start()
    {
        _player = GetComponent<CharacterController>();
        _playerMovement = GetComponent<PlayerMovement>();
        _edgeHolding = GetComponent<EdgeHolding>();
    }

    private void Update()
    {
        if(!_isClimbing && _vineFound != null && Input.GetKeyDown(KeyCode.UpArrow))
        {
            GrabVines();
        }
    }

    private void FixedUpdate()
    {
        FindVines();

        if (_isClimbing)
        {
            VineMovement();
        }
    }

    private void GrabVines()
    {
        _isClimbing = true;
        _playerMovement.Velocity = Vector3.zero;
        _playerMovement.enabled = false;
        _edgeHolding.enabled = false;
    }

    private void VineMovement()
    {
        Ray ray = new Ray(transform.position - (_cameraTransform.forward * 100), _cameraTransform.forward);
        Vector3 moveVector = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveVector += -_speed * _cameraTransform.right;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveVector += _speed * _cameraTransform.right;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveVector += -_speed * Vector3.up;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveVector += _speed * Vector3.up;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            _playerMovement.enabled = true;
            _edgeHolding.enabled = true;
            _playerMovement.Velocity = -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * _vineJumpHeight);
        }

        _player.Move(moveVector * Time.fixedDeltaTime);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _allExceptZTriggerAndDefault) && hit.collider.gameObject.name.Contains("VineSquare"))
        {
            if (IsCameraAlongX())
            {
                transform.position = new Vector3(hit.transform.position.x + 0.2f, transform.position.y, transform.position.z);
            }
            if (IsCameraAlongZ())
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, hit.transform.position.z - 0.2f);
            }
        }
    }

    private void FindVines()
    {
        Ray ray = new Ray(transform.position - (_cameraTransform.forward * 100), _cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _allExceptZTriggerAndDefault) && hit.collider.gameObject.name.Contains("VineSquare"))
        {
            _vineFound = hit.transform;
        }
        else
        {
            _vineFound = null;

            _isClimbing = false;
            _playerMovement.enabled = true;
            _edgeHolding.enabled = true;
        }
    }

    private bool IsCameraAlongX()
        => _cameraTransform.forward.Abs() == Vector3.right;
    private bool IsCameraAlongZ()
        => _cameraTransform.forward.Abs() == Vector3.forward;
}
