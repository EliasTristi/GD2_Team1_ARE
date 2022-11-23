using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IconSelection : MonoBehaviour
{
    public float _lerpSpeed = 15;

    public GameObject parent;

    private Vector3 _position = Vector3.zero;
    
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            Debug.DrawRay(transform.position, transform.forward * 100, Color.yellow);
            if (hit.collider != null)
            {
                _position = hit.collider.transform.position;
                //Debug.Log("collider detected");
                parent.transform.position = Vector3.Slerp(parent.transform.position, _position, _lerpSpeed * Time.deltaTime);
            }
        }
    }
}
