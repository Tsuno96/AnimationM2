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
    [Range(0f,180f)]
    public float angleConstraint;
    public float[] angles;

    // Start is called before the first frame update
    void Start()
    {
        lines = new List<float>();
        lr = gameObject.GetComponent<LineRenderer>();
        angles = new float[lr.positionCount-2];
        start = lr.GetPosition(0);

        sumLines = 0;
        for(int i =1; i<lr.positionCount; i++)
        {
            lines.Add((lr.GetPosition(i) - lr.GetPosition(i - 1)).magnitude);
            sumLines += lines[lines.Count-1];
        }
        GetAngles();

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


        GoToTargetConstraint();
        GetAngles();
        //AjustCont();


    
    }

    private void GoToTarget()
    {
        //Backward
        lr.SetPosition(lr.positionCount - 1, target.position);
        for (int i = lr.positionCount - 2; i >= 0; i--)
        {
            Vector3 pos = lr.GetPosition(i) - lr.GetPosition(i + 1);
            pos = Vector3.Normalize(pos) * lines[i];
            lr.SetPosition(i, pos + lr.GetPosition(i + 1));
        }

        //Forward
        lr.SetPosition(0, start);
        for (int i = 1; i < lr.positionCount; i++)
        {
            Vector3 pos = lr.GetPosition(i) - lr.GetPosition(i - 1);
            pos = Vector3.Normalize(pos) * lines[i - 1];
            lr.SetPosition(i, pos + lr.GetPosition(i - 1));
        }
    }

    void GoToTargetConstraint()
    {
        //Backward
        lr.SetPosition(lr.positionCount - 1, target.position);
        for (int i = lr.positionCount - 2; i >= 0; i--)
        {
            Vector3 pos = lr.GetPosition(i) - lr.GetPosition(i + 1);
            pos = Vector3.Normalize(pos) * lines[i];
            lr.SetPosition(i, pos + lr.GetPosition(i + 1));
            if(i < lr.positionCount -2)
            {
                Vector3 a = lr.GetPosition(i) - lr.GetPosition(i+1);
                Vector3 b = lr.GetPosition(i + 2) - lr.GetPosition(i+1);
                float ag = SignedAngleBetween(a, b, Vector3.Cross(a, b));
                if(ag < angleConstraint)
                {
                    Vector3 CB = b.normalized;

                    Vector3 v = CB;
                    Vector3 v1 = Quaternion.AngleAxis(angleConstraint, Vector3.forward) * v;
                    Vector3 v2 = Quaternion.AngleAxis(-angleConstraint, Vector3.forward) * v;
                    Vector3 pos1 = lr.GetPosition(i + 1) + v1;
                    Vector3 pos2 = lr.GetPosition(i + 1) + v2;
                    if(Vector3.Distance(lr.GetPosition(i),pos1)> Vector3.Distance(lr.GetPosition(i), pos2))
                    {
                        pos = pos2;
                    }
                    else
                    {
                        pos = pos1;
                    }

                    lr.SetPosition(i, pos);
                }
            }
        }

        //Forward
        lr.SetPosition(0, start);
        for (int i = 1; i < lr.positionCount; i++)
        {
            Vector3 pos = lr.GetPosition(i) - lr.GetPosition(i - 1);
            pos = Vector3.Normalize(pos) * lines[i - 1];
            lr.SetPosition(i, pos + lr.GetPosition(i - 1));
            if (i > 1)
            {
                Vector3 a = lr.GetPosition(i) - lr.GetPosition(i-1);
                Vector3 b = lr.GetPosition(i-2) - lr.GetPosition(i-1);
                float ag = SignedAngleBetween(a, b, Vector3.Cross(a, b));
                if (ag < angleConstraint)
                {
                    Vector3 CB = b.normalized;

                    Vector3 v = CB;
                    Vector3 v1 = Quaternion.AngleAxis(angleConstraint, Vector3.forward) * v;
                    Vector3 v2 = Quaternion.AngleAxis(-angleConstraint, Vector3.forward) * v;
                    Vector3 pos1 = lr.GetPosition(i-1) + v1;
                    Vector3 pos2 = lr.GetPosition(i-1) + v2;
                    if (Vector3.Distance(lr.GetPosition(i), pos1) > Vector3.Distance(lr.GetPosition(i), pos2))
                    {
                        pos = pos2;
                    }
                    else
                    {
                        pos = pos1;
                    }

                    lr.SetPosition(i, pos);
                }
            }
        }

    }

    void GetAngles()
    {
        for (int i = 1; i < lr.positionCount-1; i++)
        {
            Vector3 a = lr.GetPosition(i-1) - lr.GetPosition(i);
            Vector3 b = lr.GetPosition(i+1) - lr.GetPosition(i);
            angles[i-1] = SignedAngleBetween(a, b, Vector3.Cross(a,b));
        }
    }
    float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
    {
        //angle in [0,180]
        float angle = Vector3.Angle(a, b);
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));

        //angle in [-179,180]
        float signed_angle = angle * sign;

        //angle in [0,360] (not used but included here for completeness)
        //float angle360 =  (signed_angle + 180) % 360;

        return signed_angle;
    }
    void AjustCont()
    {
        for (int i = 1; i < lr.positionCount - 1; i++)
        {
            Vector3 a = lr.GetPosition(i - 1) - lr.GetPosition(i);
            Vector3 b = lr.GetPosition(i + 1) - lr.GetPosition(i);
            float ag = SignedAngleBetween(a, b, Vector3.Cross(a, b));
            if(ag < angleConstraint)
            {
                float h =  lines[i-1];
                Vector3 pos = new Vector3(Mathf.Cos(angleConstraint) * h + lr.GetPosition(i - 1).x, Mathf.Sin(angleConstraint) * h + lr.GetPosition(i - 1).y, 0);
                lr.SetPosition(i + 1, pos);
            }
        }
    }
}
