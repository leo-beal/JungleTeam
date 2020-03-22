using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Vector3 openPosition;
    public Vector3 closePosition;
    public List<GameObject> plates;

    public bool initializeAsClosed;
    public bool remainOpen;

    [Range(0.001f, 1f)]
    public float transitionSpeed;

    private bool isOpen;
    private void Awake()
    {
        if (initializeAsClosed)
            gameObject.transform.position = closePosition;
        else
            gameObject.transform.position = openPosition;
    }

    private void Update()
    {
        Vector3 target = openPosition;

        foreach (var plate in plates)
        {
            if (isOpen && remainOpen)
                break; //stay at open position since we want to stay open

            if (!plate.CompareTag("Pressed"))
            {
                target = closePosition;
                break;
            }
        }

        if (target == openPosition)
            isOpen = true;

        if (gameObject.transform.position != target)
            ComputePosition(target);
    }

    private void ComputePosition(Vector3 target)
    {
        //compute mins and maxes
        float xMin = Mathf.Min(openPosition.x, closePosition.x);
        float xMax = Mathf.Max(openPosition.x, closePosition.x);
        float yMin = Mathf.Min(openPosition.y, closePosition.y);
        float yMax = Mathf.Max(openPosition.y, closePosition.y);
        float zMin = Mathf.Min(openPosition.z, closePosition.z);
        float zMax = Mathf.Max(openPosition.z, closePosition.z);

        //1 or 0 based on if we are altering that component
        var transform = new Vector3()
        {
            x = (gameObject.transform.position.x == target.x ? 0 : transitionSpeed),
            y = (gameObject.transform.position.y == target.y ? 0 : transitionSpeed),
            z = (gameObject.transform.position.z == target.z ? 0 : transitionSpeed)
        };

        //apply time difference
        transform *= Time.deltaTime;

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