using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraLook : MonoBehaviour
{
    public float sensitivityY, sensitivityX;
    private float rotationY = 0.0f;
    private float rotationX = 0.0f;
    private float mouseY, mouseX;
    // Start is called before the first frame update
    void Start()
    {
        sensitivityX = 100;
        sensitivityY = 100;
    }

    // Update is called once per frame
    void Update()
    {
        mouseY = Input.GetAxis("Mouse Y");
        mouseX = Input.GetAxis("Mouse X");
        rotationX += mouseX * sensitivityX * Time.deltaTime;
        rotationY += -mouseY * sensitivityY * Time.deltaTime;

        Quaternion finalRotation = Quaternion.Euler(rotationY, rotationX, 0.0f);
        this.GetComponentInChildren<Transform>().rotation = finalRotation;
        Debug.Log("Cameraside: " + this.GetComponentInChildren<Transform>().rotation.eulerAngles.x);
    }
}
