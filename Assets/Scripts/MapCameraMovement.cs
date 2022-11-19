using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraMovement : MonoBehaviour
{
    public float _speed = 5;

    private float _timer = 0;
    
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        _timer = Time.deltaTime;

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        characterController.Move(move * Time.deltaTime * _speed);
    }
}
