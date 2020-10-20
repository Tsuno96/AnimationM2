using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Point prefabPoint;
    public List<Point> lstPoints;
    public int nPoints;
    public Vector3 vec3Bounds;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < nPoints; i++)
        {
            Point p = prefabPoint;
            Vector3 posGenerated = new Vector3(Random.Range(-vec3Bounds.x, vec3Bounds.x), Random.Range(-vec3Bounds.y, vec3Bounds.y), Random.Range(-vec3Bounds.z, vec3Bounds.z));
            p.Initialise(10f, posGenerated, vec3Bounds);
            Instantiate(p, p.GetPos(), Quaternion.identity);
            p.name = "Point" + i;
            lstPoints.Add(p);
            p.Gravity();
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
}
