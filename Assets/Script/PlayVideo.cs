using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVideo : MonoBehaviour
{
    // Start is called before the first frame update
   
    public GameObject originalObject;
    public GameObject videoScreen;

    void Start()
    {
        originalObject.SetActive(true);
        videoScreen.SetActive(false);
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider Other)
    {
        if (Other.tag == "Player")
        {
            videoScreen.SetActive(true);
            originalObject.SetActive(false);
            Debug.Log("Playing!");
        }
       
      
    }

    
    }
