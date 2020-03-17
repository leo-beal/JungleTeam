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
        public GameObject Head;
        public GameObject Shaft;
        public GameObject Tail;

        public Rigidbody Whole;
        //public Rigidbody HeadRB;
        
        private Vector3 prevPosition;
        private Quaternion prevRotation;
        private Vector3 prevVelocity;


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
                prevPosition = this.transform.position;
                prevRotation = this.transform.rotation;
                prevVelocity = this.GetComponent<Rigidbody>().velocity;
                this.transform.LookAt(transform.position + Whole.velocity);
            }
        }
    }
}