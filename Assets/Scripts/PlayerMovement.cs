using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _playerModel;

    [SerializeField] private float _runningSpeed = 10;
    [SerializeField] private float _jumpHeight = 3f;
    [SerializeField] private float _globalGravity = 9.81f;


    [Space]
    [Header("Debug properties")]

    [SerializeField] private bool _detectCollisions = true;
    [SerializeField] private bool _enableOverlapRecovery = true;
    [SerializeField] private bool _callMoveFunction = true;
    [SerializeField] private bool _showSkinWidth = false;
    [SerializeField] private bool _debugCollisions = false;
    [SerializeField] private bool _debugIsGrounded = false;

    private CharacterController _charCtrl;
    private Transform _camera;
    public Vector3 Velocity;
    private Vector3 _inputVector;
    private Vector3 _jumpForce;
    private bool _jump;
    private bool _freeze = false;

    private void Start()
    {
        _charCtrl = GetComponent<CharacterController>();
        _charCtrl.detectCollisions = _detectCollisions;
        _charCtrl.enableOverlapRecovery = _enableOverlapRecovery;

        _camera = Camera.main.transform;
        _camera.GetComponent<CameraBehaviour>().StartRotation +=
            (s, e) => _freeze = true;
        _camera.GetComponent<CameraBehaviour>().StopRotation +=
            (s, e) => _freeze = false;

        Physics.gravity = new Vector3(0, -_globalGravity, 0);

        // Calculate the force it would take to reach the _jumpheight
        #region Calculate jump force info
        // https://en.wikipedia.org/wiki/Equations_of_motion
        // we find the formula: v ^ 2 = v0 ^ 2 + 2 * a(r - r0) where:
        // v = the final speed = 0
        // v0 = the speed we start with, the value we need to find
        // a = out acceleration = -9.81
        // r = end position = 1
        // r0 = start position = 0
        /* This means we can actually find that v0 = sqrt(2 * 9.81 * 1).Notice we didn't take - 9.81 as value, but +
        9.81.A square root of a negative number would not work out. This leads to the following code. Notice
        how we reset the _jump variable since we want to avoid applying the jump multiple times.*/
        // YOU DO NOT NEED TO MEMORISE THIS!
        #endregion
        _jumpForce = -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
    }

    void Update()
    {
        RegisterInput();
        RotateModel();
    }

    private void FixedUpdate()
    {
        if (_callMoveFunction && !_freeze)
        {
            ApplyGravity();
            ApplyMovement();
            ApplyJump();
            _charCtrl.Move(Velocity * Time.deltaTime);
        }

        if (_debugIsGrounded)
        {
            Debug.Log(_charCtrl.isGrounded);
        }
    }

    private void RegisterInput()
    {
        _inputVector.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
            _jump = true;
    }
    private void RotateModel()
    {
        float rotation = 0;
        if (_inputVector.x < 0) rotation += 90;
        else if (_inputVector.x > 0) rotation -= 90;

        _playerModel.rotation = _camera.rotation * Quaternion.Euler(0, rotation, 0);
    }

    private void ApplyMovement()
    {
        // Move relative to camera rotation
        Vector3 horizontalMovement = _camera.rotation * _inputVector * _runningSpeed;

        Velocity.x = horizontalMovement.x;
        Velocity.z = horizontalMovement.z;
    }

    private void ApplyGravity()
    {
        if( _charCtrl.isGrounded)
        {
            Velocity.y = Physics.gravity.y * _charCtrl.skinWidth;
        }
        else
        {
            // v = a * t1-t0
            Velocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    private void ApplyJump()
    {
        if( _jump && _charCtrl.isGrounded)
        {
            Velocity = _jumpForce;
        }
        _jump = false;
    }

    #region For debugging purpose
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!_debugCollisions) return;
        Debug.Log("ControllerColliderHit - CollisionFlag: " + _charCtrl.collisionFlags);
        Debug.Log("ControllerColliderHit - Collider: " + hit.collider.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_debugCollisions) return;
        Debug.Log("Collision Enter: " + collision.collider.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_debugCollisions) return;
        Debug.Log("Trigger Enter: " + other.name);
    }

    private void OnDrawGizmos()
    {
        if (_charCtrl == null || !_showSkinWidth) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, _charCtrl.skinWidth + _charCtrl.radius);
    }

    private void OnValidate()
    {
        _charCtrl ??= GetComponent<CharacterController>();
        _charCtrl.detectCollisions = _detectCollisions;
        _charCtrl.enableOverlapRecovery = _enableOverlapRecovery;
    }
    #endregion
}
