using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowProjectile : MonoBehaviour
{
    private Vector3 start;
    private Vector3 end;
    private Vector3 midPoint;
    private Vector3 dir;

    private float speed = 5.0f;

    public AgentManager managerScript;

    // Start is called before the first frame update
    void Start()
    {
        managerScript = FindObjectOfType<AgentManager>();
        start = transform.position;
        end = FindObjectOfType<AgentManager>().player.transform.position;
        midPoint = (end - start) * 0.5f;
        midPoint.y += (end - start).magnitude / 5.5f;
        dir = (midPoint).normalized;
        transform.forward = dir;
        GetComponent<Rigidbody>().AddForce(dir * speed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            managerScript.player.GetComponent<Player>().TakeDamage(10);
            Destroy(gameObject);
        }

        else if(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Floor")
        {
            if((transform.position - managerScript.player.transform.position).magnitude <= 2.0f)
            {
                managerScript.player.GetComponent<Player>().TakeDamage(3);
            }
            Destroy(gameObject);
        }
    }
}
