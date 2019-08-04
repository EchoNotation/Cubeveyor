using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOn : MonoBehaviour
{
    GameObject[] lights;
    System.Diagnostics.Stopwatch timer;
    private const int lightDelay = 1000;

    // Start is called before the first frame update
    void Start()
    {
        timer = new System.Diagnostics.Stopwatch();
        lights = GameObject.FindGameObjectsWithTag("Light");
        Variables.crosshairVisible = false;

        foreach(GameObject i in lights)
        {
            i.GetComponent<Light>().enabled = false;
        }

        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer.ElapsedMilliseconds > lightDelay)
        {
            foreach(GameObject i in lights)
            {
                i.GetComponent<Light>().enabled = true;
            }

            Variables.crosshairVisible = true;
            GameObject.Find("SoundManager").GetComponent<SoundManager>().LightsSound();

            timer.Stop();
            timer.Reset();
        }
    }
}
