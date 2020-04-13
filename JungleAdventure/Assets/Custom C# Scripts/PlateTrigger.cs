using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateTrigger : MonoBehaviour
{
    public Vector3 pressedPosition;
    public Vector3 unpressedPosition;
    public Vector3 EjectionForce;
    public List<GameObject> pressors;

    [Range(0.001f, 1f)]
    public float transitionSpeed;
    private void OnCollisionEnter(Collision other)
    {
        if (pressors.Contains(other.gameObject))
            gameObject.tag = "Pressed";
    }

    private void OnCollisionExit(Collision other)
    {
        if (pressors.Contains(other.gameObject))
            gameObject.tag = "Untagged";
    }

    private void Update()
    {
        var target = (gameObject.CompareTag("Pressed") ? pressedPosition : unpressedPosition);

        if (gameObject.transform.position != target)
            ComputePosition(target);
    }

    public void Eject()
    {
        foreach (var pressor in pressors)
        {
            var rb = pressor.GetComponent<Rigidbody>();

            if (rb != null)
                rb.velocity += EjectionForce;
        }
    }

    private void ComputePosition(Vector3 target)
    {
        //compute mins and maxes
        float xMin = Mathf.Min(pressedPosition.x, unpressedPosition.x);
        float xMax = Mathf.Max(pressedPosition.x, unpressedPosition.x);
        float yMin = Mathf.Min(pressedPosition.y, unpressedPosition.y);
        float yMax = Mathf.Max(pressedPosition.y, unpressedPosition.y);
        float zMin = Mathf.Min(pressedPosition.z, unpressedPosition.z);
        float zMax = Mathf.Max(pressedPosition.z, unpressedPosition.z);

        //1 or 0 based on if we are altering that component
        var transform = new Vector3()
        {
            x = (gameObject.transform.position.x == target.x ? 0 : transitionSpeed),
            y = (gameObject.transform.position.y == target.y ? 0 : transitionSpeed),
            z = (gameObject.transform.position.z == target.z ? 0 : transitionSpeed)
        };

        //apply time difference
        transform *= Time.deltaTime * 0.5f;

        //apply direction
        transform.x *= (gameObject.transform.position.x > target.x ? -1 : 1);
        transform.y *= (gameObject.transform.position.y > target.y ? -1 : 1);
        transform.z *= (gameObject.transform.position.z > target.z ? -1 : 1);

        //get proposed position
        var position = transform + gameObject.transform.position;

        //validate
        transform.x = CheckRange(transform.x, xMin, xMax);
        transform.y = CheckRange(transform.y, yMin, yMax);
        transform.z = CheckRange(transform.z, zMin, zMax);

        //apply position
        gameObject.transform.position = position;
    }

    private float CheckRange(float val, float min, float max)
    {
        if (val > max)
            return max;

        if (val < min)
            return min;

        return val;
    }
}