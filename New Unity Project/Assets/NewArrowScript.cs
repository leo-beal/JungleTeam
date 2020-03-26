using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class NewArrowScript : MonoBehaviour
    {
        public bool flying;
        public bool flaming;
        public GameObject Head;
        public GameObject Shaft;
        public GameObject Tail;

        public Rigidbody Whole;
        //public Rigidbody HeadRB;
        
        //private Vector3 prevPosition;
        private Quaternion prevRotation;
        //private Vector3 prevVelocity;
        private Vector3 prevAngle;


        // Start is called before the first frame update
        void Start()
        {
            flying = false;
            flaming = false;
            //HeadRB.isKinematic = true;
            //HeadRB.useGravity = false;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (flying)
            {
                //prevPosition = this.transform.position;
                //prevRotation = this.transform.rotation;
                prevAngle = transform.eulerAngles;
                //prevVelocity = this.GetComponent<Rigidbody>().velocity;
                this.transform.LookAt(transform.position + Whole.velocity);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            
            flying = false;
            Whole.velocity = Vector3.zero;
            //only once per noch
            
            if (other.gameObject.tag == "Target")
            {
                transform.eulerAngles = prevAngle;
                transform.parent = other.gameObject.transform;
                Whole.useGravity = false;
                Whole.isKinematic = true;
                
            }
        }
    }
}