using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implementation From: http://www.donovankeith.com/2016/05/making-objects-float-up-down-in-unity/

public class AmmoSpawner : MonoBehaviour
{
    Vector3 tempPos = new Vector3();
    Vector3 posOffset = new Vector3();

    public float degreesPerSecond;
    public float amplitude;
    public float frequency ;

    private AudioSource audioSource;
    private bool pickUpDelay = false;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<Player>();
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
        transform.Rotate(Vector3.up, degreesPerSecond * Time.deltaTime);
    }

    IEnumerator ResetPickUpDelay(float secondsToWait)
    {
        pickUpDelay = true;

        yield return new WaitForSeconds(secondsToWait);

        pickUpDelay = false;

        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (!pickUpDelay && player.currentWeapon.currentAmmo < player.currentWeapon.maxAmmo)
            {
                audioSource.PlayOneShot(audioSource.clip);

                player.currentWeapon.AddAmmo(player.currentWeapon.maxAmmo / 2);

                transform.GetChild(0).gameObject.SetActive(false);

                StartCoroutine("ResetPickUpDelay", 5f);
            }
        }
    }
}
