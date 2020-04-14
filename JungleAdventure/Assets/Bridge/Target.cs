using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public bool down;

    public bool movingDown;

    public bool movingUp;

    public float speedUp;

    public float speedDown;

    public Vector3 endRotation;
    
    public Vector3 startRotation;

    private Vector3 changeInRotation;

    private Vector3 prevRotation;

    private float time;

    public GameObject TargetArea;

    private TargetHit hit;
    
    
    // Start is called before the first frame update
    void Start()
    {
        down = false;
        movingDown = false;
        movingUp = false;
        time = 0;
        changeInRotation = Vector3.forward;
        hit = TargetArea.GetComponent<TargetHit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hit.Hit)
        {
            down = true;
            movingDown = true;
            hit.Hit = false;
        }
        if (down)
        {
            prevRotation = transform.localEulerAngles;
            time += Time.deltaTime;
            if (movingDown && transform.localEulerAngles != endRotation)
            {
                transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, speedDown * time);
            }

            if (movingDown && changeInRotation == Vector3.zero)
            {
                time = 0;
                movingDown = false;
                movingUp = true;
            }

            if (movingUp && transform.localEulerAngles != startRotation)
            {
                transform.localEulerAngles = Vector3.Lerp(endRotation, startRotation, speedUp * time);
            }

            if (movingUp && transform.localEulerAngles == startRotation)
            {
                time = 0;
                down = false;
                movingUp = false;
            }

            changeInRotation = transform.localEulerAngles - prevRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Substring(0, 5) == "Arrow")
        {
            down = true;
            movingDown = true;
        }
    }
}
