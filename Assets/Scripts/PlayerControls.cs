/* 
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
    public Camera playerCam;
    private float rayDistance = 100f;
    private GameObject objectHeld;
    private bool isObjectHeld;
    private float distance = 3f;
    public float maxDistanceGrab = 100f;
    private bool rewindNext;

    GameObject currentHit, lastHit;
    CharacterController controller;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;
    // Use this for initialization
    void Start()
    {
        // turn off the cursor
        Cursor.lockState = CursorLockMode.Locked;
        playerCam = GetComponentInChildren<Camera>();
        isObjectHeld = false;
        rewindNext = false;
        Variables.isEditMode = true;
        controller = this.GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);

        // Input.GetAxis() is used to get the user's input
        // You can furthor set it on Unity. (Edit, Project Settings, Input)

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
                holdObject(false);
            }
        }
        else if (objectHeld != null)
        {
            holdObject(true);
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
                    Variables.isEditMode = true;
                }
                else
                {
                    i.GetComponent<Payload>().Play();
                    Variables.isEditMode = false;
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
        if (Variables.isEditMode)
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
            }
        }
    }
    private void holdObject(bool hold)
    {
        RaycastHit hit;
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.tag == "Wall")
            {
                currentHit = hit.collider.gameObject;
                Outline temp = currentHit.GetComponent<Outline>();
                temp.color = 1;
                temp.eraseRenderer = false;
                if (lastHit != currentHit && lastHit != null)
                {
                    Outline test = lastHit.GetComponent<Outline>();
                    test.color = 2;
                    test.eraseRenderer = true;
                }
                lastHit = currentHit;

            }
            if (!hold)
            {
                isObjectHeld = false;
                objectHeld.GetComponent<Rigidbody>().velocity = Vector3.zero;
                objectHeld.GetComponent<Rigidbody>().position = (hit.normal + currentHit.transform.position);
                objectHeld = null;
                currentHit.GetComponent<Outline>().color = 2;
                currentHit.GetComponent<Outline>().eraseRenderer = true;
            }
        }

        if (objectHeld != null)
        {
            Ray playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, .75f, 0));

            Vector3 nextPos = playerCam.transform.position + playerAim.direction * distance;
            Vector3 currPos = objectHeld.transform.position;

            objectHeld.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 10;

            if (Vector3.Distance(objectHeld.transform.position, playerCam.transform.position) > maxDistanceGrab || !Variables.isEditMode)
            {
                holdObject(false);
            }
        }
    }
}
