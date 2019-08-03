/* 
 * author : jiankaiwang
 * description : The script provides you with basic operations of first personal control.
 * platform : Unity
 * date : 2017/12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public float speed = 10.0f;
    private float translation;
    private float straffe;
    public Camera playerCam;
    public float rayDistance;
    private GameObject objectHeld;
    private bool isObjectHeld;
    public float distance = 1f;
    public float maxDistanceGrab = 100f;

    // Use this for initialization
    void Start()
    {
        // turn off the cursor
        Cursor.lockState = CursorLockMode.Locked;
        playerCam = GetComponentInChildren<Camera>();
        isObjectHeld = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Input.GetAxis() is used to get the user's input
        // You can furthor set it on Unity. (Edit, Project Settings, Input)
        translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        straffe = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(straffe, 0, translation);


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // turn on the cursor
            Cursor.lockState = CursorLockMode.None;
        }

        if(Input.GetKeyDown(KeyCode.Mouse1)){
            if(!isObjectHeld && objectHeld == null){
                Pickup();
            } else if(isObjectHeld){
                DropObject();
            }
        } else if (objectHeld != null){
            holdObject();
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
        }
    }
      private void holdObject(){
        Ray playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
       
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
