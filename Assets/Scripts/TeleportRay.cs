using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportRay : MonoBehaviour {

    public int numPoints = 50;
    public float resolution = 0.1f;
    public float gravity = 0.05f;
    public Vector3 offset = new Vector3(0, 0.1f, 0);
    private LineRenderer lineRenderer;

    // Use this for initialization
    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
    }
    
    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 a = Camera.main.transform.position + offset;
            Vector3 b = ray.GetPoint(resolution) + offset;
            Vector3 delta = b - a;

            Vector3[] pos = new Vector3[numPoints];
            pos[0] = a; pos[1] = b;

            float ressqr = delta.x * delta.x + delta.z * delta.z;

            AnimationCurve curve = new AnimationCurve();


            curve.AddKey(0.0f, 1.0f);
            curve.AddKey(1.0f, 1.0f);

            for (int i = 2; i < numPoints; ++i)
            {
                Vector3 v;
                v.x = a.x + i * delta.x;
                v.z = a.z + i * delta.z;
                v.y = a.y + i * delta.y - i * i * ressqr * gravity;
                pos[i] = v;
                curve.AddKey(i, Vector3.Distance(v, a) / 1000.0f);
            }
            lineRenderer.positionCount = numPoints;
            lineRenderer.widthCurve = curve;
            lineRenderer.SetPositions(pos);
        }
        else
        {
            //lineRenderer.positionCount = 0;
            //lineRenderer.SetPositions(new Vector3[]{ });
        }
    }
}
