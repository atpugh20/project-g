using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public int length;
    public LineRenderer lineRenderer;
    public Vector3[] segmentPositions;
    public Vector3[] segmentVelocities;

    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;
    public float trailSpeed;

    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDir;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.positionCount = length;
        segmentPositions = new Vector3[length];
        segmentVelocities = new Vector3[length];
    }

    // Update is called once per frame
    void Update()
    {
        wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);
        segmentPositions[0] = targetDir.position;

        for (int i = 1; i < segmentPositions.Length; i++)
        {
            segmentPositions[i] = Vector3.SmoothDamp(segmentPositions[i], segmentPositions[i - 1] + targetDir.right * targetDist, ref segmentVelocities[i], smoothSpeed + i / trailSpeed);
        }

        lineRenderer.SetPositions(segmentPositions);
    }
}
