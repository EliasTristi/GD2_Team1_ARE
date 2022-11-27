using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWith : MonoBehaviour
{
    private CharacterController _player;
    private Transform _cameraTransform;

    private void Start()
    {
        _player = GetComponent<CharacterController>();
        _cameraTransform = GameObject.Find("Camera").transform;
    }

    private void Update()
    {
        if (_player.isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(Physics.Raycast(transform.position, _cameraTransform.forward, out RaycastHit hit) && hit.collider.TryGetComponent<IInteractible>(out IInteractible interactible))
            {
                interactible.Interaction = gameObject;
            }
        }
    }
}
