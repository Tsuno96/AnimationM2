using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particule : MonoBehaviour
{
    public Vector3 vec3Velocity;
    public Vector3 vec3PreviousPos;
    
    void Start()
    {
        vec3Velocity = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));
    }

}
