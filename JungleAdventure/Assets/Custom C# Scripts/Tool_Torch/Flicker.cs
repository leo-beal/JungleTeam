using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    [Range(0, 5)]
    public float duration = 1.0f;
    [Range(0, 100)]
    public float Intensity = 10f;
    public Color color0 = Color.red;
    public Color color1 = Color.yellow;

    Light lt;

    void Start()
    {
        lt = GetComponent<Light>();
        
        if (lt != null)
            lt.enabled = this.enabled;
    }

    private void OnDisable()
    {
        if (lt != null)
            lt.enabled = false;
    }

    private void OnEnable()
    {
        if (lt != null)
            lt.enabled = true;
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time, duration) / duration;
        if (lt != null)
        {
            lt.color = Color.Lerp(color0, color1, t);
            lt.intensity = Intensity;
        }
    }
}
