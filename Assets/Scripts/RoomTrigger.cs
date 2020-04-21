using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public List<GameObject> enemies;
    public bool hasTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        hasTriggered = false;
        foreach (var item in enemies)
        {
            item.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !hasTriggered)
        {
            foreach (var item in enemies)
            {
                item.SetActive(true);
            }

            FindObjectOfType<AgentManager>().enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

            hasTriggered = true;
        }
    }
}
