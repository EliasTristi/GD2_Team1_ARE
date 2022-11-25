using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZTeleport : MonoBehaviour
{
    [SerializeField] private GameObject _playerRoot;
    [SerializeField] private int _raycastDistance = 1000;

    private Camera _camera;
    private Transform _ZTriggersContainer;
    private CharacterController _charCtrl;

    private int _platformLayer;
    private int _playerLayer;
    private int _ZTriggerLayer;
    private LayerMask _platformLayerMask;
    private int _platformPlayerLayerMask;

    private Dictionary<GameObject, Vector3> _zTriggerStartPositions
        = new Dictionary<GameObject, Vector3>();
    private bool _wasBehindPlatform = false;
    private bool _isBehindPlatform = false;
    private bool _ignoreBehindPlatform = false;

    private void Start()
    {
        _platformLayer = LayerMask.NameToLayer("Platform");
        _playerLayer = LayerMask.NameToLayer("Player");
        _ZTriggerLayer = LayerMask.NameToLayer("ZTrigger");
        _platformLayerMask = LayerMask.GetMask("Platform");
        _platformPlayerLayerMask = LayerMask.GetMask("Platform", "Player");
        _charCtrl = _playerRoot.GetComponent<CharacterController>();
        _camera = Camera.main;

        var cameraScript = _camera.GetComponent<CameraBehaviour>();
        cameraScript.StartRotation +=
            (s, e) => _ignoreBehindPlatform = true;
        cameraScript.StopRotation +=
            (s, e) =>
            {
                _ignoreBehindPlatform = false;
                _isBehindPlatform = true;
                _wasBehindPlatform = true;
                UpdateZTriggers();
            };

        _ZTriggersContainer = new GameObject("ZTriggers").transform;

        CreateZTriggers();
        UpdateZTriggers();
    }

    private void Update()
    {
        // If the player moves behind an object, teleport to front
        // If the player moves away from being behind an object, do nothing
        // If the player moves behind an object while camera is turning, do nothing
        if (!_ignoreBehindPlatform)
        {
            UpdateIsBehindPlatform();
            if (_isBehindPlatform && !_wasBehindPlatform)
                TeleportToFrontOfPlatform();
        }
    }

    private void OnTriggerEnter(Collider zTrigger)
    {
        if (zTrigger.gameObject.layer == _ZTriggerLayer)
        {
            Vector3 collisionPoint = zTrigger.ClosestPointOnBounds(_playerRoot.transform.position);
            Vector3 collisionNormal = _playerRoot.transform.position - collisionPoint;
            Debug.Log($"Collision Normal: {collisionNormal} - Player: {_playerRoot.transform.position} - Collision Point: {collisionPoint}");

            var zTriggerScript = zTrigger.gameObject.GetComponent<ZTrigger>();
            zTriggerScript ??= zTrigger.transform.parent.GetComponent<ZTrigger>();
            if (zTriggerScript.Type == ZTriggerType.Platform
                && !_charCtrl.isGrounded
                && collisionNormal.y > 0)
                TeleportToFrontOfPlatform();
            else if (zTriggerScript.Type == ZTriggerType.Trigger)
                TeleportToTrigger(zTriggerScript);
            UpdateZTriggers();
        }
    }

    private void TeleportToTrigger(ZTrigger zTriggerScript)
    {
        if (zTriggerScript.OriginalObject == null)
        {
            _zTriggerStartPositions.Remove(zTriggerScript.gameObject);
            Destroy(zTriggerScript.gameObject);
            Debug.Log("Not teleporting, since original object was removed.");
            return;
        }
        // teleport player to Z depth of original object that trigger refers to
        Vector3 playerPosition = _playerRoot.transform.position;
        if (IsCameraAlongZ())
            playerPosition.z = zTriggerScript.OriginalPosition.z;
        else if (IsCameraAlongX())
            playerPosition.x = zTriggerScript.OriginalPosition.x;
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

    private void TeleportToFrontOfPlatform()
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

        foreach (ZTrigger original in FindObjectsOfType<ZTrigger>())
        {
            GameObject levelObject = original.gameObject;
            GameObject zTrigger = Instantiate(levelObject, _ZTriggersContainer);
            _zTriggerStartPositions.Add(zTrigger, levelObject.transform.position);

            // remove all unnecessary components
            foreach (Component component in zTrigger.GetComponentsInChildren<Component>())
            {
                if (component is Transform) continue;
                else if (component is ZTrigger) ((ZTrigger)component).OriginalObject = levelObject;
                else if (component is Collider) ((Collider)component).isTrigger = true;
                else DestroyImmediate(component);
            }

            // change layer
            zTrigger.layer = _ZTriggerLayer;
            foreach (Transform triggerChild in zTrigger.transform)
                triggerChild.gameObject.layer = _ZTriggerLayer;

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

    private void UpdateIsBehindPlatform()
    {
        _wasBehindPlatform = _isBehindPlatform;

        Vector3 playerPosition = _playerRoot.transform.position;
        Vector3 cameraDirection = _camera.transform.forward;
        Vector3 raycastOrigin = playerPosition;
        if (IsCameraAlongZ()) raycastOrigin.z = _camera.transform.position.z;
        else if (IsCameraAlongX()) raycastOrigin.x = _camera.transform.position.x;

        _isBehindPlatform = false;
        if (Physics.Raycast(raycastOrigin, cameraDirection, out RaycastHit hitInfo, _raycastDistance, _platformPlayerLayerMask))
            if (hitInfo.collider.gameObject.layer == _platformLayer)
                _isBehindPlatform = true;
    }

    private bool IsCameraAlongX()
        => _camera.transform.forward.Abs() == Vector3.right;
    private bool IsCameraAlongZ()
        => _camera.transform.forward.Abs() == Vector3.forward;
}
