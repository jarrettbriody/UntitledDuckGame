using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AgentManager : MonoBehaviour
{
    public Text hp;

    public Text ducksLeft;

    public List<GameObject> chickens;
    public Text ammoLeft;
    public List<GameObject> ducks;

    public Terrain terrain;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        ducks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Duck"));
    }

    // Update is called once per frame
    void Update()
    {
        hp.text = "HP: " + player.GetComponent<Player>().playerHealth;
        ducksLeft.text = "Ducks Remaining: " + ducks.Count;
        ammoLeft.text = "Ammo: " + player.GetComponent<Player>().gunAmmoRemaining;

        if (ducks.Count <= 1)
            SceneManager.LoadScene("scene");
    }
}
