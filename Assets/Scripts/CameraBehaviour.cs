using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    [SerializeField]
    private GameObject _playerPos;
    [SerializeField]
    private float _rotationSpeed = 10.0f;
    float timeCount = 0.0f;

    private Vector3 rotationPoint;

    // Start is called before the first frame update
    void Start()
    {
        //set rotation point
      
        rotationPoint = gameObject.transform.position;
        rotationPoint.z = _playerPos.transform.position.z;

       // StartCoroutine(RotateTest());
    }

    // Update is called once per frame
    void Update()
    {
        // _targetRot = Quaternion.AngleAxis(90, transform.forward);

        //transform.rotation = Quaternion.Lerp(transform.rotation, _targetRot, timeCount * _rotationSpeed);
        //timeCount = timeCount + Time.deltaTime;

        //To make it smooth and not stop -> timeCount * _rotationSpeed instead of 90
        //I still need to find a way to combine these 2 properly
        if (Input.GetButtonDown("RotateCameraLeft"))
        {
            transform.RotateAround(rotationPoint, new Vector3(0, 1, 0), 90);
        }
        else if(Input.GetButtonDown("RotateCameraRight"))
        {
            transform.RotateAround(rotationPoint, new Vector3(0, 1, 0), -90);
        }
    }



    //IEnumerator RotateTest()
    //{

    //}

}
