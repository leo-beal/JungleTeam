using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Inventory : MonoBehaviour
{
    private Queue<Interactable> objects = new Queue<Interactable>();

    public SteamVR_Action_Boolean addToInventory;
    public SteamVR_Action_Boolean pullFromInventory;
    public Hand hand;

    public Canvas canvas;
    public void Update()
    {
        Interactable toAdd = null;

        if (hand.AttachedObjects.Count == 1)
            toAdd = hand.AttachedObjects[0].interactable;

        if (addToInventory.active && addToInventory.stateDown)
        {
            if (toAdd != null)
            {
                hand.DetachObject(toAdd.gameObject);
                hand.HoverUnlock(toAdd);

                toAdd.gameObject.SetActive(false);

                objects.Enqueue(toAdd);

                toAdd = null;
            }
            else
            {
                //display list of objects for player to grab.
            }
        }
        else if (pullFromInventory.active && pullFromInventory.stateDown)
        {
            if (toAdd != null)
            {
                hand.DetachObject(toAdd.gameObject);
                hand.HoverUnlock(toAdd);

                toAdd.gameObject.SetActive(false);

                objects.Enqueue(toAdd);

                toAdd = null;
            }

            if (objects.Count > 0)
            {
                var grab = objects.Dequeue();

                grab.gameObject.SetActive(true);

                hand.AttachObject(grab.gameObject, GrabTypes.Grip);
                hand.HoverLock(toAdd);
            }
        }
    }
}
