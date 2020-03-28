using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Pointer : MonoBehaviour
{
    public float DefaultLength = 5f;
    public GameObject End;
    public InputModule inputModule;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        float length = DefaultLength;

        var hit = CreateRaycast(length);

        var position = transform.position + (transform.forward * length);

        if (hit.collider != null)
            position = hit.point;

        End.transform.position = position;

        lineRenderer.SetPositions(new Vector3[] { transform.position, position });
    }

    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        var ray = new Ray(transform.position, transform.forward);

        Physics.Raycast(ray, out hit, DefaultLength);

        return hit;
    }
}
