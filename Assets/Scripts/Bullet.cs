using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float bulletSpeed = 1000f;
    [HideInInspector] public Vector3 raycastHitPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += GameObject.FindGameObjectWithTag("MainCamera").transform.forward * Time.deltaTime * bulletSpeed;
    }
}
