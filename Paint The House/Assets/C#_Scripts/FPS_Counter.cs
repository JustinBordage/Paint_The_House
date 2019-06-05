using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS_Counter : MonoBehaviour
{
    private Text fpsText;
    private float elapsedTime = 0f;
    private int frames = 0;


    void Start()
    {
        fpsText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        frames++;
        elapsedTime += Time.unscaledDeltaTime;

        if (elapsedTime >= 1f)
        {
            fpsText.text = "FPS: " + frames;

            elapsedTime -= 1f;
            frames = 0;
        }
    }
}
