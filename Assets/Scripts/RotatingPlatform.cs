using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    private const int _rotationAngle = 90;
    private const float _rotationSpeed = 40f;
    private const float _rotationFrequency = 1.5f;
    private float _delay;

    private bool _isRotating = false;
    public bool IsRotating { get { return _isRotating; } }

    private float _rotationAngleDone;

    [SerializeField]
    private LayerMask _playerMask;
    private GameObject _camera;
    public GameObject Camera { get { return _camera; } }
    private GameObject _player;

    private void Start()
    {
        _camera = GameObject.Find("Camera");
        _player = GameObject.Find("Player");


        Transform playerDetector = null;
        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms)
        {
            if (t.name.Equals("PlayerDetector"))
            {
                playerDetector = t;
            }
        }

        playerDetector.GetComponent<BoxCollider>().size = new Vector3(1 + ((1/transform.localScale.x) / 5), 1 + ((1 / transform.localScale.y) / 5), 1 + ((1 / transform.localScale.z) / 5));
    }

    private void Update()
    {
        if (_isRotating)
            RotatePlatform();
        else
            Wait();
    }

    private void Wait()
    {
        if (_delay < _rotationFrequency)
            _delay += Time.deltaTime;
        else
        {
            _delay -= _delay;
            _isRotating = true;
        }
    }

    private void RotatePlatform()
    {
        float rotation = _rotationSpeed * Time.deltaTime;

        if (_rotationAngleDone + rotation <= _rotationAngle)
        {
            transform.Rotate(Vector3.up, rotation);

            _rotationAngleDone += rotation;
        }
        else
        {
            transform.Rotate(Vector3.up, _rotationAngle - _rotationAngleDone);
            _isRotating = false;
            _rotationAngleDone = 0;
        }
    }
}
