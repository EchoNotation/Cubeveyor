using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookScript : MonoBehaviour
{
    public float sensitivityY;
    public float sensitivityX;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private float mouseX;
    private float mouseY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotationX += mouseX * sensitivityX * Time.deltaTime;
        rotationY += -mouseY * sensitivityY * Time.deltaTime;

        Quaternion finalRotation = Quaternion.Euler(rotationY, rotationX, 0.0f);
        transform.rotation = finalRotation;
    }
}
