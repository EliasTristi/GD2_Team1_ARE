using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObjects : MonoBehaviour
{
    private CharacterController _player;
    private GameObject _pickedItem;
    private GameObject _camera;

    [SerializeField]
    private Vector2 _throwForce = new Vector2(180, 200);
    private int _throwDirection;

    [SerializeField]
    private LayerMask _pickable;

    private bool IsCameraAlongX
    {
        get
        {
            return _camera.transform.forward == Vector3.right;
        }
    }
    private bool IsCameraAlongZ
    {
        get
        {
            return _camera.transform.forward == Vector3.forward;
        }
    }
    private bool IsCameraAgainstZ
    {
        get
        {
            return _camera.transform.forward == -Vector3.forward;
        }
    }

    private void Start()
    {
        _player = GetComponent<CharacterController>();
        _camera = GameObject.Find("Camera");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && _pickedItem != null)
        {
            ThrowItem();
        }
        else if (_player.isGrounded && Input.GetKeyDown(KeyCode.UpArrow) && _pickedItem == null)
        {
            _pickedItem = pickupItem();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
            _throwDirection = -1;
        if (Input.GetKey(KeyCode.RightArrow))
            _throwDirection = 1;
    }

    private void ThrowItem()
    {
        Rigidbody itemBody = _pickedItem.GetComponent<Rigidbody>();

        if (IsCameraAlongX)
        {
            itemBody.AddForce(0, _throwForce.y, -_throwForce.x * _throwDirection);
        }
        else if(IsCameraAlongZ)
        {
            itemBody.AddForce(_throwForce.x * _throwDirection, _throwForce.y, 0);
        }
        else if (IsCameraAgainstZ)
        {
            itemBody.AddForce(-_throwForce.x * _throwDirection, _throwForce.y, 0);
        }
        else
        {
            itemBody.AddForce(0, _throwForce.y, _throwForce.x * _throwDirection);
        }

        itemBody.useGravity = true;
        _pickedItem.transform.parent = null;
        _pickedItem = null;
    }

    private GameObject pickupItem()
    {
        Vector3 direction;

        if (IsCameraAlongX)
            direction = new Vector3(1, 0, 0);
        else
            direction = new Vector3(0, 0, 1);

        if (Physics.BoxCast(transform.position - (direction * 100), new Vector3(1.5f, 1, 1.5f), direction, out RaycastHit hit, Quaternion.Euler(transform.forward), 200, _pickable))
        {
            GameObject item = hit.collider.gameObject;

            item.transform.parent = transform;
            item.transform.position = transform.position + Vector3.up;
            item.GetComponent<Rigidbody>().useGravity = false;
            return item;
        }
        else
            return null;
    }
}
