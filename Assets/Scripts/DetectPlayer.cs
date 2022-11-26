using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    private RotatingPlatform _rotatingPlatform;

    private void Start()
    {
        _rotatingPlatform = GetComponentInParent<RotatingPlatform>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Equals("Player") && _rotatingPlatform.IsRotating)
        {
            //access camera through RotatingPlatform and ask to rotate
        }
    }
}
