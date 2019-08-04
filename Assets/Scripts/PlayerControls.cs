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
    public Canvas escMenu, crosshair, controlCanvas;

    private Vector3 moveDirection = Vector3.zero;
    // Use this for initialization
    void Start()
    {
        // turn off the cursor
        Cursor.lockState = CursorLockMode.Locked;
        playerCam = GetComponentInChildren<Camera>();
        isObjectHeld = false;
        escMenu.enabled = false;
        crosshair.enabled = false;
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

            if (Input.GetButton("Jump") && !Variables.inEscMenu)
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
            escMenu.enabled = !escMenu.enabled;

            // turn on the cursor
            if (Variables.inEscMenu)
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Variables.inEscMenu = false;
            }
            else
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Variables.inEscMenu = true;
            }
        }

        if (Variables.crosshairVisible)
        {
            if (Variables.inEscMenu)
            {
                crosshair.enabled = false;
            }
            else
            {
                crosshair.enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !Variables.inEscMenu)
        {
            if (!isObjectHeld && objectHeld == null)
            {
                HandleRewind(true);
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

        if (Input.GetKeyDown(KeyCode.R) && !Variables.inEscMenu)
        {
            HandleRewind(false);
        }
    }

    void Pickup()
    {
        if (Variables.isEditMode && !Variables.inEscMenu)
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
        Outline currentOutline;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.tag == "Wall")
            {
                currentHit = hit.collider.gameObject;
                currentOutline = currentHit.GetComponent<Outline>();
                currentOutline.color = 1;
                currentOutline.eraseRenderer = false;
                if (lastHit != currentHit && lastHit != null)
                {
                    Outline test = lastHit.GetComponent<Outline>();
                    test.color = 2;
                    test.eraseRenderer = true;
                }
                lastHit = currentHit;

                if (!hold)
                {
                    RaycastHit hit2;
                    Ray ray2 = new Ray(currentHit.transform.position, hit.normal);
                    Physics.Raycast(ray2, out hit2, 2f);


                    //objectHeld.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    objectHeld.GetComponent<Pusher>().setTarget((hit.normal + currentHit.transform.position));


                    if (hit2.collider != null && hit2.collider.tag == "Pickupable" && hit2.collider.gameObject != objectHeld)
                    {
                        objectHeld = hit2.collider.gameObject;
                        isObjectHeld = true;
                    }
                    else
                    {
                        objectHeld = null;
                        isObjectHeld = false;
                    }
                    currentOutline.color = 2;
                    currentOutline.eraseRenderer = true;

                }
            }
        }

        if (objectHeld != null)
        {
            Ray playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, .75f, 0));

            Vector3 nextPos = playerCam.transform.position + playerAim.direction * distance;
            Vector3 currPos = objectHeld.transform.position;

            objectHeld.GetComponent<Pusher>().setTarget(nextPos);
            //objectHeld.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 10;

            if (!Variables.isEditMode)
            {
                holdObject(false);
            }
        }
    }

    private void HandleRewind(bool forceRewind) {
            
            if (forceRewind) {
                rewindNext = true;
            }

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

            }

            GameObject[] pushers = GameObject.FindGameObjectsWithTag("Pickupable");

            foreach (GameObject j in pushers)
            {
                if (rewindNext)
                {
                    j.GetComponent<Pusher>().Rewind();
                }
            }

            rewindNext = !rewindNext;

    }

    public void ShowControls()
    {
        controlCanvas.enabled = true;
        escMenu.enabled = false;
    }

    public void HideControls()
    {
        controlCanvas.enabled = false;
        escMenu.enabled = true;
    }
}
