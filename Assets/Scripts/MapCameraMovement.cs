using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraMovement : MonoBehaviour
{
    public float _speed = 5;
    
    private CharacterController characterController;
    private float _parentYRotation;

    private Vector3 move;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Debug.Log(Mathf.Round(transform.eulerAngles.y));

        if (Mathf.Round(transform.eulerAngles.y) == 0)
            move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal"));
        else if (Mathf.Round(transform.eulerAngles.y) == 90)
            move = new Vector3(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal"));
        else if (Mathf.Round(transform.eulerAngles.y) == 180)
            move = new Vector3(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        else if (Mathf.Round(transform.eulerAngles.y) == 270)
            move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));

        characterController.Move(move * Time.deltaTime * _speed);
    }
}
