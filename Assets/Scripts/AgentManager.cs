using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class AgentManager : MonoBehaviour
{
    public Text hp;

    public Text enemiesLeft;

    public List<GameObject> enemies;
    public Text ammoLeft;

    public Text fireMode;
    
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
        fireMode.text = "Fire Mode: " + FireModeToString();

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

    private string FireModeToString()
    {
        if(player.GetComponent<Player>().currentWeapon.autoFire)
        {
            return "Auto";
        }
        else
        {
            return "Semi";
        }
    }
}
