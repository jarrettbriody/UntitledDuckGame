using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPoint;
    public AudioSource audioSource;
    public AudioClip emptyMagazine;
    public Camera fpsCam;
    public float gunDamage, gunRange;
    public int maxAmmo, currentAmmo;
    public bool autoFire = false;
    public Animator anim;
    public bool swapping = false;

    void Start()
    {
        bulletSpawnPoint = transform.Find("BulletSpawnPoint").gameObject;
        audioSource = GetComponent<AudioSource>();
        fpsCam = Camera.main;
        anim = GetComponent<Animator>();
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
    }

    public abstract void SwapOut();
    public abstract void CheckInput();
    public abstract void Shoot();
}
