using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float bulletSpeed = 1000f;
    [HideInInspector] public Vector3 raycastHitPosition;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * bulletSpeed;

        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > 2000f)
        {
            Destroy(gameObject);
        }
    }
}
