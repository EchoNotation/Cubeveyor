using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Transform sceneController;

    private System.Diagnostics.Stopwatch timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = new System.Diagnostics.Stopwatch();
    }

    // Update is called once per frame
    void Update()
    {
       if (timer.ElapsedMilliseconds > 1000) {
            Variables.lastLevel++;
            sceneController.GetComponent<SceneControl>().LoadScene("Level Transition");
        } 
    }

    public void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Payload") && !timer.IsRunning)
        {
            //Complete level
            timer.Reset();
            timer.Start();
        }
        
    
    }
}
