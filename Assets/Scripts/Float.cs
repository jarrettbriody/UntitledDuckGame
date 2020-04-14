using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implementation From: http://www.donovankeith.com/2016/05/making-objects-float-up-down-in-unity/

public class Float : MonoBehaviour
{
    Vector3 tempPos = new Vector3();
    Vector3 posOffset = new Vector3();

    public float degreesPerSecond;
    public float amplitude;
    public float frequency ;

    // Start is called before the first frame update
    void Start()
    {
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}
