using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    public AgentManager managerScript;
    public float seekWeight;
    public float fleeWeight;
    public float maxSpeed = 1f;
    public float maxForce = 1f;
    public float radius;
    public float mass = 1.0f;

    public Vector3 position;
    public Vector3 direction;
    public Vector3 velocity;
    public Vector3 force;

    public bool isPredator = true;

    public AudioSource quack;

    // Start is called before the first frame update
    void Start()
    {
        position = gameObject.transform.position;
        direction = new Vector3(0, 0, 0);
        velocity = new Vector3(0, 0, 0);
        force = new Vector3(0, 0, 0);
        radius = GetComponent<SphereCollider>().radius;
        managerScript = FindObjectOfType<AgentManager>();
        quack = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0,1000) == 999)
            quack.PlayOneShot(quack.clip);
        CalcSteeringForces();
        UpdatePosition();
        SetTransform();
    }

    public void CalcSteeringForces()
    {
        Vector3 ultForce = Vector3.zero;

        if (managerScript.terrain.SampleHeight(position) > 5.0f)
        {
            ultForce += Seek(new Vector3(50, managerScript.terrain.SampleHeight(new Vector3(50, 0, 50)), 50));
        }
        else
        {
            if (Vector3.Dot(managerScript.player.transform.forward, transform.position - managerScript.player.transform.position) < 0 || (managerScript.player.transform.position - transform.position).magnitude > 10.0f)
            {
                ultForce += Seek(managerScript.player.transform.position) * seekWeight;
                isPredator = true;
            }
            else
            {
                ultForce += Flee(managerScript.player.transform.position) * fleeWeight * 10;
                isPredator = false;
            }
        }
        ultForce += Separation() * fleeWeight;
        ultForce.y = 0;
        ultForce = Vector3.ClampMagnitude(ultForce, maxForce);
        ApplyForce(ultForce);
    }

    public void UpdatePosition()
    {
        velocity += force;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        position += velocity;
        position = new Vector3(position.x, managerScript.terrain.SampleHeight(position), position.z);
        direction = velocity.normalized;
        force = Vector3.zero;
    }

    void SetTransform()
    {
        transform.forward = direction;
        velocity.y = 0;
        transform.position = position;
    }

    public void ApplyForce(Vector3 newForce)
    {
        force += newForce / mass;
    }

    public Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desiredVelocity = targetPos - gameObject.transform.position;
        desiredVelocity = desiredVelocity.normalized * maxSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;
        return steeringForce * Time.deltaTime;
    }

    public Vector3 Flee(Vector3 targetPos)
    {
        Vector3 desiredVelocity = gameObject.transform.position - targetPos;
        desiredVelocity = desiredVelocity.normalized * maxSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;
        return steeringForce * Time.deltaTime;
    }

    public Vector3 Separation()
    {
        int indexOfClosest = -1;
        foreach (var item in managerScript.ducks)
        {
            if (item != gameObject)
            {
                if (indexOfClosest == -1)
                {
                    indexOfClosest = managerScript.ducks.IndexOf(item);
                }
                else if ((managerScript.ducks[indexOfClosest].transform.position - transform.position).magnitude > (item.transform.position - transform.position).magnitude)
                {
                    indexOfClosest = managerScript.ducks.IndexOf(item);
                }
            }
        }
        if ((managerScript.ducks[indexOfClosest].transform.position - gameObject.transform.position).magnitude <= radius * 2)
        {
            return Flee(managerScript.ducks[indexOfClosest].transform.position);
        }
        return Vector3.zero;
    }

    public void HitByRay()
    {
        Debug.Log("I got shot");
        // Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !isPredator)
        {
            AudioSource a = managerScript.GetComponent<AudioSource>();
            a.PlayOneShot(a.clip);
            managerScript.ducks.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
