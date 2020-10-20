using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;


public class Point : MonoBehaviour
{
    public float fMasse;
    public Vector3 gravity;
    public Vector3 vec3Pos;
    public Vector3 vec3Acceleration;
    public Vector3 vec3Velocity;
    public bool bGravity;
    Vector3 vec3Bounds;
    List<Point> lstptNghbrs;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Initialise(float m, Vector3 pos, Vector3 Bounds)
    {
        lstptNghbrs = new List<Point>();
        fMasse = m;
        vec3Pos = pos;
        vec3Acceleration = Vector3.zero;
        vec3Velocity = new Vector3(0.1f,0,50);
        gravity = new Vector3(0,9.8f,0);
        bGravity = true;
        vec3Bounds = vec3Velocity;

    }
    public Vector3 GetPos()
    {
        return vec3Pos;
    }
    // Update is called once per frame
    void Update()
    {
        if(bGravity)
        { 
            Gravity();
        }
    }
    public void Gravity()
    {
        calcA();
        calcV();
        calcP();
        checkBounds();
    }
    void calcA()
    {
        vec3Acceleration = gravity * -fMasse;
        vec3Acceleration = (vec3Acceleration - vec3Velocity * vec3Velocity.magnitude) * (1 / fMasse);
    }
    void calcV()
    {
        vec3Velocity +=  vec3Acceleration * Time.deltaTime;
    }
    void calcP()
    {
        vec3Pos += vec3Velocity * Time.deltaTime;
        transform.position = vec3Pos;
    }
    void checkBounds()
    {
        if(vec3Pos.y <= -vec3Bounds.y)
        {
            vec3Velocity = Vector3.Cross(new Vector3(1,-1,1),vec3Velocity);
            transform.position = new Vector3(transform.position.x, -vec3Bounds.y, transform.position.z);
            bGravity = false;
        }
    }

    void tbljnfltg()
    {
        float density = 0;
        float nearDensity = 0;


    }
}
