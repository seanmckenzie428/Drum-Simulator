using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class MovementRecorder : MonoBehaviour
{
    
    public List<Vector3> positions;
    public List<Quaternion> rotations;

    public void Record()
    {
        positions.Add(transform.position);
        rotations.Add(transform.rotation);
    }

    public bool Restore(int point)
    {
        if (point < positions.Count && point < rotations.Count) 
        {
            var t = transform;
            t.position = positions[point];
            t.rotation = rotations[point];
            return true;
        }

        return false;
    }

    public void SaveRecording()
    {
        BinaryFormatter bf = new BinaryFormatter(); 
        FileStream file = File.Create(Application.persistentDataPath 
                                      + "/" + gameObject.name + ".dat"); 
        PosRot data = new PosRot();
        data.positions = positions;
        data.rotations = rotations;
        
        string json = JsonUtility.ToJson(data);

        Debug.Log(json);
        
        bf.Serialize(file, json);
        file.Close();
        Debug.Log("Game data saved!");
    }
    
    
    public void LoadRecording()
    {
        if (File.Exists(Application.persistentDataPath 
                        + "/" + gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = 
                File.Open(Application.persistentDataPath 
                          + "/" + gameObject.name + ".dat", FileMode.Open);
            string json = (string)bf.Deserialize(file);

            Debug.Log(json);
            PosRot data = JsonUtility.FromJson<PosRot>(json);
            file.Close();
            positions = data.positions;
            rotations = data.rotations;
            Debug.Log("Game data loaded!");
        }
        else
            Debug.LogError("There is no save data!");
    }
}

[Serializable]
public class PosRot
{
    public List<Vector3> positions;
    public List<Quaternion> rotations;
}