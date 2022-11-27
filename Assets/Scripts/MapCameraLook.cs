using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraLook : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 1f;

    [HideInInspector]
    public float parentRotation;
    
    private float _timer = 0;
    private float _lookRotationHorizontal = 45;
    private float _lookRotationVertical = 30;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.J))
            Rotate(parentRotation + 45, Vector3.up);
        else if (Input.GetKey(KeyCode.L))
            Rotate(parentRotation - 45, Vector3.up);
        else if (Input.GetKey(KeyCode.I))
            Rotate(Quaternion.Euler(_lookRotationVertical + 30, parentRotation, 0));
        else if (Input.GetKey(KeyCode.K))
            Rotate(Quaternion.Euler(_lookRotationVertical - 60, parentRotation, 0));
        else
            Rotate(parentRotation, Vector3.up);
    }

    private void Rotate(float degrees, Vector3 axis)
    {
        _timer += Time.deltaTime;

        Quaternion newRotation = Quaternion.AngleAxis(degrees, axis);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, _timer);

        if (_timer >= _rotateSpeed)
        {
            _timer = 0;
        }
    }

    private void Rotate(Quaternion parentRotation)
    {
        _timer += Time.deltaTime;

        transform.rotation = Quaternion.Lerp(transform.rotation, parentRotation, _timer);

        if (_timer >= _rotateSpeed)
        {
            _timer = 0;
        }
    }
}
