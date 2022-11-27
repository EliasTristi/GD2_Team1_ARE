using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    private const int _rotationAngle = 90;
    private const float _rotationSpeed = 40f;
    [SerializeField]
    private float _rotationFrequency = 3f;
    private float _delay;
    [SerializeField]
    private int _rotationDirection = 1;  // 1 0r -1 == left or right
    public int RotationDirection { get { return _rotationDirection; } } 

    private bool _isRotating = false;
    public bool RotationStarts { get; private set; }
    public bool IsRotating { get { return _isRotating; } }

    private float _rotationAngleDone;

    private GameObject _camera;

    private void Start()
    {
        _camera = GameObject.Find("Camera");


        Transform playerDetector = null;
        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms)
        {
            if (t.name.Equals("PlayerDetector"))
            {
                playerDetector = t;
            }
        }

        playerDetector.GetComponent<BoxCollider>().size = new Vector3(1 + ((1/transform.localScale.x)), 1 + ((1 / transform.localScale.y) / 5), 1 + ((1 / transform.localScale.z)));
    }

    private void FixedUpdate()
    {
        if (_isRotating)
            RotatePlatform();
        else
            Wait();

        if (_rotationDirection != 1 && _rotationDirection != -1)
            Debug.LogWarning("RotationDirection must have a value of -1 or 1 (=Right or left). entering an other value will change its rotation speed, desynchronizing its speed with the camera's");
    }

    private void Wait()
    {
        if (_delay < _rotationFrequency)
            _delay += Time.fixedDeltaTime;
        else
        {
            _delay -= _delay;
            _isRotating = true;
            RotationStarts = true;
        }
    }

    private void RotatePlatform()
    {
        RotationStarts = false;

        float rotation = _rotationSpeed * Time.fixedDeltaTime;

        if (_rotationAngleDone + rotation <= _rotationAngle)
        {
            transform.Rotate(Vector3.up, rotation * RotationDirection);

            _rotationAngleDone += rotation;
        }
        else
        {
            transform.Rotate(Vector3.up, (_rotationAngle - _rotationAngleDone) * RotationDirection);
            _isRotating = false;
            _rotationAngleDone = 0;
        }
    }
}
