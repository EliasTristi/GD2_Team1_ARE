using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconSelection : MonoBehaviour
{
    public float _lerpSpeed = 5;

    void Start()
    {
        
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            if (hit.collider != null)
            {
                Debug.Log("collider detected");
                Camera.main.transform.position = Vector3.Lerp(transform.position, hit.collider.transform.position, _lerpSpeed * Time.deltaTime);
            }
        }
    }
}
