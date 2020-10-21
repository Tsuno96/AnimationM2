using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;


public class PhysicsManager : MonoBehaviour
{

    private List<Particule> lstParticules;
    private Vector3 vec3Gravity = new Vector3(0,-9.8f,0);
    private Vector3 vec3Boundaries;
    [Range(0.01f,1)]
    public float fTimeScale;
    [Range(0, 10)]
    public float fk;
    [Range(0, 10)]
    public float fknear;
    [Range(1, 10)]
    public float fdensity;
    [Range(1, 10)]
    public float fradiusCohesion;

    void Start()
    {
        Spawner sp = GetComponent<Spawner>();
        lstParticules = sp.lstParticules;
        vec3Boundaries = sp.vec3Bounds;
    }

    void Update()
    {
        Gravity();
        Viscosite();

        CheckBounds();
        
    }
    public void Gravity()
    {
        foreach (Particule p in lstParticules)
        {
            p.vec3Velocity += fTimeScale * vec3Gravity;
            //p.transform.position += p.vec3Velocity * fTimeScale;
        }
    }


    void CheckBounds()
    {
        foreach (Particule p in lstParticules)
        {
            Vector3 pos = p.transform.position;                     
            bool X = pos.x >= vec3Boundaries.x || pos.x <= -vec3Boundaries.x;
            bool Y = pos.y >= vec3Boundaries.y || pos.y <= -vec3Boundaries.y;
            bool Z = pos.z >= vec3Boundaries.z || pos.z <= -vec3Boundaries.z;

            if (X || Y || Z)
            {
                pos.x = Mathf.Clamp(p.transform.position.x, -vec3Boundaries.x, vec3Boundaries.x);
                pos.y = Mathf.Clamp(p.transform.position.y, -vec3Boundaries.y, vec3Boundaries.y);
                pos.z = Mathf.Clamp(p.transform.position.z, -vec3Boundaries.x, vec3Boundaries.z);
                p.transform.position = pos;
                p.vec3Velocity *= -0.25f;
            }
        }
        
    }


    void Viscosite()
    {
        foreach (Particule p in lstParticules)
        {
            p.vec3PreviousPos = p.transform.position;
            p.transform.position += p.vec3Velocity * fTimeScale;
        }

        DoubleDensiteRelaxation();
        
        foreach (Particule p in lstParticules)
        {
            p.vec3Velocity = (p.transform.position - p.vec3PreviousPos) / fTimeScale;
        }
    }

    void DoubleDensiteRelaxation()
    {
        foreach (Particule p in lstParticules)
        {
            float densite = 0;
            float densiteNear = 0;

            List<Particule> Neighbors = GetNeighbors(p);

            foreach (Particule np in Neighbors)
            {
                float q = Vector3.Distance(np.transform.position, p.transform.position) / fradiusCohesion;
                if (q < 1)
                {
                    densite += Mathf.Pow(1 - q, 2);
                    densiteNear += Mathf.Pow(1 - q, 3);
                }
            }

            float pressure = fk * (densite - fdensity);
            float pressureNear = fknear * densiteNear;

            Vector3 dx = Vector3.zero;

            foreach (Particule np in Neighbors)
            {
                float q = Vector3.Distance(np.transform.position, p.transform.position) / fradiusCohesion;
                if (q < 1)
                {
                    Vector3 D = Mathf.Pow(fTimeScale, 2) * (pressure * (1 - q) + pressureNear * Mathf.Pow(1 - q, 2)) * (np.transform.position - p.transform.position).normalized;
                    np.transform.position += D / 2;
                    dx -= D / 2;
                }
            }

            p.transform.position += dx;//WOWOWOWOWOWOW
        }
    }

    List<Particule> GetNeighbors(Particule pO)
    {
        List<Particule> tmpN = new List<Particule>();
        foreach (Particule p in lstParticules)
        {
            if(Vector3.Distance(pO.transform.position,p.transform.position)<=fradiusCohesion)
            {
                tmpN.Add(p);
            }
        }
        return tmpN;
    }
}
