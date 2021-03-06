﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    
    

    
    [RequireComponent(typeof(Interactable))]
    public class QuiverScript : MonoBehaviour
    {

        public GameObject bow;
        public GameObject arrow;
        private GameObject offset;
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void HandHoverUpdate(Hand hand)
        {
            GrabTypes startingGrabType = hand.GetGrabStarting();

            //Transform off;
            
            

            if (hand.currentAttachedObject == null && startingGrabType != GrabTypes.None)
            {

                if (hand.otherHand.currentAttachedObject == null ||
                    hand.otherHand.currentAttachedObject.name.Substring(0, 3) != "Bow")
                {
                    GameObject newBow = Instantiate(bow);
                    GameObject hold = newBow.transform.Find("Bow").Find("Hold Point").gameObject;
                    hand.AttachObject(newBow, startingGrabType, Hand.defaultAttachmentFlags, hold.transform);
                }
                else
                {
                    GameObject newArrow = Instantiate(arrow);
                    //off.position = newArrow.transform.position - new Vector3(0, 0, 0);
                    //Vector3 off = new Vector3(newArrow.transform.position.x, newArrow.transform.position.y, newArrow.transform.position.z - 5);
                    //arrow.transform.position = off;
                    GameObject tail = newArrow.transform.Find("Tail").gameObject;
                    // Attach this object to the hand
                    hand.AttachObject(newArrow, startingGrabType, Hand.defaultAttachmentFlags, tail.transform);
                }
            }
        }
    }
}
