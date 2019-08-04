using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    private bool transitioning, levelTransition, proceedWithTransition;
    private int phase, levelPhase;
    private float lerpInc;
    private System.Diagnostics.Stopwatch timer;
    private GameObject trackingCamera;
    private const long hoverDelay = 1000;
    private const long rotateDelay = 800;
    Vector3 cubeTransform, secondPosition, finalPosition, cameraOrigin, temp, thirdPosition, initialCubeTransform, finalCenterPosition;
    Vector3 finalRotation, origRotation, lookAtRotation;
    public Canvas nextLevel, quitCanvas, controlCanvas;
    public GameObject centerBit;

    public void Start()
    {
        proceedWithTransition = false;
        levelTransition = false;
        transitioning = false;
        timer = new System.Diagnostics.Stopwatch();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            ExitCube();
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
                    LoadScene("Tutorial1");
                    transitioning = false;
                    break;
                default:
                    Debug.Log("Invalid phase while transitioning! Phase: " + phase);
                    break;
            }
        }
        else if(levelTransition)
        {
            switch(levelPhase) {
                case 0:
                    temp = Vector3.Lerp(cameraOrigin, secondPosition, lerpInc);
                    trackingCamera.transform.position = temp;
                    lerpInc += 0.01f;

                    if(lerpInc >= 1)
                    {
                        lerpInc = 0;
                        levelPhase++;
                    }
                    break;
                case 1:
                    temp = Vector3.Lerp(initialCubeTransform, finalCenterPosition, lerpInc);
                    centerBit.transform.position = temp;
                    lerpInc += 0.01f;

                    if(lerpInc >= 1)
                    {
                        lerpInc = 0;
                        levelPhase++;
                        nextLevel.enabled = true;
                        quitCanvas.enabled = true;
                    }
                    break;
                case 2:
                    if(proceedWithTransition)
                    {
                        levelPhase++;
                    }
                    break;
                case 3:
                    temp = Vector3.Slerp(origRotation, lookAtRotation, lerpInc);
                    trackingCamera.transform.rotation = Quaternion.Euler(temp.x, temp.y, temp.z);
                    lerpInc += 0.01f;

                    if(lerpInc >= 1)
                    {
                        lerpInc = 0;
                        levelPhase++;
                        timer.Start();
                    }
                    break;
                case 4:
                    if(timer.ElapsedMilliseconds > rotateDelay)
                    {
                        levelPhase++;
                        timer.Stop();
                        timer.Reset();
                    }
                    break;
                case 5:
                    temp = Vector3.Lerp(secondPosition, thirdPosition, lerpInc);
                    trackingCamera.transform.position = temp;
                    temp = Vector3.Slerp(lookAtRotation, finalRotation, lerpInc);
                    trackingCamera.transform.rotation = Quaternion.Euler(temp.x, temp.y, temp.z);
                    lerpInc += 0.01f;

                    if(lerpInc >= 1)
                    {
                        lerpInc = 0;
                        timer.Start();
                        levelPhase++;
                    }
                    break;
                case 6:
                    if(timer.ElapsedMilliseconds > hoverDelay)
                    {
                        levelPhase++;
                        timer.Stop();
                        timer.Reset();
                    }
                    break;
                case 7:
                    temp = Vector3.Lerp(thirdPosition, finalPosition, lerpInc);
                    trackingCamera.transform.position = temp;
                    lerpInc += 0.01f;

                    if(lerpInc >= 1)
                    {
                        lerpInc = 0;
                        levelPhase++;
                    }
                    break;
                case 8:
                    Debug.Log("Attempting to load Level: " + "Level" + (Variables.lastLevel + 1));
                    LoadScene("Level" + (Variables.lastLevel + 1));
                    break;
                case 9:
                    break;
                default:
                    Debug.Log("Unexpected level phase during level-level transition! LevelPhase: " + levelPhase);
                    break;
            }
        }
    }

    // Start is called before the first frame update
    public void LoadScene(string sceneName)
    {
        //Implement any required transition logic here...

        SceneManager.LoadScene(sceneName);
        Variables.currentScene = sceneName;
    }

    public void TransitionIntoCube()
    {
        cubeTransform = GameObject.Find("RubiksCube").transform.position;
        trackingCamera = GameObject.Find("Main Camera");
        cameraOrigin = trackingCamera.transform.position;
        origRotation = trackingCamera.transform.rotation.eulerAngles;
        secondPosition = new Vector3(0, cubeTransform.y + 5f, 0);
        finalPosition = new Vector3(0, cubeTransform.y + 0.85f, 0);
        transitioning = true;

        GameObject.Find("Camera").GetComponent<MenuCameraMovement>().StartTransition();
        GameObject.Find("Canvas").SetActive(false);
    }

    public void ExitCube()
    {
        initialCubeTransform = GameObject.Find("Rubik'sCube").transform.position;
        cubeTransform = GameObject.Find("RubiksCube").transform.position;
        levelTransition = true;
        lookAtRotation = new Vector3(10, 90, 0);
        finalRotation = new Vector3(90, 90, 0);
        trackingCamera = GameObject.Find("Main Camera");
        origRotation = trackingCamera.transform.rotation.eulerAngles;
        cameraOrigin = trackingCamera.transform.position;
        finalPosition = new Vector3(cubeTransform.x, cubeTransform.y + 0.85f, cubeTransform.z);
        thirdPosition = new Vector3(cubeTransform.x, cubeTransform.y + 5f, cubeTransform.z);
        secondPosition = new Vector3(initialCubeTransform.x, initialCubeTransform.y + 5f, initialCubeTransform.z);
        finalCenterPosition = new Vector3(initialCubeTransform.x, initialCubeTransform.y + 1f, initialCubeTransform.z);
        centerBit = GameObject.Find("CenterBit");
    }

    public void LevelToLevel()
    {
        proceedWithTransition = true;
    }

    public void Restart()
    {
        Variables.isEditMode = true;
        Variables.inEscMenu = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        LoadScene(Variables.currentScene);
    }

    public void QuitToMain()
    {
        Variables.isEditMode = true;
        Variables.inEscMenu = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        LoadScene("MainMenu");
    }

    public void ShowControls()
    {
        controlCanvas.enabled = true;
    }

    public void HideControls()
    {
        controlCanvas.enabled = false;
    }

    public void quit()
    {
        Application.Quit();
    }

}
