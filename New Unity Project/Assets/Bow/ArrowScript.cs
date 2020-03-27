using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class ArrowScript : MonoBehaviour
    {
        public bool flying;
        public bool flaming;
        
        //Not activly using these but may still want them
        public GameObject Head;
        public GameObject Shaft;
        public GameObject Tail;

        public Rigidbody Whole;
        private Vector3 prevAngle;


        // Start is called before the first frame update
        void Start()
        {
            flying = false;
            flaming = false;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (flying)
            {
                prevAngle = transform.eulerAngles;
                transform.LookAt(transform.position + Whole.velocity);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            
            flying = false;
            Whole.velocity = Vector3.zero;
            //only once per nock
            
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