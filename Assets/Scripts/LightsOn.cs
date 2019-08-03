using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOn : MonoBehaviour
{
    GameObject[] lights;
    System.Diagnostics.Stopwatch timer;
    GameObject canvas;
    private const int lightDelay = 1000;

    // Start is called before the first frame update
    void Start()
    {
        timer = new System.Diagnostics.Stopwatch();
        canvas = GameObject.Find("Canvas");
        canvas.SetActive(false);

        lights = GameObject.FindGameObjectsWithTag("Light");

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

            canvas.SetActive(true);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().LightsSound();

            timer.Stop();
            timer.Reset();
        }
    }
}
