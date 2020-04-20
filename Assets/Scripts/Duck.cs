using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Duck : MonoBehaviour
{
    public AgentManager managerScript;

    public GameObject duckProjectile;

    public float fireCD;
    private float fireTimer;

    public int health = 20;

    // Start is called before the first frame update
    void Start()
    {
        managerScript = FindObjectOfType<AgentManager>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, managerScript.player.transform.position - transform.position, out raycastHit, 50.0f))
        {
            if (raycastHit.transform.gameObject.tag == "Player")
            {
                Vector3 oldForward = transform.forward;
                Vector3 newForward = (managerScript.player.transform.position - transform.position).normalized;
                newForward.y = oldForward.y;
                transform.forward = newForward;
                Fire();
            }
        }
    }

    public void Fire()
    {
        if (fireTimer >= fireCD)
        {
            fireTimer = 0.0f;
            Vector3 projStart = transform.position;
            projStart.y += 0.5f;
            Instantiate(duckProjectile, projStart, Quaternion.identity);
        }
        else fireTimer += Time.deltaTime;
    }

    void HitByRay()
    {
        health -= 10;
        if (health <= 0)
        {
            AudioSource squash = managerScript.transform.GetChild(0).GetComponent<AudioSource>();
            squash.PlayOneShot(squash.clip);
            managerScript.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
