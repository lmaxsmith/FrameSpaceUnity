using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalOpen : MonoBehaviour
{
    public GameObject particle;
    public GameObject portal;
    private float waitTime = 10.0f;
    private float timer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        particle.SetActive(false);
        portal.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // Check if we have reached beyond 2 seconds.
        // Subtracting two is more accurate over time than resetting to zero.
        if (timer > waitTime)
        {
            particle.SetActive(true);
            portal.SetActive(false);
        }
    }

    /*void OnTriggerEnter(Collider Other)
    {
        if (Other.tag == "Player")
        {
             particle.SetActive(true);
            portal.SetActive(false);
        }


    }*/

}
