using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _cameraPivot;

    [SerializeField] private float _runningSpeed = 10;
    [SerializeField] private float _jumpHeight = 3f;
    [SerializeField] private float _globalGravity = 9.81f;

    [SerializeField] private bool _detectCollisions = true;
    [SerializeField] private bool _enableOverlapRecovery = true;

    [Space]
    [Header("Debug properties")]

    [SerializeField] private bool _callMoveFunction = true;
    [SerializeField] private bool _showSkinWidth = false;
    [SerializeField] private bool _debugOnControllerColliderHit = false;
    [SerializeField] private bool _debugIsGrounded = false;

    private CharacterController _charCtrl = null;
    private Vector3 _velocity;
    private Vector3 _inputVector;
    private Vector3 _jumpForce;
    private bool _jump;

    private void Start()
    {
        _charCtrl = GetComponent<CharacterController>();
        _charCtrl.detectCollisions = _detectCollisions;
        _charCtrl.enableOverlapRecovery = _enableOverlapRecovery;

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
        _inputVector.x = Input.GetAxis("Horizontal");

        if ( Input.GetButtonDown("Jump"))
        {
            _jump = true;
        }
    }

    private void FixedUpdate()
    {
        if (_callMoveFunction)
        {
            ApplyGravity();
            ApplyMovement();
            ApplyJump();
            _charCtrl.Move(_velocity * Time.deltaTime);
        }

        if (_debugIsGrounded)
        {
            Debug.Log(_charCtrl.isGrounded);
        }
    }

    private void ApplyMovement()
    {
        // Move relative to camera rotation
        Vector3 horizontalMovement = _cameraPivot.transform.rotation * _inputVector * _runningSpeed;

        _velocity.x = horizontalMovement.x;
        _velocity.z = horizontalMovement.z;
    }

    private void ApplyGravity()
    {
        if( _charCtrl.isGrounded)
        {
            _velocity.y = Physics.gravity.y * _charCtrl.skinWidth;
        }
        else
        {
            // v = a * t1-t0
            _velocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    private void ApplyJump()
    {
        if( _jump && _charCtrl.isGrounded)
        {
            _velocity = _jumpForce;
        }
        _jump = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!_debugOnControllerColliderHit) return;

        Debug.Log("CollisionFlag: " + _charCtrl.collisionFlags);
        Debug.Log("Collider: " + hit.collider.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter: " + collision.collider.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter: " + other.name);
    }

    #region For debugging purpose
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
