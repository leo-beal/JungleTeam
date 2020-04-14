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
        private bool enableFire;
        private bool enabledFire;
        
        //Not activly using these but may still want them
        public GameObject Head;
        public GameObject Shaft;
        public GameObject Tail;

        public Rigidbody Whole;
        private Vector3 prevAngle;

        public GameObject fire;

        // Start is called before the first frame update
        void Start()
        {
            flying = false;
            flaming = false;
            enabledFire = false;
            enableFire = false;
            fire.SetActive(false);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (flying)
            {
                prevAngle = transform.eulerAngles;
                transform.LookAt(transform.position + Whole.velocity);
            }

            if (flaming && !enabledFire)
            {
                enableFire = true;
                enabledFire = true;
            }

            if (enableFire)
            {
                fire.SetActive(true);
                enableFire = false;
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
            if (other.gameObject.CompareTag("Flamable"))
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}