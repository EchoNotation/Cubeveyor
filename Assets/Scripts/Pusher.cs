using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
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
    GameObject soundManager;
    public Direction direction;
    private bool waitingToDeploy;
    private const long deployTimer = 1500;
    private GameObject grabbedObject;
    private ParticleSystem Grab;
    AudioSource source;
    private Vector3 targetPos, initalPos, offsetDirection;
    private Rigidbody body;
    private Outline linkedWall;


    // Start is called before the first frame update
    void Start()
    {
        source = this.GetComponent<AudioSource>();
        Grab = GetComponentInChildren<ParticleSystem>();
        Grab.Stop();
        waitingToDeploy = false;
        timer = new System.Diagnostics.Stopwatch();
        transform.rotation = Quaternion.Euler(45, 45, 45);
        soundManager = GameObject.Find("SoundManager");
        body = GetComponent<Rigidbody>();
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * 50, 0, Space.World);
        if (body.position != targetPos)
        {
            Vector3 diffrence = (targetPos - body.position);
            body.velocity = diffrence * Time.deltaTime * 150f + offsetDirection * diffrence.magnitude;
        }
        if (waitingToDeploy)
        {
            if (timer.ElapsedMilliseconds > deployTimer)
            {
                //Deploy the payload!
                grabbedObject.GetComponent<Payload>().Release();
                waitingToDeploy = false;
                timer.Stop();
                timer.Reset();
            }
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Payload") && !Variables.isEditMode)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }

            source.Play();

            grabbedObject = col.gameObject;
            grabbedObject.GetComponent<Payload>().Grab(direction, this.transform);
            waitingToDeploy = true;
            timer.Start();
            Grab.Play();

        }
    }

    public void Rewind()
    {
        timer.Stop();
        timer.Reset();
        source.Stop();
        Grab.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void setTarget(Vector3 targetPos, Vector3 offsetDirection)
    {
        initalPos = transform.position;
        this.targetPos = targetPos;
        this.offsetDirection = offsetDirection;
    }

    public void LinkWall(Outline wall)
    {
        linkedWall = wall;
        linkedWall.LinkPusher(this.gameObject);
    }

    public void UnlinkWall()
    {
        if (linkedWall != null)
        {
            linkedWall.UnlinkPusher();
            linkedWall = null;
        }
    }
    public bool isLinked(){
        if (linkedWall == null) {
            return false;
        }
        return true;
    }
}
