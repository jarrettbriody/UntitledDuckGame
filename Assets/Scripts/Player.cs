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

    public Weapon currentWeapon;
    [SerializeField] private Weapon rifle, shotgun;
    [SerializeField] private Transform rifleUI, shotgunUI, selectedWeapon, unselectedWeapon;
    private bool rifleSelected = true;
    public bool obtainedShotgun = false;
    public bool finishedSwapping = true;

    public AudioSource playerHit;

    public AgentManager managerScript;

    // Start is called before the first frame update
    void Start()
    {
        shotgunUI.GetComponent<MeshRenderer>().enabled = false;
        managerScript = FindObjectOfType<AgentManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && obtainedShotgun)
        {
            StartCoroutine(SwapWeapon());
        }
    }

    IEnumerator SwapWeapon()
    {
        finishedSwapping = false;
        currentWeapon.SwapOut();
        currentWeapon.anim.Play("SwapOut");

        while (!finishedSwapping)
        {
            yield return null;
        }

        // Update our UI and swap weapons
        currentWeapon = rifleSelected ? shotgun : rifle;
        rifleUI.position = rifleSelected ? unselectedWeapon.position : selectedWeapon.position;
        shotgunUI.position = rifleSelected ? selectedWeapon.position : unselectedWeapon.position;
        rifle.gameObject.SetActive(!rifleSelected);
        shotgun.gameObject.SetActive(rifleSelected);

        rifleSelected = !rifleSelected;

        finishedSwapping = false;

        yield return new WaitForFixedUpdate(); // We have to wait so that the shotgun can initialize its values

        currentWeapon.anim.Play("SwapIn");

        while (!finishedSwapping)
        {
            yield return null;
        }

        currentWeapon.swapping = false;
    }

    public void ObtainedShotgun()
    {
        obtainedShotgun = true;
        shotgunUI.GetComponent<MeshRenderer>().enabled = true;
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
