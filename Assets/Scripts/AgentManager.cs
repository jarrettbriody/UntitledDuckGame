using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AgentManager : MonoBehaviour
{
    public Text hp;

    public Text enemiesLeft;

    public List<GameObject> enemies;
    public Text ammoLeft;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    // Update is called once per frame
    void Update()
    {
        hp.text = "HP: " + player.GetComponent<Player>().playerHealth;
        enemiesLeft.text = "Enemies Remaining: " + enemies.Count;
        ammoLeft.text = "Ammo: " + player.GetComponent<Player>().gunAmmoRemaining;

        if (enemies.Count <= 0)
        {
            RoomTrigger[] triggers = FindObjectsOfType<RoomTrigger>();
            for (int i = 0; i < triggers.Length; i++)
            {
                if (!triggers[i].hasTriggered) return;
            }
            SceneManager.LoadScene("Barnyard");
        }
            
    }
}
