using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTrigger : MonoBehaviour
{

    private System.Diagnostics.Stopwatch timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = new System.Diagnostics.Stopwatch();
        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer.ElapsedMilliseconds > 600)
        {
            GameObject.Find("SceneController").GetComponent<SceneControl>().ExitCube();
        }
    }
}
