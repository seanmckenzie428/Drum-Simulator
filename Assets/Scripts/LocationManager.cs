using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationManager : MonoBehaviour
{

    [SerializeField] private Button recordButton;
    [SerializeField] private Button playbackButton;
    [SerializeField] private bool isRecording;
    [SerializeField] private bool isPlayback = true;
    [SerializeField] private GameObject[] objectsToRecord;
    public float recordInterval = 0.1f;
    private float _lastTimeRecorded = 0;
    private float _lastTimePositionSet = 0;
    private int _playbackCounter = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        objectsToRecord = GameObject.FindGameObjectsWithTag("record");
    }

    // Update is called once per frame
    void Update()
    {
        if (isRecording)
        {
            if (_lastTimeRecorded + recordInterval <= Time.time)
            {
                foreach (var o in objectsToRecord)
                {
                    o.GetComponent<Rigidbody>().isKinematic = false;
                    var recorder = o.GetComponent<MovementRecorder>();
                    recorder.Record();
                }

                _lastTimeRecorded = Time.time;
            }

        } else if (isPlayback)
        {
            if (_lastTimePositionSet + recordInterval <= Time.time)
            {
                foreach (var o in objectsToRecord)
                {
                    o.GetComponent<Rigidbody>().isKinematic = true;
                    o.GetComponent<MovementRecorder>().Restore(_playbackCounter);
                }

                _playbackCounter++;
                _lastTimePositionSet = Time.time;
            }
        }
        
        
    }

    public void ToggleRecording()
    {
        Debug.Log("ToggleRecording");
        Debug.Log(isRecording);
        isRecording = !isRecording;
    }

    public void TogglePlayback()
    {
        Debug.Log("TogglePlayback");
        Debug.Log(isPlayback);
        isPlayback = !isPlayback;
        _playbackCounter = 0;
    }

    public void ToggleUIButtonActive()
    {
        if (!isRecording)
        {
            recordButton.gameObject.SetActive(true);
            playbackButton.gameObject.SetActive(false);
        }
        else
        {
            playbackButton.gameObject.SetActive(true);
            recordButton.gameObject.SetActive(false);
        }
    }
    
    
}
