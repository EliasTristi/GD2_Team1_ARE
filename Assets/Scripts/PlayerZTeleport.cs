using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZTeleport : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _levelContainer;
    [SerializeField] private int _raycastDistance = 1000;

    private Transform _ZTriggersContainer;
    private CharacterController _charCtrl;
    private LayerMask _platformLayerMask;
    private int _platformLayer;
    private int _platformZTriggerLayer;
    private Dictionary<GameObject, Vector3> _zTriggerStartPositions
        = new Dictionary<GameObject, Vector3>();

    private void Start()
    {
        _platformLayerMask = LayerMask.GetMask("Platform");
        _platformLayer = LayerMask.NameToLayer("Platform");
        _platformZTriggerLayer = LayerMask.NameToLayer("PlatformZTrigger");
        _ZTriggersContainer = new GameObject("ZTriggers").transform;
        _charCtrl = GetComponent<CharacterController>();

        CreateZTriggers();
        UpdateZTriggers();
    }

    private void OnTriggerEnter(Collider zTrigger)
    {
        if (_charCtrl.isGrounded)
        {
            Debug.Log("grounded and in trigger");
            return;
        }
        else Debug.Log($"Trigger: {zTrigger}");

        if (zTrigger.gameObject.layer == _platformZTriggerLayer)
        {
            Vector3 collisionPoint = zTrigger.ClosestPointOnBounds(transform.position);
            Vector3 collisionNormal = transform.position - collisionPoint;

            Debug.Log($"Collision Normal: {collisionNormal} - Player: {transform.position} - Collision Point: {collisionPoint}");
            // only use the trigger if player lands on it from above
            if (collisionNormal.y > 0)
            {
                Teleport();
                UpdateZTriggers();
            }
        }
    }

    private void Teleport()
    {
        // look which block is directly below player in 2D and teleport to there in Z space

        Vector3 playerPosition = transform.position;
        Vector3 raycastOrigin = new Vector3(playerPosition.x, playerPosition.y - 1, playerPosition.z);
        Vector3 cameraDirection = _camera.transform.forward;
        if (IsCameraAlongZ())
        {
            raycastOrigin.z = _camera.transform.position.z;
            if (Physics.Raycast(raycastOrigin, cameraDirection, out RaycastHit hitInfo, _raycastDistance, _platformLayerMask))
            {
                playerPosition.z = hitInfo.point.z + 0.5f * cameraDirection.z;
            }
        }
        else if (IsCameraAlongX())
        {
            raycastOrigin.x = _camera.transform.position.x;
            if (Physics.Raycast(raycastOrigin, cameraDirection, out RaycastHit hitInfo, _raycastDistance, _platformLayerMask))
            {
                playerPosition.x = hitInfo.point.x + 0.5f * cameraDirection.x;
            }
        }
        else
        {
            Debug.LogWarning("Attempted Teleportation while camera is not on 90° angle");
            return;
        }

        Debug.Log($"Teleport {transform.position} to {playerPosition}");
        _charCtrl.enabled = false;
        transform.position = playerPosition;
        _charCtrl.enabled = true;
    }

    private void CreateZTriggers()
    {
        // Create trigger copies of every platform
        // these will later be placed on the same Z depth as the player

        foreach (Transform child in _levelContainer)
        {
            GameObject platform = child.gameObject;
            if (platform.layer == _platformLayer)
            {
                GameObject zTrigger = Instantiate(platform, _ZTriggersContainer);
                _zTriggerStartPositions.Add(zTrigger, platform.transform.position);

                // remove unnecessary components
                foreach (Renderer renderer in zTrigger.GetComponentsInChildren<Renderer>())
                    Destroy(renderer);
                foreach (MeshFilter meshFilter in zTrigger.GetComponentsInChildren<MeshFilter>())
                    Destroy(meshFilter);
                foreach (Collider collider in zTrigger.GetComponentsInChildren<Collider>())
                    collider.isTrigger = true;

                // change layer
                zTrigger.layer = _platformZTriggerLayer;
                foreach (Transform triggerChild in zTrigger.transform)
                    triggerChild.gameObject.layer = _platformZTriggerLayer;
            }
        }
    }

    private void UpdateZTriggers()
    {
        // Update the local Z position of all triggers to match that of the player

        foreach(GameObject zTrigger in _zTriggerStartPositions.Keys)
        {
            Vector3 position = _zTriggerStartPositions[zTrigger];

            if (IsCameraAlongZ())
                position.z = transform.position.z - .5f * zTrigger.transform.localScale.z;
            else if (IsCameraAlongX())
                position.x = transform.position.x - .5f * zTrigger.transform.localScale.x;
            else
            {
                Debug.LogWarning("Attempted ZTrigger update while camera is not on 90° angle");
                return;
            }

            zTrigger.transform.position = position;
        }
    }

    private bool IsCameraAlongX()
        => _camera.transform.forward.Abs() == Vector3.right;
    private bool IsCameraAlongZ()
        => _camera.transform.forward.Abs() == Vector3.forward;
}
