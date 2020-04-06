using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Pointer : MonoBehaviour
{
    public float DefaultLength = 5f;
    public GameObject End;
    //public InputModule inputModule;
    public SteamVR_Action_Boolean Activator;

    public List<GameObject> Inventories;
    public SteamVR_Action_Boolean Selector;

    [Range(1f, 30f)]
    public float ActivatedTime;
    public Hand hand;

    private LineRenderer lineRenderer;
    private bool isActive;
    private float elapsedTime;
    private UIButton button;

    private void Awake()
    {
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        isActive = false;
        elapsedTime = 0;
        button = null;
    }

    void Update()
    {
        if (Selector.active && Selector.stateDown && Selector.activeDevice.ToString() == hand.name && button != null)
        {
            TrySelectFromInventory();
        }

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

    private void TrySelectFromInventory()
    {
        var root = button.item.transform.root.gameObject;
        var rootInteractable = root.GetComponent<Interactable>();

        foreach (var inventory in this.Inventories)
        {
            var list = inventory.GetComponent<Inventory>();

            if (list.objects.Contains(rootInteractable))
            {
                list.objects.Remove(rootInteractable);

                var attachedObjects = hand.AttachedObjects;

                foreach (var o in attachedObjects)
                {
                    var interactable = o.attachedObject.GetComponent<Interactable>();

                    if (interactable != null)
                    {
                        hand.DetachObject(rootInteractable.gameObject, false);
                        hand.HoverUnlock(rootInteractable);
                        list.objects.Add(rootInteractable);
                    }
                }

                root.SetActive(true);

                hand.AttachObject(root, GrabTypes.Grip);
                hand.HoverLock(rootInteractable);
            }

            list.RecreateCanvas();
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

    void ResetTime()
    {
        elapsedTime = 0;
    }

    RaycastHit UpdateLine()
    {
        UpdateTime();

        float length = DefaultLength;

        var hit = CreateRaycast(length);

        var position = transform.position + (transform.forward * length);

        if (hit.collider != null)
        {
            position = hit.point;

            var tempbutton = hit.collider.gameObject.GetComponent<UIButton>();

            if (tempbutton != null)
            {
                ResetTime();

                if (button != null)
                    button.RemoveHighlight();

                button = tempbutton;
                button.Highlight();
            }
            else if (button != null)
            {
                button.RemoveHighlight();
                button = null;
            }
        }

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
