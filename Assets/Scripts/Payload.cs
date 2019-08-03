using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Payload : MonoBehaviour
{
    //The direction this cube is travelling
    private Rigidbody body;
    private Direction direction;
    private bool isGrappled;
    private const int forceConstant = 5;
    private const int upConstant = 2;
    private Vector3 payloadOrigin;
    private Transform target;
    private Vector3 lastError;
    private GameObject soundManager;

    // Start is called before the first frame update
    void Start()
    {
        isGrappled = false;
        body = this.GetComponent<Rigidbody>();
        payloadOrigin = this.transform.position;
        soundManager = GameObject.Find("SoundManager");
        lastError = Vector3.zero;
        this.transform.position = payloadOrigin;
        this.GetComponent<Rigidbody>().velocity = new Vector3();
        this.GetComponent<Rigidbody>().useGravity = false;
        //body.AddForce(new Vector3(forceConstant,0,0), ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrappled)
        {
            Vector3 error = (target.position - transform.position);
            Vector3 derivitve = error - lastError;
            body.velocity += (error * 1f + derivitve * 4f);
            lastError = error;
        }
    }

    public void Grab(Direction dir, Transform target)
    {
        this.target = target;
        isGrappled = true;
        direction = dir;
    }

    public void Release()
    {
        if(isGrappled)
        {
            isGrappled = false;
            Vector3 force = new Vector3();

            switch(direction)
            {
                case Direction.UP:
                    force = Vector3.up;
                    break;
                case Direction.DOWN:
                    force = Vector3.down;
                    break;
                case Direction.LEFT:
                    force = Vector3.left;
                    break;
                case Direction.RIGHT:
                    force = Vector3.right;
                    break;
                case Direction.FORWARD:
                    force = Vector3.forward;
                    break;
                case Direction.BACKWARD:
                    force = Vector3.back;
                    break;
                default:
                    Debug.Log("Unrecognizaed payload direction! Direction: " + direction.ToString());
                    break;
            }

            if(direction == Direction.UP)
            {
                body.AddForce(force * forceConstant * upConstant, ForceMode.Impulse);
            }
            else
            {
                body.AddForce(force * forceConstant, ForceMode.Impulse);
            }
            body.velocity = Vector3.zero;
            body.transform.position = target.position;
            body.AddForce(force * forceConstant, ForceMode.Impulse);
        }
    }

    public void Play()
    {
        this.GetComponent<Rigidbody>().useGravity = true;
        soundManager.GetComponent<SoundManager>().PlayRewindSound(false);
    }

    public void Rewind()
    {
        this.transform.position = payloadOrigin;
        this.GetComponent<Rigidbody>().velocity = new Vector3();
        this.GetComponent<Rigidbody>().useGravity = false;
        isGrappled = false;
        soundManager.GetComponent<SoundManager>().PlayRewindSound(true);
    }
}
