using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    Flicker flicker;
    // Start is called before the first frame update
    void Start()
    {
        flicker = GetComponent<Flicker>();
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
            collision.collider.enabled = false;
            collision.gameObject.SetActive(false);
        }
    }
}
