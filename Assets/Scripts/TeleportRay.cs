using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportRay : MonoBehaviour {
	public OVRInput.Controller controller;
	public Transform avatar;
	public Transform camera;
    public int numPoints = 75;
    public float resolution = 0.1f;
    public float gravity = 0.2f;
	public float teleportThresh = 0.6f;
    public Vector3 offset = new Vector3(0, 0, 0);
   
	private LineRenderer lineRenderer;
	private Vector3 contactPt = new Vector3(0, 0, 0);
	private bool foundContact = false;

    // Use this for initialization
    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
    }
    
    // Update is called once per frame
    void Update () {
		if (OVRInput.Get(OVRInput.Button.One, controller))
        {
			foundContact = false;
			Ray ray = new Ray(transform.position, transform.forward);
            Vector3 a = transform.position + offset;
            Vector3 b = ray.GetPoint(resolution) + offset;
            Vector3 delta = b - a;

            Vector3[] pos = new Vector3[numPoints];
            pos[0] = a; pos[1] = b;

            float ressqr = delta.x * delta.x + delta.z * delta.z;
            for (int i = 2; i < numPoints; ++i)
            {
                Vector3 v;
                v.x = a.x + i * delta.x;
                v.z = a.z + i * delta.z;
                v.y = a.y + i * delta.y - i * i * ressqr * gravity;
                pos[i] = v;

				if (!foundContact && v.y <= 0) {
					foundContact = true;
					contactPt = v;
				}
            }

            lineRenderer.positionCount = numPoints;
            lineRenderer.SetPositions(pos);
        }
        else
        {
			if (lineRenderer.positionCount > 0) {
				lineRenderer.positionCount = 0;
				lineRenderer.SetPositions (new Vector3[]{ });

				if (foundContact) {
					TeleportPoint.TeleportTo(avatar, contactPt, teleportThresh);
					camera.position = avatar.position;
				}
			}
        }
    }
}
