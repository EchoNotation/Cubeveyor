using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] musicSources = GameObject.FindGameObjectsWithTag("MusicManager");

        if(musicSources.Length > 1)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
