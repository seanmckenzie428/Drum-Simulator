using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class LocationManager : MonoBehaviour
{

    [SerializeField] private Button recordButton;
    [SerializeField] private Button playbackButton;
    [SerializeField] private bool isRecording;
    [SerializeField] private bool isPlayback = true;
    [SerializeField] private GameObject[] objectsToRecord;
    [SerializeField] private TextMeshProUGUI recordingStatus;
    [SerializeField] private TextMeshProUGUI playbackStatus;
    private bool _hasRecording = false;
    public float recordInterval = 0.1f;
    private float _lastTimeRecorded = 0;
    private float _lastTimePositionSet = 0;
    private int _playbackCounter = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        objectsToRecord = GameObject.FindGameObjectsWithTag("record");
        foreach (var o in objectsToRecord)
        {
            o.GetComponent<Rigidbody>().isKinematic = true;
            o.GetComponent<MovementRecorder>().LoadRecording();
        }

        Debug.Log(recordingStatus);
        Debug.Log(playbackStatus);
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
                var isPlaying = false;
                foreach (var o in objectsToRecord)
                {
                    o.GetComponent<Rigidbody>().isKinematic = true;
                    isPlaying = o.GetComponent<MovementRecorder>().Restore(_playbackCounter);
                }
                _playbackCounter++;
                _lastTimePositionSet = Time.time;
                if (!isPlaying)
                {
                    TogglePlayback();
                }
            }
        }
        
        
    }

    public void ToggleRecording()
    {
        Debug.Log("ToggleRecording");
        Debug.Log(isRecording);
        isRecording = !isRecording;
        if (isRecording)
        {
            recordingStatus.text = "Recording...";
            recordingStatus.color = Color.red;
        }
        else
        {
            foreach (var o in objectsToRecord)
            {
                o.GetComponent<Rigidbody>().isKinematic = true;
            }
            _hasRecording = true;
            playbackStatus.text = "Playback ready";
            recordingStatus.text = "Recording finished";
            playbackStatus.color = Color.blue;
            recordingStatus.color = Color.blue;
        }
    }

    public void DeleteRecording()
    {
        Debug.Log("DeleteRecording");
        if (!isRecording)
        {
            foreach (var o in objectsToRecord)
            {
                o.GetComponent<MovementRecorder>().positions = new List<Vector3>();
                o.GetComponent<MovementRecorder>().rotations = new List<Quaternion>();
            }

            _hasRecording = false;
            isPlayback = false;
            playbackStatus.text = "";
            recordingStatus.text = "Ready to record";
            recordingStatus.color = Color.yellow;
        }
    }

    public void TogglePlayback()
    {
        Debug.Log("TogglePlayback");
        Debug.Log(isPlayback);
        isPlayback = !isPlayback;
        _playbackCounter = 0;
        if (isPlayback)
        {
            foreach (var o in objectsToRecord)
            {
                o.GetComponent<Rigidbody>().isKinematic = true;
            }
            playbackStatus.text = "Playing...";
            playbackStatus.color = Color.green;
        }
        else
        {
            playbackStatus.text = "Playback ready";
            playbackStatus.color = Color.blue;
        }

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

    public void SaveRecording()
    {
        foreach (var o in objectsToRecord)
        {
            o.GetComponent<MovementRecorder>().SaveRecording();
        }
    }


}
