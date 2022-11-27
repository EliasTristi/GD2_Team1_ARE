using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraZoom : MonoBehaviour
{
    private Camera _cam;
    private float _zoom;
    private float _timer = 0;

    void Start()
    {
        _cam = GetComponent<Camera>();
        _zoom = _cam.orthographicSize;
    }

    void Update()
    {
        //Debug.Log($"size {_cam.orthographicSize}, zoom {_zoom}");

        _timer += Time.deltaTime;

        if (Input.GetButtonDown("MapZoomMin") && _cam.orthographicSize <= 8)
        {
            _zoom = _cam.orthographicSize + 1;
        }
        else if (Input.GetButtonDown("MapZoomMax") && _cam.orthographicSize >= 5)
        {
            _zoom = _cam.orthographicSize - 1;
        }

        if (_timer < 1)
        {
            _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, _zoom, _timer * 10);
            _timer = 0;
        }

    }

    //private void Zoom()
    //{
    //    if (_cam.orthographicSize > _zoom)
    //    {
    //        Mathf.Lerp(_cam.orthographicSize, _zoom, 1f);
    //    }
    //    else if (_cam.orthographicSize < _zoom)
    //    {
    //        Mathf.Lerp(_zoom, _cam.orthographicSize, 1f);
    //    }
    //    if (_cam.orthographicSize == _zoom)
    //        _isZooming = false;
    //}
}
