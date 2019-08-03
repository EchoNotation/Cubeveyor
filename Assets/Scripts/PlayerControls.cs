﻿/* 
 * author : jiankaiwang
 * description : The script provides you with basic operations of first personal control.
 * platform : Unity
 * date : 2017/12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class PlayerControls : MonoBehaviour
{

    public float speed = 10.0f;
    private float translation;
    public Camera playerCam;
    public float rayDistance;
    private GameObject objectHeld;
    private bool isObjectHeld;
    private float distance = 3f;
    public float maxDistanceGrab = 100f;
    private float strafe;
    private bool rewindNext;

    private Rigidbody body;
    private float distToGround, distToFront, distToSide;
    private CapsuleCollider shape;
    private Vector3 rayCastBoundsFront, rayCastBoundsBack, rayCastBoundsLeft, rayCastBoundsRight;

    bool isGrounded()
    {
        return Physics.Raycast(rayCastBoundsFront, -Vector3.up, distToGround) | Physics.Raycast(rayCastBoundsBack, -Vector3.up, distToGround) | Physics.Raycast(rayCastBoundsLeft, -Vector3.up, distToGround) | Physics.Raycast(rayCastBoundsRight, -Vector3.up, distToGround);
    }
    // Use this for initialization
    void Start()
    {
        // turn off the cursor
        Cursor.lockState = CursorLockMode.Locked;
        playerCam = GetComponentInChildren<Camera>();
        isObjectHeld = false;
        rewindNext = false;
        body = this.gameObject.GetComponent<Rigidbody>();
        shape = this.gameObject.GetComponent<CapsuleCollider>();
        distToGround = shape.bounds.extents.y;
        distToFront = shape.bounds.extents.z;
        distToSide = shape.bounds.extents.x;
    }

    // Update is called once per frame
    void Update()
    {
        rayCastBoundsFront.y = transform.position.y;
        rayCastBoundsFront.x = transform.position.x + distToFront;
        rayCastBoundsFront.z = transform.position.z;
        rayCastBoundsBack.y = transform.position.y;
        rayCastBoundsBack.x = transform.position.x - distToFront;
        rayCastBoundsBack.z = transform.position.z;
        rayCastBoundsLeft.y = transform.position.y;
        rayCastBoundsLeft.x = transform.position.x - distToSide;
        rayCastBoundsLeft.z = transform.position.z;
        rayCastBoundsRight.y = transform.position.y;
        rayCastBoundsRight.x = transform.position.x + distToSide;
        rayCastBoundsRight.z = transform.position.z;
        // Input.GetAxis() is used to get the user's input
        // You can furthor set it on Unity. (Edit, Project Settings, Input)
        translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        strafe = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(strafe, 0, translation);

        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            body.AddForce(0, 200, 0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // turn on the cursor
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!isObjectHeld && objectHeld == null)
            {
                Pickup();
            }
            else if (isObjectHeld)
            {
                DropObject();
            }
        }
        else if (objectHeld != null)
        {
            holdObject();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //Rewind or Play
            GameObject[] payloads = GameObject.FindGameObjectsWithTag("Payload");

            foreach (GameObject i in payloads)
            {
                if (rewindNext)
                {
                    i.GetComponent<Payload>().Rewind();
                }
                else
                {
                    i.GetComponent<Payload>().Play();
                }

                rewindNext = !rewindNext;
            }

            GameObject[] pushers = GameObject.FindGameObjectsWithTag("Pusher");

            foreach (GameObject j in pushers)
            {
                if (rewindNext)
                {
                    j.GetComponent<Pusher>().Rewind();
                }
            }
        }
    }

    void Pickup()
    {
        RaycastHit hit;
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.tag == "Pickupable")
            {
                objectHeld = hit.collider.gameObject;
                isObjectHeld = true;
            }
            else if (hit.collider.tag == "Wall")
            {
                Debug.Log(hit.collider.gameObject.name);
                Outline temp = hit.collider.gameObject.GetComponent<Outline>();
                temp.color = 0;
                temp.eraseRenderer = false;

            }
        }
    }
    private void holdObject()
    {
        RaycastHit hit;
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
        if (hit.collider.tag == "Wall")
                {
                    Debug.Log(hit.collider.gameObject.name);
                    Outline temp = hit.collider.gameObject.GetComponent<Outline>();
                    temp.color = 0;
                    temp.eraseRenderer = false;

                }
        }

        Ray playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, .75f, 0));

        Vector3 nextPos = playerCam.transform.position + playerAim.direction * distance;
        Vector3 currPos = objectHeld.transform.position;

        objectHeld.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 10;

        if (Vector3.Distance(objectHeld.transform.position, playerCam.transform.position) > maxDistanceGrab)
        {
            DropObject();
        }
    }

    private void DropObject()
    {
        isObjectHeld = false;
        objectHeld.GetComponent<Rigidbody>().velocity = Vector3.zero;
        objectHeld = null;
    }
}