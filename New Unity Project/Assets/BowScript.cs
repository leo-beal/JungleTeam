using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{

    [RequireComponent(typeof(Interactable))]
    public class BowScript : MonoBehaviour
    {
        private bool noched;
        private Hand hand;

        private Collider nochCollider;

        public GameObject nochFollow;

        public GameObject nochStart;

        public GameObject nochEnd;

        private GameObject currentArrow;
        private GameObject tail;
        
        private Vector3 arrowOffset = new Vector3(-5, 0, 0);
        // Start is called before the first frame update
        void Start()
        {
            nochCollider = GetComponent<Collider>();
            //currentArrow.transform.up = nochStart.transform.right;
            //currentArrow.transform.right = nochStart.transform.forward;
        }

        // Update is called once per frame
        void Update()
        {
            
            if (noched && hand.otherHand.currentAttachedObject != null && currentArrow != null)
            {
                //nochFollow.transform.position = tail.transform.position;
                currentArrow.transform.localPosition = new Vector3(0, 0, 5);
                currentArrow.transform.rotation = nochStart.transform.rotation;

                //nochFollow.transform.position = new Vector3(currentArrow.transform.position.x, currentArrow.transform.position.y, currentArrow.transform.position.z);
                //currentArrow.transform.position = new Vector3(nochFollow.transform.position.x - tail.transform.position.x, nochStart.transform.position.x, nochStart.transform.position.y);
                //currentArrow.transform.eulerAngles = new Vector3(nochStart.transform.eulerAngles.z, nochStart.transform.eulerAngles.x, nochStart.transform.eulerAngles.y);
            }
        }

        private void OnTriggerEnter(Collider noch)
        {
            //only once per noch
            if (!noched && noch.name == "Tail" && hand.otherHand.currentAttachedObject.name == "Arrow Variant")
            {
                currentArrow = hand.otherHand.currentAttachedObject;
                currentArrow.transform.parent = nochFollow.transform;
                noched = true;
                tail = currentArrow.transform.Find("Whole").Find("Tail").gameObject;
                currentArrow.transform.localPosition = new Vector3(tail.transform.position.x, 0, 0);
                //hand.otherHand.DetachObject(currentArrow);
                //hand.otherHand.AttachObject(nochFollow, GrabTypes.None);
            }
        }
        
        private void OnAttachedToHand( Hand attachedHand )
        {
            hand = attachedHand;
        }
    }
}
