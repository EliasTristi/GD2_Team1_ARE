using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    private int _cubeCounter = 0;

    public int CubeCounter
    {
        get { return _cubeCounter; }
    }

    public UnityEvent<Inventory> _cubeCollected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseCubeCounter()
    {
        ++_cubeCounter;
        _cubeCollected.Invoke(this);
    }

    public int GetCurrentCubes()
    {
        return _cubeCounter;
    }
}
