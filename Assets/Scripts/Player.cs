using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera fpsCam;

    public int playerHealth = 10;

    public Weapon currentWeapon;
    [SerializeField] private Weapon rifle, shotgun;
    [SerializeField] private Transform rifleUI, shotgunUI, selectedWeapon, unselectedWeapon;
    private bool rifleSelected = true;
    public bool obtainedShotgun = false;
    public bool finishedSwapping = true;

    // Start is called before the first frame update
    void Start()
    {
        shotgunUI.GetComponent<MeshRenderer>().enabled = false;
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
}
