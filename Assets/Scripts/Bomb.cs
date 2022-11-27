using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private bool _activated = false;
    private float _tick = 5f;

    private GameObject _camera;
    [SerializeField]
    private LayerMask _destroyable;

    [SerializeField]
    private Material[] _twinkleColor;
    private MeshRenderer _meshRenderer;

    private bool IsCameraAlongX
    {
        get
        {
            return _camera.transform.forward.Abs() == Vector3.right;
        }
    }

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _camera = GameObject.Find("Camera");
    }

    private void FixedUpdate()
    {
        if (!_activated && GetComponentInParent<CharacterController>())
        {
            _activated = true;
        }
        if(_activated)
        {
            TickDown();
        }

        if(_tick <= 0)
        {
            Explode();
        }
    }


    private int _activeMaterial;
    private void TickDown()
    {
        if((_tick - Time.fixedDeltaTime) <= (int)_tick)
        {
            if (_activeMaterial == 1)
            {
                _meshRenderer.material = _twinkleColor[0];
                _activeMaterial = 0;
            }
            else
            {
                _meshRenderer.material = _twinkleColor[1];
                _activeMaterial = 1;
            }
        }

        _tick -= Time.fixedDeltaTime;
    }

    private void Explode()
    {
        Vector3 castDirection;

        if (IsCameraAlongX)
            castDirection = new Vector3(1, 0, 0);
        else
            castDirection = new Vector3(0, 0, 1);

        if (Physics.SphereCast(transform.position - (castDirection * 100), 1.5f, castDirection, out RaycastHit hit, 200, _destroyable))
        {
            Destroy(hit.collider.gameObject);
        }

        Destroy(gameObject);
    }
}
