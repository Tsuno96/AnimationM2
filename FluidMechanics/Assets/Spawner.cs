using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Particule prefabPoint;
    public List<Particule> lstParticules;
    public int nPoints;
    public Vector3 vec3Bounds;

    void Awake()
    {
        for (int i = 0; i < nPoints; i++)
        {
            Vector3 posGenerated = new Vector3(Random.Range(-vec3Bounds.x, vec3Bounds.x), Random.Range(-vec3Bounds.y, vec3Bounds.y), Random.Range(-vec3Bounds.z, vec3Bounds.z));
            Particule p = Instantiate(prefabPoint, posGenerated, Quaternion.identity);
            p.name = "Particule" + i;
            lstParticules.Add(p);
        }
    }

}
