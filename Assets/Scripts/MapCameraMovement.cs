using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraMovement : MonoBehaviour
{
    public float _speed = 5;
    
    private CharacterController characterController;
    //private float _parentYRotation;

    private Vector3 move;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Mathf.Round(transform.eulerAngles.y) == 45)
            move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal"));
        else if (Mathf.Round(transform.eulerAngles.y) == 135)
            move = new Vector3(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal"));
        else if (Mathf.Round(transform.eulerAngles.y) == 225)
            move = new Vector3(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        else if (Mathf.Round(transform.eulerAngles.y) == 315)
            move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));

        //Debug.Log(Mathf.Round(transform.eulerAngles.y));

        characterController.Move(move * Time.deltaTime * _speed);
    }
}
