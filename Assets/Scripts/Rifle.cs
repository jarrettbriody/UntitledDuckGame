using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Rifle : Weapon
{
    private bool autoFireDelay = false;
    Image hitmarker;

    void Awake()
    {
        hitmarker = GameObject.Find("Hitmarker").GetComponent<Image>();
    }

    void Update()
    {
        CheckInput();
    }

    public override void CheckInput()
    {
        if (!swapping)
        {
            if (Input.GetKeyDown(KeyCode.B) && !UI.isPaused)
            {
                GameObject.Find("FireModeChange").GetComponent<AudioSource>().Play();

                autoFire = !autoFire;
            }

            if (!autoFire)
            {
                if (Input.GetMouseButtonDown(0) && !UI.isPaused)
                {
                    if (currentAmmo > 0)
                    {
                        Shoot();
                    }
                    else
                    {
                        audioSource.PlayOneShot(emptyMagazine);
                    }
                }
            }
            else
            {
                if (!autoFireDelay)
                {
                    if (Input.GetMouseButton(0) && !UI.isPaused)
                    {
                        if (currentAmmo > 0)
                        {
                            StartCoroutine("ResetAutoFireDelay", 0.1f);

                            Shoot();
                        }
                        else
                        {
                            StartCoroutine("ResetAutoFireDelay", 0.1f);

                            audioSource.PlayOneShot(emptyMagazine);
                        }
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

    public override void SwapOut()
    {
        swapping = true;
        StopAllCoroutines();
        autoFireDelay = false;
    }

    public override void Shoot()
    {
        // play particle and sound effects here
        audioSource.PlayOneShot(audioSource.clip);

        bulletSpawnPoint.GetComponent<ParticleSystem>().Play();

        GameObject firedBullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform);

        currentAmmo--;

        RaycastHit firstHit; 
        Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out firstHit, gunRange);
        Transform closest = firstHit.transform;

        foreach (RaycastHit hit in Physics.RaycastAll(fpsCam.transform.position, fpsCam.transform.forward, gunRange))
        {
            if (Vector3.Distance(fpsCam.transform.position, hit.transform.position) < Vector3.Distance(fpsCam.transform.position, closest.position) 
                && hit.transform.tag == "Wall" || hit.transform.tag == "Floor" || hit.transform.tag == "Enemy")
            {
                closest = hit.transform;
            }
        }

        if (closest != null && closest.tag == "Enemy") // This could be changed to a general enemy tag once more varietes are in the game
        {
            closest.SendMessage("HitByRay"); // include a void HitByRay() method in other scripts that should react to getting shot
                                             // firedBullet.GetComponent<Bullet>().raycastHitPosition = hit.transform.position;
            StartCoroutine(HitmarkerDelay());
        }
    }

    
    IEnumerator HitmarkerDelay()
    {
        hitmarker.enabled = true;

        yield return new WaitForSeconds(0.1f);

        hitmarker.enabled = false;
    }
    
}
