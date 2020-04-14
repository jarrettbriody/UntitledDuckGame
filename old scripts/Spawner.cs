using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject duckPrefab;
    public AgentManager agentManager;

    // Start is called before the first frame update
    void Start()
    {
        agentManager = GameObject.FindObjectOfType<AgentManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 10000) == 999)
        {
            GameObject d = Instantiate(duckPrefab, transform.position, Quaternion.identity);
            agentManager.ducks.Add(d);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject d = Instantiate(duckPrefab, transform.position, Quaternion.identity);
                agentManager.ducks.Add(d);
            }
        }
    }
}
