// Author: Paul King

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    //public float spinSpeed = 0.5f;
    public Vector3 spinSpeed = new Vector3(0, 5f, 0);
    public bool reverseDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(reverseDirection)
            transform.Rotate(-spinSpeed);
        else
            transform.Rotate(spinSpeed);
    }
}
