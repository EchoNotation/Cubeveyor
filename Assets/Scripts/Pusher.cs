using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    FORWARD,
    BACKWARD,
    BLANK,
}

public class Pusher : MonoBehaviour
{
    //The direction that this pusher imparts its  force in.
    private System.Diagnostics.Stopwatch timer;
    public Direction direction;
    private bool waitingToDeploy;
    private const long deployTimer = 1000;
    private long initialTime;
    private GameObject grabbedObject;

    // Start is called before the first frame update
    void Start()
    {
        waitingToDeploy = false;
        timer = new System.Diagnostics.Stopwatch();
        timer.Start();
        initialTime = timer.ElapsedMilliseconds;
    }

    // Update is called once per frame
    void Update()
    {
        if(waitingToDeploy)
        {
            if(timer.ElapsedMilliseconds - deployTimer > initialTime)
            {
                //Deploy the payload!
                grabbedObject.GetComponent<Payload>().Release();
                initialTime = timer.ElapsedMilliseconds;
                waitingToDeploy = false;
            }
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Payload"))
        {
            grabbedObject = col.gameObject;
            grabbedObject.GetComponent<Payload>().Grab(direction);
            waitingToDeploy = true;
        }
    }

}
