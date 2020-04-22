using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class AgentManager : MonoBehaviour
{
    public Text hp;

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
        if (Input.GetKeyDown(KeyCode.Escape) && !FindObjectOfType<UIButtonCallbacks>().isOver)
        {
            FindObjectOfType<UIButtonCallbacks>().Pause();
        }
        hp.text = "HP: " + player.GetComponent<Player>().playerHealth;
        ammoLeft.text = "Ammo: " + player.GetComponent<Player>().currentWeapon.currentAmmo + " / " + player.GetComponent<Player>().currentWeapon.maxAmmo;
        fireMode.text = "Fire Mode: " + FireModeToString();

        if (enemies.Count <= 0)
        {
            RoomTrigger[] triggers = FindObjectsOfType<RoomTrigger>();
            for (int i = 0; i < triggers.Length; i++)
            {
                if (!triggers[i].hasTriggered) return;
            }
            FindObjectOfType<UIButtonCallbacks>().ShowGameOver();
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
