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

    // Start is called before the first frame update
    void Start()
    {
        isGrappled = false;
        body = this.GetComponent<Rigidbody>();
        payloadOrigin = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrappled)
        {
            body.velocity = new Vector3();
        }
    }

    public void Grab(Direction dir)
    {
        isGrappled = true;
        direction = dir;
    }

    public void Release()
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

    }

    public void ReturnToOrigin()
    {
        this.transform.position = payloadOrigin;
        
    }
}
