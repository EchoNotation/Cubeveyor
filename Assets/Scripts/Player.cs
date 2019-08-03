using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody body;
    public float sensitivityX;
    private float rotationX = 0.0f;
    private float mouseX;
    private Vector3 forward;
    // Start is called before the first frame update
    void Start()
    {
        body = this.GetComponent<Rigidbody>();
        forward = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            body.AddForce(forward);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 temp = Quaternion.AngleAxis(-90, Vector3.up) * forward;
            body.AddForce(temp);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 temp = Quaternion.AngleAxis(90, Vector3.up) * forward;
            body.AddForce(temp);
        }
        if (Input.GetKey(KeyCode.S))
        {
            body.AddForce(-forward);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            body.AddForce(Vector3.down);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            body.AddForce(Vector3.up);
        }
    }

    public void UpdateForward(Vector3 newAngle)
    {
        forward = new Vector3(newAngle.x, newAngle.y, 0);
    }
}
