using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera fpsCam;

    public GameObject gun;

    public GameObject bulletPrefab;
    public GameObject bulletSpawnPoint;

    public int playerHealth = 10;
    bool pickUpDelay = false;

    public float gunDamage = 10f;
    public float gunRange = 1000f;
    public int gunAmmoRemaining = 30;

    public bool autoFire = false;
    bool autoFireDelay = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            GameObject.Find("FireModeChange").GetComponent<AudioSource>().Play();

            autoFire = !autoFire;
        }

        if (!autoFire)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (gunAmmoRemaining > 0)
                {
                    Shoot();
                }
            }
        }
        else
        {
            if (!autoFireDelay)
            {
                if (Input.GetMouseButton(0))
                {
                    if (gunAmmoRemaining > 0)
                    {
                        StartCoroutine("ResetAutoFireDelay", 0.1f);

                        Shoot();
                    }
                }
            }
        }
    }

    IEnumerator ResetAutoFireDelay(float secondsToWait)
    {
        autoFireDelay = true;

        yield return new WaitForSeconds(secondsToWait);

        autoFireDelay = false;
    }

    IEnumerator ResetPickUpDelay(float secondsToWait)
    {
        pickUpDelay = true;

        yield return new WaitForSeconds(secondsToWait);

        pickUpDelay = false;
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
            if (hit.transform.tag == "Duck") // This could be changed to a general enemy tag once more varietes are in the game
            {
                hit.transform.SendMessage("HitByRay"); // include a void HitByRay() method in other scripts that should react to getting shot
                // firedBullet.GetComponent<Bullet>().raycastHitPosition = hit.transform.position;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.tag == "AmmoPickupPlatform")
        {
            if (!pickUpDelay)
            {
                gunAmmoRemaining += 15;

                StartCoroutine("ResetPickUpDelay", 5f);
            }
        }
    }
}
