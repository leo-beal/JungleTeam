using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{

    [RequireComponent(typeof(Interactable))]
    public class NewBowScript : MonoBehaviour
    {
        private bool noched;
        private Hand hand;

        public Collider nochCollider;

        //public GameObject nochFollow;

        public GameObject nochStart;

        public GameObject nochEnd;

        public Animator DrawString;

        public GameObject fireLoc;

        //public Collider nochCollider;

        private GameObject currentArrow;
        private GameObject pivot;
        private GameObject tail;

        private float point;

        private float velocity = 25f;

        //private bool nockAnm = false;
        
        private Vector3 arrowOffset = new Vector3(-5, 0, 0);
        // Start is called before the first frame update
        void Start()
        {
            //nochCollider = GetComponent<Collider>();
            DrawString.enabled = true;
            DrawString.speed = 0.0f;
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
                currentArrow.transform.parent = null;

                if (point > 0.1f)
                {
                    ArrowScript arrow = currentArrow.GetComponent<ArrowScript>();

                    currentArrow.transform.position = fireLoc.transform.position;


                    arrow.Whole.AddForce(currentArrow.transform.forward * (velocity * point), ForceMode.VelocityChange);
                    //arrow.Whole.AddTorque(currentArrow.transform.forward * 10);


                    arrow.flying = true;
                }

                currentArrow = null;
                point = 0f;
                Debug.Log("reseting animation");
                DrawString.Play(0, 0, point);
                noched = false;
            }
            if (noched && hand.otherHand.currentAttachedObject != null && currentArrow != null)
            {

                transform.rotation = Quaternion.LookRotation(hand.transform.position - hand.otherHand.transform.position, hand.transform.TransformDirection(Vector3.forward));
                
                float zTransform = Mathf.Clamp(currentArrow.transform.localPosition.z - 2, nochEnd.transform.localPosition.z,
                    nochStart.transform.localPosition.z);
                currentArrow.transform.localPosition = new Vector3(0, 0, 2 + zTransform);
                currentArrow.transform.LookAt(fireLoc.transform);

                float distance = nochStart.transform.localPosition.z - nochEnd.transform.localPosition.z;
                point = -(currentArrow.transform.localPosition.z - 5.5f) / distance;
                if (Math.Abs(point) > 1)
                {
                    point = .9999f;
                }
                
                
                DrawString.Play(0, 0, point);
            }
        }

        private void OnTriggerEnter(Collider noch)
        {
            //only once per noch
            if (!noched && noch.name == "Tail" && hand.otherHand.currentAttachedObject.name == "NewArrow")
            {
                currentArrow = hand.otherHand.currentAttachedObject;
                currentArrow.transform.parent = nochStart.transform;
                noched = true;
                tail = currentArrow.transform.Find("Tail").gameObject;
                //pivot = currentArrow.transform.Find("pivot").gameObject;
                currentArrow.transform.localPosition = new Vector3(tail.transform.position.x, 0, 0);
                DrawString.enabled = true;
            }
        }
        
        private void OnAttachedToHand( Hand attachedHand )
        {
            hand = attachedHand;
        }
    }
}
