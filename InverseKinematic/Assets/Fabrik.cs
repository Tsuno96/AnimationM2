using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Fabrik : MonoBehaviour
{
    public LineRenderer lr;
    public Transform target;
    Vector3 start;
    List<float> lines;
    public float sumLines;

    // Start is called before the first frame update
    void Start()
    {
        lines = new List<float>();
        lr = gameObject.GetComponent<LineRenderer>();
        start = lr.GetPosition(0);

        sumLines = 0;
        for(int i =1; i<lr.positionCount; i++)
        {
            lines.Add((lr.GetPosition(i) - lr.GetPosition(i - 1)).magnitude);
            sumLines += lines[lines.Count-1];
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 clickPosition = -Vector3.one;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            clickPosition = hit.point;
            //Debug.Log(clickPosition);
            target.position = clickPosition;
        }

        

        //Backward
        lr.SetPosition(lr.positionCount - 1, target.position);
        for(int i = lr.positionCount-2; i>=0;i--)
        {
            Vector3 pos = lr.GetPosition(i) - lr.GetPosition(i+1);
            pos = Vector3.Normalize(pos) * lines[i];
            lr.SetPosition(i, pos + lr.GetPosition(i+1));
        }

        //Forward
        lr.SetPosition(0, start);
        for(int i = 1; i<lr.positionCount;i++)
        {
            Vector3 pos = lr.GetPosition(i) - lr.GetPosition(i-1);
            pos = Vector3.Normalize(pos) * lines[i - 1];
            lr.SetPosition(i, pos + lr.GetPosition(i -1));
        }


    
    }

    private void GoToTarget()
    {


    }

}
