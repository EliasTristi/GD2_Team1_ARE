using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZTeleport : MonoBehaviour
{
    [SerializeField] private Transform _levelContainer;
    [SerializeField] private GameObject _playerRoot;
    [SerializeField] private int _raycastDistance = 1000;

    private Camera _camera;
    private Transform _ZTriggersContainer;
    private CharacterController _charCtrl;
    private LayerMask _platformLayerMask;
    private int _platformLayer;
    private int _platformZTriggerLayer;
    private Dictionary<GameObject, Vector3> _zTriggerStartPositions
        = new Dictionary<GameObject, Vector3>();

    private void Start()
    {
        _platformLayerMask = LayerMask.GetMask("Level");
        _platformLayer = LayerMask.NameToLayer("Level");
        _platformZTriggerLayer = LayerMask.NameToLayer("LevelTrigger");
        _charCtrl = _playerRoot.GetComponent<CharacterController>();
        _camera = Camera.main;
        _camera.GetComponent<CameraBehaviour>().StopRotation +=
            (s, e) => UpdateZTriggers();

        if (_levelContainer == null) Debug.LogError("Please select a level container in inspector, this is the object that holds all the platforms");
        _ZTriggersContainer = new GameObject("ZTriggers").transform;

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
            Vector3 collisionPoint = zTrigger.ClosestPointOnBounds(_playerRoot.transform.position);
            Vector3 collisionNormal = _playerRoot.transform.position - collisionPoint;

            Debug.Log($"Collision Normal: {collisionNormal} - Player: {_playerRoot.transform.position} - Collision Point: {collisionPoint}");
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

        Vector3 playerPosition = _playerRoot.transform.position;
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

        Debug.Log($"Teleport {_playerRoot.transform.position} to {playerPosition}");
        _charCtrl.enabled = false;
        _playerRoot.transform.position = playerPosition;
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
                position.z = _playerRoot.transform.position.z - .5f * zTrigger.transform.localScale.z;
            else if (IsCameraAlongX())
                position.x = _playerRoot.transform.position.x - .5f * zTrigger.transform.localScale.x;
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
