using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Camera fpsCam;

    public GameObject gun;

    public GameObject bulletPrefab;
    public GameObject bulletSpawnPoint;

    public int playerHealth = 100;

    public float gunDamage = 10f;
    public float gunRange = 1000f;
    public int gunAmmoRemaining = 30;

    public  ParticleSystem muzzleFlash;

    public AudioSource playerHit;

    public AgentManager managerScript;

    // Start is called before the first frame update
    void Start()
    {
        managerScript = FindObjectOfType<AgentManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(gunAmmoRemaining > 0)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        // play particle and sound effects here
        AudioSource a = gun.GetComponent<AudioSource>();
        a.PlayOneShot(a.clip);

        bulletSpawnPoint.GetComponent<ParticleSystem>().Play();

        GameObject firedBullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform);

        gunAmmoRemaining--;

        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, gunRange))
        {
            if (hit.transform.tag == "Enemy") // This could be changed to a general enemy tag once more varietes are in the game
            {
                hit.transform.SendMessage("HitByRay"); // include a void HitByRay() method in other scripts that should react to getting shot
                // firedBullet.GetComponent<Bullet>().raycastHitPosition = hit.transform.position;
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        playerHit.PlayOneShot(playerHit.clip);
        playerHealth -= dmg;
        if(playerHealth <= 0) SceneManager.LoadScene("Barnyard");
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if(collision.gameObject.GetComponent<Chicken>() != null)
            {
                if (!collision.gameObject.GetComponent<Chicken>().isPredator)
                {
                    AudioSource squash = managerScript.transform.GetChild(0).GetComponent<AudioSource>();
                    squash.PlayOneShot(squash.clip);
                    managerScript.enemies.Remove(collision.gameObject);
                    Destroy(collision.gameObject);
                }
                else
                {
                    playerHit.PlayOneShot(playerHit.clip);
                    TakeDamage(7);
                }
            }
        }
    }
}
