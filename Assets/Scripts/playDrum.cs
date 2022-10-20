using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playDrum : MonoBehaviour
{
    public AudioClip beat;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().clip = beat;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<AudioSource>().Play();
        Debug.Log("drum hit");
    }
}
