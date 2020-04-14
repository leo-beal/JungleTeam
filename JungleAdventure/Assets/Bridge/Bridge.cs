using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public GameObject walkWay;

    public GameObject leftTarget;

    public GameObject rightTarget;

    public Vector3 start;

    public Vector3 end;

    public bool drop;

    public float speed;

    private Target left;

    private Target right;

    private float time;

    private Vector3 prevRotation;
    private Vector3 changeInRotation;
    // Start is called before the first frame update
    void Start()
    {
        drop = false;
        left = leftTarget.GetComponent<Target>();
        right = rightTarget.GetComponent<Target>();
        time = 0;
        changeInRotation = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        prevRotation = walkWay.transform.localEulerAngles;
        if (!drop && left.down && right.down && left.movingUp && right.movingUp)
        {
            drop = true;
        }

        if (drop)
        {
            time += Time.deltaTime;
            if (walkWay.transform.localEulerAngles != end)
            {
                walkWay.transform.localEulerAngles = Vector3.Lerp(start, end, speed * time);
            }

            if (walkWay.transform.localEulerAngles == end)
            {
                time = 0;
                drop = false;
            }
        }
        changeInRotation = walkWay.transform.localEulerAngles - prevRotation;
    }
}
