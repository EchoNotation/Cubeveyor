using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraMovement : MonoBehaviour
{
    int i;

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(Mathf.Sin(i / 30) * 10, 0, 0), 10 * Time.deltaTime);  ;
        i++;
    }
}
