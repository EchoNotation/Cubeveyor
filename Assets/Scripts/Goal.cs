using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Transform sceneController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Payload"))
        {
            //Complete level
            sceneController.GetComponent<SceneControl>().LoadScene("MainMenu");
        }
    }
}
