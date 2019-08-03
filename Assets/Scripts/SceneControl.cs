using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadScene(string sceneName)
    {
        //Implement any required transition logic here...

        SceneManager.LoadScene(sceneName);
    }

    public void TransitionIntoCube()
    {

    }
}
