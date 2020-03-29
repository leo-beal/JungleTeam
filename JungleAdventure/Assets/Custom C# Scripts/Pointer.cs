using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Pointer : MonoBehaviour
{
    public float DefaultLength = 5f;
    public GameObject End;
    //public InputModule inputModule;
    public SteamVR_Action_Boolean Activator;
    [Range(1f, 30f)]
    public float ActivatedTime;
    public Hand hand;

    private LineRenderer lineRenderer;
    private bool isActive;
    private float elapsedTime;

    private void Awake()
    {
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        isActive = false;
        elapsedTime = 0;
    }

    void Update()
    {
        //player pressed the button
        if (Activator.active && Activator.state && Activator.activeDevice.ToString() == hand.name)
        {
            if (hand.AttachedObjects.Count == 0)
            {
                var hit = UpdateLine();

                if (isActive && hit.collider != null)
                {
                    //we hit something. check if it is interactable.
                    var rootObject = hit.collider.attachedRigidbody?.transform?.root?.gameObject;

                    if (rootObject != null)
                    {
                        var interactable = rootObject.GetComponent<Interactable>();

                        if (interactable != null)
                        {
                            //attach to hand
                            hand.AttachObject(rootObject, GrabTypes.Grip);
                            hand.HoverLock(interactable);

                            //remove raycast
                            DestroyLine();
                        }
                    }
                }
            }
            else
            {
                DestroyLine();
            }
        }
        else if (isActive)
        {
            UpdateLine();
        }
        
        if (elapsedTime > ActivatedTime)
        {
            DestroyLine();
        }
    }

    void UpdateTime()
    {
        if (!isActive)
        {
            isActive = true;
            elapsedTime = Time.deltaTime;
        }
        else
        {
            elapsedTime += Time.deltaTime;
        }
    }

    RaycastHit UpdateLine()
    {
        UpdateTime();

        float length = DefaultLength;

        var hit = CreateRaycast(length);

        var position = transform.position + (transform.forward * length);

        if (hit.collider != null)
            position = hit.point;

        End.transform.position = position;

        lineRenderer.SetPositions(new Vector3[] { transform.position, position });
        lineRenderer.enabled = true;
        End.SetActive(true);

        return hit;
    }

    void DestroyLine()
    {
        isActive = false;
        elapsedTime = 0;
        lineRenderer.enabled = false;
        End.SetActive(false);
    }

    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        var ray = new Ray(transform.position, transform.forward);

        Physics.Raycast(ray, out hit, DefaultLength);

        return hit;
    }
}
