﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Fire : MonoBehaviour
{
    public float BurnTime = 5.0f;

    private bool burned;

    Flicker flicker;
    Flamable flamable;
    // Start is called before the first frame update
    void Start()
    {
        flicker = GetComponent<Flicker>();
        
        if (flicker != null)
            flicker.enabled = this.enabled;
    }

    private void OnDisable()
    {
        if (flicker != null)
            flicker.enabled = false;
    }

    private void OnEnable()
    {
        if (flicker != null)
            flicker.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!this.enabled)
            return;

        if (collision.collider.CompareTag("Flamable"))
        {
            flamable = collision.collider.gameObject.GetComponent<Flamable>();
            burned = true;
        }
        if (collision.gameObject.name.Substring(0, 5) == "Arrow")
        {
            ArrowScript arrow = collision.gameObject.GetComponent<ArrowScript>();
            arrow.flaming = true;
        }
    }

    private void Update()
    {
        if (burned)
        {
            if (flamable != null)
                flamable.Ignite();
        }
    }
}
