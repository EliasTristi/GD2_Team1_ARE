using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZTrigger : MonoBehaviour
{
    public ZTriggerType Type;
    [HideInInspector] public GameObject OriginalObject;
    public Vector3 OriginalPosition { get => OriginalObject.transform.position; }
}

public enum ZTriggerType
{
    Platform,
    Trigger
}
