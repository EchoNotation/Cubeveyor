using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    private bool transitioning;
    private int phase;
    private float lerpInc;
    private System.Diagnostics.Stopwatch timer;
    private GameObject trackingCamera;
    private const long hoverDelay = 1000;
    Vector3 cubeTransform, secondPosition, finalPosition, cameraOrigin, temp;
    Vector3 finalRotation, origRotation;

    public void Start()
    {
        timer = new System.Diagnostics.Stopwatch();
        trackingCamera = GameObject.Find("Main Camera");
        cameraOrigin = trackingCamera.transform.position;
        origRotation = trackingCamera.transform.rotation.eulerAngles;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            TransitionIntoCube();
        }

        if(transitioning)
        {
            switch(phase) {
                case 0:
                    temp = Vector3.Lerp(cameraOrigin, secondPosition, lerpInc);
                    lerpInc += 0.01f;
                    trackingCamera.transform.position = temp;
                    
                    if(lerpInc >= 1)
                    {
                        phase++;
                        lerpInc = 0;
                        timer.Start();
                    }
                    break;
                case 1:
                    if(timer.ElapsedMilliseconds > hoverDelay)
                    {
                        phase++;
                        timer.Stop();
                        timer.Reset();
                    }
                    break;
                case 2:
                    temp = Vector3.Lerp(secondPosition, finalPosition, lerpInc);
                    lerpInc += 0.02f;
                    trackingCamera.transform.position = temp;

                    if(lerpInc >= 1)
                    {
                        phase++;
                        lerpInc = 0;
                    }
                    break;
                case 3:
                    //Change this string
                    LoadScene("Tutorial1");
                    break;
                default:
                    Debug.Log("Invalid phase while transitioning! Phase: " + phase);
                    break;
            }
        }
    }

    // Start is called before the first frame update
    public void LoadScene(string sceneName)
    {
        //Implement any required transition logic here...

        SceneManager.LoadScene(sceneName);
    }

    public void TransitionIntoCube()
    {
        cubeTransform = GameObject.Find("RubiksCube").transform.position;
        secondPosition = new Vector3(0, cubeTransform.y + 5f, 0);
        finalPosition = new Vector3(0, cubeTransform.y + 0.85f, 0);
        transitioning = true;

        GameObject.Find("Camera").GetComponent<MenuCameraMovement>().StartTransition();
        GameObject.Find("Canvas").SetActive(false);
    }
}
