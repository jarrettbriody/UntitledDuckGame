using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private int bulletCount;
    [SerializeField] private float bulletSpread;
    [SerializeField] private AudioClip cocked;

    private bool fireDelay = false;

    void Update()
    {
        CheckInput();
    }

    public override void CheckInput()
    {

        if (!fireDelay && !swapping)
        {
            if (Input.GetMouseButtonDown(0) && !UI.isPaused)
            {
                if (currentAmmo > 0)
                {
                    StartCoroutine("ResetFireDelay", 0.5f);

                    Shoot();
                }
                else
                {
                    audioSource.PlayOneShot(emptyMagazine);
                }
            }
        }
    }

    IEnumerator ResetFireDelay(float secondsToWait)
    {
        fireDelay = true;

        yield return new WaitForSeconds(secondsToWait);

        audioSource.PlayOneShot(cocked);

        yield return new WaitForSeconds(secondsToWait);

        fireDelay = false;
    }

    public override void SwapOut()
    {
        swapping = true;
        StopAllCoroutines();
        fireDelay = false;
    }

    public override void Shoot()
    {
        // play particle and sound effects here
        audioSource.PlayOneShot(audioSource.clip);
        bulletSpawnPoint.GetComponent<ParticleSystem>().Play();

        GameObject firedBullet;

        currentAmmo--;

        RaycastHit hit;

        Quaternion bulletDirection;

        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 cameraAngles = fpsCam.transform.rotation.eulerAngles;
            bulletDirection = Quaternion.Euler(cameraAngles.x + Random.Range(-bulletSpread, bulletSpread), cameraAngles.y + Random.Range(-bulletSpread, bulletSpread), cameraAngles.z);

            firedBullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletDirection);

            if (Physics.Raycast(bulletSpawnPoint.transform.position, firedBullet.transform.forward, out hit, gunRange))
            {
                if (hit.transform.tag == "Enemy") // This could be changed to a general enemy tag once more varietes are in the game
                {
                    hit.transform.SendMessage("HitByRay"); // include a void HitByRay() method in other scripts that should react to getting shot
                                                           // firedBullet.GetComponent<Bullet>().raycastHitPosition = hit.transform.position;
                }
            }
        }
    }
}
