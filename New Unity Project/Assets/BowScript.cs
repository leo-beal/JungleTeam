using System;
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

        public Animator DrawString;

        private GameObject currentArrow;
        private GameObject tail;
        
        private Vector3 arrowOffset = new Vector3(-5, 0, 0);
        // Start is called before the first frame update
        void Start()
        {
            nochCollider = GetComponent<Collider>();
            DrawString.enabled = false;
            //currentArrow.transform.up = nochStart.transform.right;
            //currentArrow.transform.right = nochStart.transform.forward;
            if ( DrawString == null )
            {
                DrawString = GetComponent<Animator>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (currentArrow != null && hand.otherHand.currentAttachedObject == null)
            {
                currentArrow = null;
                if (DrawString.enabled)
                {
                    Debug.Log("reseting animation");
                    DrawString.Play(0, 0, 0.0001f);
                    DrawString.enabled = false;
                }
                noched = false;
            }
            if (noched && hand.otherHand.currentAttachedObject != null && currentArrow != null)
            {
                //hand.objectAttachmentPoint.LookAt();
                hand.currentAttachedObject.transform.LookAt(2 * this.transform.position - hand.otherHand.transform.position); //Probably a better way to do this
                transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 0, .1f);
                float zTransform = Mathf.Clamp(currentArrow.transform.localPosition.z - 7, nochEnd.transform.localPosition.z,
                    nochStart.transform.localPosition.z);
                //nochFollow.transform.position = tail.transform.position;
                currentArrow.transform.localPosition = new Vector3(0, 0, 7 + zTransform);
                currentArrow.transform.localEulerAngles = new Vector3(nochStart.transform.localEulerAngles.x, nochStart.transform.localEulerAngles.y - 90, nochStart.transform.localEulerAngles.z);//nochStart.transform.localRotation;

                float distance = nochStart.transform.localPosition.z - nochEnd.transform.localPosition.z;
                float point = -(currentArrow.transform.localPosition.z - 5.5f) / distance;
                if (Math.Abs(point) > 1)
                {
                    point = .9999f;
                }
                
                
                DrawString.Play(0, 0, point);

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
                DrawString.enabled = true;
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
