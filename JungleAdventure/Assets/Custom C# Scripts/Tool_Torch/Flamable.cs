using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamable : MonoBehaviour
{
    public float BurnTime = 5.0f;
    public bool Burning = false;
    public bool Burned = false;

    private float BurnStartTime = float.MinValue;

    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;

        mat.SetFloat("_DissolveAmount", -1);

        if (!gameObject.CompareTag("Flamable"))
            gameObject.tag = "Flamable";
    }

    // Update is called once per frame
    void Update()
    {
        if (Burning && !Burned)
        {
            if (BurnStartTime == float.MinValue)
                BurnStartTime = Time.time;

            float amount = Time.time - BurnStartTime;

            mat.SetFloat("_DissolveAmount", map(amount, 0, BurnTime, 0, 1));

            if (amount > BurnTime)
            {
                Burned = true; // Finished burning
            }
        }

        if (Burned)
            gameObject.SetActive(false);
    }

    private float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    public void Ignite()
    {
        if (!Burning && !Burned)
            Burning = true;
    }
}
