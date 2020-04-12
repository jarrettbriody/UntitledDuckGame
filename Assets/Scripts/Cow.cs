using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cow : MonoBehaviour
{
    public AgentManager managerScript;
    public int hp = 10;
    AudioSource smack;

    // Start is called before the first frame update
    void Start()
    {
        managerScript = FindObjectOfType<AgentManager>();
        smack = GameObject.Find("Smack").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Duck")
        {
            if (!collision.gameObject.GetComponent<Duck>().isPredator)
            {
                AudioSource a = managerScript.GetComponent<AudioSource>();
                a.PlayOneShot(a.clip);
                managerScript.ducks.Remove(collision.gameObject);
                Destroy(collision.gameObject);
            }
            else
            {
                smack.PlayOneShot(smack.clip);
                hp--;
                if(hp <= 0)
                {
                    SceneManager.LoadScene("scene");
                }
            }
        }
    }
}
