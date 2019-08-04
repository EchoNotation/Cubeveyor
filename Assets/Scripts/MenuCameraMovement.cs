using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraMovement : MonoBehaviour
{
    float slerpInc;
    private bool transitioning = false;
    Vector3 origRotation, finalRotation, temp;

    private void Start()
    {
        origRotation = GameObject.Find("Main Camera").transform.rotation.eulerAngles;
        finalRotation = new Vector3(90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(transitioning)
        {
            temp = Vector3.Slerp(origRotation, finalRotation, slerpInc);
            slerpInc += 0.01f;
            GameObject.Find("Main Camera").transform.rotation = Quaternion.Euler(temp.x, temp.y, temp.z);
        }
        else
        {
            
        }
        
    }

    public void StartTransition()
    {
        transitioning = true;
    }
}
