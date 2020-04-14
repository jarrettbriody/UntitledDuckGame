using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : MonoBehaviour
{
    public AgentManager managerScript;

    public bool isPredator = true;

    public AudioSource cluck;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 1000) == 999)
            cluck.PlayOneShot(cluck.clip);

        if (Vector3.Dot(managerScript.player.transform.forward, transform.position - managerScript.player.transform.position) < 0 || (managerScript.player.transform.position - transform.position).magnitude > 10.0f)
        {
            isPredator = true;
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = managerScript.player.transform.position;
        }
        else
        {
            isPredator = false;
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = (transform.position - managerScript.player.transform.position).normalized * 5 + transform.position;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !isPredator)
        {
            AudioSource a = managerScript.GetComponent<AudioSource>();
            a.PlayOneShot(a.clip);
            managerScript.chickens.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
