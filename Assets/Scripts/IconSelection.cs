using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IconSelection : MonoBehaviour
{
    public float _lerpSpeed = 5;

    public Vector3 _offset = Vector3.one * 10;

    void Start()
    {

    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            Debug.DrawRay(transform.position, transform.forward * 100, Color.yellow);
            if (hit.collider != null)
            {
                Vector3 targetPosition = hit.transform.position + _offset;
                Debug.Log(Vector3.Distance(transform.position, hit.transform.position));

                //Debug.Log("collider detected");
                transform.position = Vector3.Lerp(transform.position, targetPosition, _lerpSpeed * Time.deltaTime);
            }
        }
    }
}
