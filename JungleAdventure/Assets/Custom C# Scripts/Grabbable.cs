using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Grabbable : MonoBehaviour
{
    private Interactable interactable;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    private void HandHoverUpdate(Hand hand)
    {
        var type = hand.GetGrabStarting();

        bool grabEnding = hand.IsGrabEnding(gameObject);

        if (interactable.attachedToHand == null && type != GrabTypes.None)
        {
            hand.AttachObject(gameObject, type);
            hand.HoverLock(interactable);
        }
        else if (grabEnding)
        {
            hand.DetachObject(gameObject);
            hand.HoverUnlock(interactable);
        }
    }
}
