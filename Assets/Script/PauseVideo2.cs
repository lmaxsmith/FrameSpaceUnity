using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseVideo2 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject videoPlayerPause;

    void Start()
    {

        videoPlayerPause.SetActive(false);
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider Other)
    {
        videoPlayerPause.SetActive(false);

    }

}
