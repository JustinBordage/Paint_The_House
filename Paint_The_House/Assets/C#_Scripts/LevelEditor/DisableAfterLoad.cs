using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterLoad : MonoBehaviour
{
    void DisableObj()
    {
        //Sets the object inactive
        gameObject.SetActive(false);

        //Destroys the script after 0.1 seconds
        Destroy(this, 0.1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Wait for 0.002 seconds (to allow scripts to load)
        Invoke("DisableObj", 0.002f);
    }
}
