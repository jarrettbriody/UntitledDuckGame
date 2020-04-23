using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cow : MonoBehaviour
{
    public AgentManager managerScript;

    public float range;
    public GameObject cowProjectile;

    public float fireCD;
    private float fireTimer;

    public const int MAX_HEALTH = 100;
    public int health = 100;

    public AudioSource moo;
    public AudioSource shoot;

    Vector2 healthUIScreenPosition;
    bool displayHealthUI = false;

    // Start is called before the first frame update
    void Start()
    {
        managerScript = FindObjectOfType<AgentManager>();
        moo = transform.GetChild(0).GetComponent<AudioSource>();
        shoot = transform.GetChild(1).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 1000) == 999)
            moo.PlayOneShot(moo.clip);
        if ((transform.position - managerScript.player.transform.position).magnitude < range)
        {
            RaycastHit raycastHit;
            if(Physics.Raycast(transform.position,managerScript.player.transform.position - transform.position, out raycastHit, 50.0f))
            {
                if (raycastHit.transform.gameObject.tag == "Player")
                {
                    Vector3 oldForward = transform.forward;
                    Vector3 newForward = (managerScript.player.transform.position - transform.position).normalized;
                    newForward.y = oldForward.y;
                    transform.forward = newForward;
                    NavMeshAgent agent = GetComponent<NavMeshAgent>();
                    agent.destination = transform.position;
                    Fire();
                }
                else
                {
                    NavMeshAgent agent = GetComponent<NavMeshAgent>();
                    agent.destination = managerScript.player.transform.position;
                }
            }
        }
        else if ((transform.position - managerScript.player.transform.position).magnitude > range - 1.0f)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = managerScript.player.transform.position;
        }

        // For OnGUI()
        healthUIScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        healthUIScreenPosition.y = Screen.height - healthUIScreenPosition.y;

        if (health < MAX_HEALTH)
        {
            displayHealthUI = true;
        }
    }

    public void Fire()
    {
        if (fireTimer >= fireCD)
        {
            fireTimer = 0.0f;
            Vector3 projStart = transform.position;
            projStart.y += 1.3f;
            Instantiate(cowProjectile, projStart, Quaternion.identity);
            shoot.PlayOneShot(shoot.clip);
        }
        else fireTimer += Time.deltaTime;
    }

    void HitByRay()
    {
        health -= 10;
        if(health <= 0)
        {
            AudioSource squash = managerScript.transform.GetChild(0).GetComponent<AudioSource>();
            squash.PlayOneShot(squash.clip);
            managerScript.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    void OnGUI()
    {
        if (displayHealthUI)
        {
            Vector2 targetPos = Camera.main.WorldToScreenPoint(transform.position);

            GUI.Box(new Rect(healthUIScreenPosition.x, healthUIScreenPosition.y, 60, 20), health + "/" + MAX_HEALTH);
        }
    }
}
