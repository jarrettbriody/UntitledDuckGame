﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Duck : MonoBehaviour
{
    public AgentManager managerScript;

    public GameObject duckProjectile;

    public float fireCD;
    private float fireTimer;

    public const int MAX_HEALTH = 20;
    public int health = 20;

    public AudioSource quack;
    public AudioSource shoot;

    Vector2 healthUIScreenPosition;
    bool displayHealthUI = false;

    // Start is called before the first frame update
    void Start()
    {
        managerScript = FindObjectOfType<AgentManager>();
        quack = transform.GetChild(0).GetComponent<AudioSource>();
        shoot = transform.GetChild(1).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 1000) == 999)
            quack.PlayOneShot(quack.clip);
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
            projStart.y += 0.5f;
            Instantiate(duckProjectile, projStart, Quaternion.identity);
            shoot.PlayOneShot(shoot.clip);
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

    void OnGUI()
    {
        if (displayHealthUI)
        {
            Vector2 targetPos = Camera.main.WorldToScreenPoint(transform.position);

            GUI.Box(new Rect(healthUIScreenPosition.x, healthUIScreenPosition.y, 60, 20), health + "/" + MAX_HEALTH);
        }
    }
}
