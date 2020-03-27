using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Valve.VR.InteractionSystem
{

    [RequireComponent(typeof(Interactable))]
    public class BowScript : MonoBehaviour
    {
        private bool noched;
        private Hand hand;

        public GameObject nockStart;

        public GameObject nockEnd;

        public GameObject nockFollow;

        public Animator DrawString;

        public GameObject fireLoc;

        public GameObject fireLocPrev;

        //private bool firLocPrevGlobal;
        

        private GameObject currentArrow;
        private GameObject pivot;
        private GameObject tail;

        private float point;

        private float velocity = 25f;

        private Vector3 prevLocPos;
        private Vector3 prevLocRot;

        private float dist;

        //private bool nockAnm = false;
        
        private Vector3 arrowOffset = new Vector3(-5, 0, 0);
        // Start is called before the first frame update
        void Start()
        {
            fireLocPrev.transform.parent = null;
            fireLocPrev.transform.position = Vector3.zero;
            fireLocPrev.transform.eulerAngles = Vector3.zero;
            DrawString.enabled = true;
            DrawString.speed = 0.0f;
            if ( DrawString == null )
            {
                DrawString = GetComponent<Animator>();
            }
            
        }

        // Update is called once per frame

        void fire()
        {
            currentArrow.transform.parent = fireLocPrev.transform;

            if (dist > 0.1f)
            {
                ArrowScript arrow = currentArrow.GetComponent<ArrowScript>();

                currentArrow.transform.position = fireLocPrev.transform.position;
                currentArrow.transform.localEulerAngles = prevLocRot + new Vector3(0, 0, 0);

                currentArrow.transform.parent = null;
                    
                    

                arrow.Whole.AddForce(currentArrow.transform.forward * (velocity * dist), ForceMode.VelocityChange);


                arrow.flying = true;
            }
            else
            { 
                currentArrow.transform.parent = null;
            }

            
            
            currentArrow = null;
            dist = 0f;
            DrawString.Play(0, 0, dist);
            noched = false;
        }

        void aim()
        {

            transform.rotation = Quaternion.LookRotation(
                         hand.transform.position - hand.otherHand.transform.position,
                         hand.transform.TransformDirection(Vector3.forward));

            dist = Mathf.Clamp(
                   Vector3.Distance(
                              hand.gameObject.transform.position,
                              hand.otherHand.gameObject.transform.position) * 2, 0, 1.5f);

            dist /= 1.5f;
                
            currentArrow.transform.localPosition = new Vector3(-.642f, 0, -.016f);
            currentArrow.transform.LookAt(fireLoc.transform);
                
            if (dist > 1)
            {
                dist = .9999f;
            }
        
            DrawString.Play(0, 0, dist);
                
            prevLocRot = transform.eulerAngles;

            fireLocPrev.transform.position = fireLoc.transform.position;
        }
        
        void Update()
        {

            if (currentArrow != null && hand.otherHand.currentAttachedObject == null)
            {
                fire(); 
            }
            
            if (noched && currentArrow != null && hand.otherHand.currentAttachedObject != null)
            {

                aim();
            }
        }

        private void OnTriggerEnter(Collider noch)
        {
            if (!noched && noch.name == "Tail" && hand.otherHand.currentAttachedObject.name.Substring(0, 5) == "Arrow")
            {
                currentArrow = hand.otherHand.currentAttachedObject;
                currentArrow.transform.parent = nockFollow.transform;
                noched = true;
                tail = currentArrow.transform.Find("Tail").gameObject;
                currentArrow.transform.localPosition = new Vector3(tail.transform.position.x, 0, 0);
                DrawString.enabled = true;
            }
        }
        
        private void OnAttachedToHand( Hand attachedHand )
        {
            hand = attachedHand;
        }

        private void OnDetachedFromHand(Hand detachedHand)
        {
            Destroy(fireLocPrev.transform.gameObject);
            Destroy(this.transform.gameObject);
        }
    }
}
