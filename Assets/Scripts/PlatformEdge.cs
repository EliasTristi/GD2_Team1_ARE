using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEdge : MonoBehaviour
{
    private BoxCollider _edgeCollider;

    private Transform _platformTransform;

    void Start()
    {
        _edgeCollider = GetComponent<BoxCollider>();

        Transform[] transforms = GetComponentsInParent<Transform>();
        foreach(Transform t in transforms)
        {
            if(t.name.Contains("Platform"))
            {
                _platformTransform = t;
            }
        }

        _edgeCollider.size = new Vector3(1 + (2 / _platformTransform.localScale.x), (1 / _platformTransform.localScale.y) - (0.3f * (1 / _platformTransform.localScale.y)), 1 + (2 / _platformTransform.localScale.x));
        _edgeCollider.center = new Vector3(_edgeCollider.center.x, (-0.5f / _platformTransform.localScale.y) + 1, _edgeCollider.center.z);
    }
}
