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
    private float strafe;
    private bool rewindNext;

    // Use this for initialization
    void Start()
    {
        // turn off the cursor
        Cursor.lockState = CursorLockMode.Locked;
        rewindNext = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Input.GetAxis() is used to get the user's input
        // You can furthor set it on Unity. (Edit, Project Settings, Input)
        translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        strafe = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(strafe, 0, translation);

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(0, speed*Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // turn on the cursor
            Cursor.lockState = CursorLockMode.None;
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            //Rewind or Play
            GameObject[] payloads = GameObject.FindGameObjectsWithTag("Payload");

            foreach(GameObject i in payloads)
            {
                if(rewindNext)
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

            foreach(GameObject j in pushers)
            {
                if(rewindNext)
                {
                    j.GetComponent<Pusher>().Rewind();
                }
            }
        }
    }
}
