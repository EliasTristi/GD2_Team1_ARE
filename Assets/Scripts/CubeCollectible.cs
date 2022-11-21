using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollectible : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
    private void OnTriggerEnter(Collider other)
    {
        print("called trig enter");
        Inventory playerInventory = other.gameObject.GetComponentInParent<Inventory>();

        if (playerInventory == null)
            return;

        playerInventory.IncreaseCubeCounter();

        Destroy(gameObject);
    }
}
