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

        public GameObject nockFollow;

        public Animator DrawString;

        public GameObject fireLoc;

        public GameObject fireLocPrev;

        //public Collider nochCollider;

        private GameObject currentArrow;
        private GameObject pivot;
        private GameObject tail;

        private float point;

        private float velocity = 25f;

        private bool grabbingArrow;

        private Vector3 prevLocPos;
        private Vector3 prevLocRot;

        private float dist;

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
                currentArrow.transform.parent = fireLocPrev.transform;

                if (dist > 0.1f)
                {
                    NewArrowScript arrow = currentArrow.GetComponent<NewArrowScript>();

                    currentArrow.transform.position = fireLocPrev.transform.position;
                    currentArrow.transform.localEulerAngles = prevLocRot + new Vector3(0, 0, 0);

                    currentArrow.transform.parent = null;
                    
                    

                    arrow.Whole.AddForce(currentArrow.transform.forward * (velocity * dist), ForceMode.VelocityChange);
                    //arrow.Whole.AddTorque(currentArrow.transform.forward * 10);


                    arrow.flying = true;
                }
                else
                { 
                    currentArrow.transform.parent = null;
                }
                
                //hand.otherHand.show();
                
                currentArrow = null;
                dist = 0f;
                Debug.Log("reseting animation");
                DrawString.Play(0, 0, dist);
                noched = false;
            }
            
            if (noched && currentArrow != null && hand.otherHand.currentAttachedObject != null)
            {

                transform.rotation = Quaternion.LookRotation(hand.transform.position - hand.otherHand.transform.position, hand.transform.TransformDirection(Vector3.forward));
                
                float zTransform = Mathf.Clamp(currentArrow.transform.localPosition.x + 0.642f, nochStart.transform.localPosition.x,
                    nochEnd.transform.localPosition.x);

                dist =
                    Mathf.Clamp(
                        Vector3.Distance(hand.gameObject.transform.position,
                            hand.otherHand.gameObject.transform.position) * 2, 0, 1.5f);

                dist /= 1.5f;// = controllDist / 3;
                
                //currentArrow.transform.localPosition = new Vector3(zTransform - 0.642f, 0, 0);
                //currentArrow.transform.position = nockFollow.transform.position;
                currentArrow.transform.localPosition = new Vector3(-.642f, 0, -.016f);
                currentArrow.transform.LookAt(fireLoc.transform);

                float distance = nochEnd.transform.localPosition.x - nochStart.transform.localPosition.x;
                point = (currentArrow.transform.localPosition.x) / distance;
                if (dist > 1)
                {
                    dist = .9999f;
                }
        
                DrawString.Play(0, 0, dist);

                //prevLocPos = fireLoc.transform.localPosition;
                prevLocRot = transform.eulerAngles;
                
                //Debug.Log(prevLocRot);
                Debug.Log(transform.eulerAngles);

                fireLocPrev.transform.position = fireLoc.transform.position;
                //fireLocPrev.transform.localPosition = fireLoc.transform.localPosition;
                //fireLocPrev.transform.rotation = transform.localRotation;
            }
        }

        private void OnTriggerEnter(Collider noch)
        {
            //only once per noch
            if (!noched && noch.name == "Tail" && hand.otherHand.currentAttachedObject.name.Substring(0, 8) == "NewArrow")
            {
                currentArrow = hand.otherHand.currentAttachedObject;
                //hand.otherHand.DetachObject(currentArrow);
                hand.otherHand.Hide();
                //hand.otherHand.Show();
                currentArrow.transform.parent = nockFollow.transform;
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
