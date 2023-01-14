using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider Other)
    {
        SceneManager.LoadScene(1);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
