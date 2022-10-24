using System.Collections;
using System.Collections.Generic;
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
}

public class PosRot
{
    private Vector3 _pos;
    private Vector3 _rot;

    public PosRot(Vector3 pos, Vector3 rot)
    {
        this._pos = pos;
        this._rot = rot;
    }
}