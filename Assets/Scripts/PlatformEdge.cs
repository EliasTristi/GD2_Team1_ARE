using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEdge : MonoBehaviour
{
    [SerializeField]
    private BoxCollider _edgeCollider;

    void Start()
    {
        _edgeCollider = GetComponent<BoxCollider>();

        _edgeCollider.size = new Vector3(1 + (2 / transform.localScale.x), (1 / transform.localScale.y) - 0.1f, 1 + (2 / transform.localScale.x));
        _edgeCollider.center = new Vector3(_edgeCollider.center.x, (-0.5f / transform.localScale.y) + 1, _edgeCollider.center.z);
    }
}
